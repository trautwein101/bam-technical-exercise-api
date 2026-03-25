using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;
using StargateAPI.Controllers;
using System.Net;

namespace StargateAPI.Business.Commands
{
    public class UpdatePerson : IRequest<UpdatePersonResult>
    {
        public required string CurrentName { get; set; } = string.Empty; 
        public required string Name { get; set; } = string.Empty;
    }

    public class UpdatePersonPreProcessor : IRequestPreProcessor<UpdatePerson>
    {
        private readonly StargateContext _context;
        public UpdatePersonPreProcessor(StargateContext context)
        {
            _context = context;
        }
        public Task Process(UpdatePerson request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.CurrentName))
            {
                throw new BadHttpRequestException("Current name is required");
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new BadHttpRequestException("New name is required");
            }

            var existingPerson = _context.People.AsNoTracking().FirstOrDefault(z => z.Name == request.CurrentName);

            if (existingPerson is null)
            {
                throw new BadHttpRequestException("Person not found");
            }

            var duplicateName = _context.People.AsNoTracking().FirstOrDefault(z => z.Name == request.Name && z.Name != request.CurrentName);

            if (duplicateName is not null)
            {
                throw new BadHttpRequestException("New name already exists");
            }

            return Task.CompletedTask;
        }
    }

    public class UpdatePersonHandler : IRequestHandler<UpdatePerson, UpdatePersonResult>
    {
        private readonly StargateContext _context;

        public UpdatePersonHandler(StargateContext context)
        {
            _context = context;
        }

        public async Task<UpdatePersonResult> Handle(UpdatePerson request, CancellationToken cancellationToken)
        {

            var person = await _context.People
               .FirstOrDefaultAsync(z => z.Name == request.CurrentName, cancellationToken);

            if (person == null)
            {
                return new UpdatePersonResult()
                {
                    Success = false,
                    Message = "Person not found",
                    ResponseCode = (int)HttpStatusCode.NotFound
                };
            }

            person.Name = request.Name;

            _context.People.Update(person);
            await _context.SaveChangesAsync(cancellationToken);

            return new UpdatePersonResult()
            {
                Id = person.Id,
                Message = "Person updated successfully"
            };

        }
    }

    public class UpdatePersonResult : BaseResponse
    {
        public int Id { get; set; }
    }
}
