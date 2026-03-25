using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Data;
using StargateAPI.Business.IServices;
using Xunit;


namespace StargateAPI.Tests.Commands
{

    public class CreatePersonTests
    {

        private static StargateContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<StargateContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new StargateContext(options);
        }


        [Fact] // PreProcessor — empty name
        public async Task Process_EmptyName_ThrowsBadHttpRequestException()
        {
            //ARRANGE
            using var context = CreateInMemoryContext();
            var preProcessor = new CreatePersonPreProcessor(context);

            var request = new CreatePerson
            {
                Name = ""
            };

            //ACT
            Func<Task> act = async () => await preProcessor.Process(request, CancellationToken.None);

            //ASSERT 
            await act.Should().ThrowAsync<BadHttpRequestException>()
                .WithMessage("Name is required");
        }


        [Fact]
        public async Task Process_DuplicatePerson_ThrowsBadHttpRequestException()
        {
            using var context = CreateInMemoryContext();

            context.People.Add(new Person
            {
                Id = 1,
                Name = "John Doe"
            });

            await context.SaveChangesAsync();

            var preProcessor = new CreatePersonPreProcessor(context);

            var request = new CreatePerson
            {
                Name = "John Doe"
            };

            Func<Task> act = async () => await preProcessor.Process(request, CancellationToken.None);

            await act.Should().ThrowAsync<BadHttpRequestException>()
                .WithMessage("Person already exists");
        }



        [Fact] // PreProcessor — valid request
        public async Task Process_ValidRequest_CompletesSuccessfully()
        {
            //ARRANGE
            using var context = CreateInMemoryContext();
            var preProcessor = new CreatePersonPreProcessor(context);

            var request = new CreatePerson
            {
                Name = "Jason Trautwein"
            };

            //ACT
            var act = async () => await preProcessor.Process(request, CancellationToken.None);

            //ASSERT
            await act.Should().NotThrowAsync();
        }



        [Fact] // Handler — success
        public async Task Handle_ValidRequest_CreatesPersonAndLogsSuccess()
        {
            //ARRANGE
            using var context = CreateInMemoryContext();

            var processLogServiceMock = new Mock<IProcessLogService>();

            var handler = new CreatePersonHandler(context, processLogServiceMock.Object);

            var request = new CreatePerson
            {
                Name = "Smithy Jones"
            };

            //ACT
            var result = await handler.Handle(request, CancellationToken.None);

            //ASSERT
            result.Id.Should().BeGreaterThan(0);

            var createdPerson = await context.People.FirstOrDefaultAsync(x => x.Name == "Smithy Jones");
            createdPerson.Should().NotBeNull();

            processLogServiceMock.Verify(x => 
                x.LogAsync("CreatePerson", "Success", $"Created person 'Smithy Jones'", CancellationToken.None),
                Times.Once());
        }


    }

}