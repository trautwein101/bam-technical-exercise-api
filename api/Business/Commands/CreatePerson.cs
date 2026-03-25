using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using StargateAPI.Business.Data;
using StargateAPI.Controllers;
using StargateAPI.Business.IServices;

namespace StargateAPI.Business.Commands
{

    public class CreatePerson : IRequest<CreatePersonResult>
    {
        public required string Name { get; set; } = string.Empty;
    }

    public class CreatePersonPreProcessor : IRequestPreProcessor<CreatePerson>
    {
        private readonly StargateContext _context;

        public CreatePersonPreProcessor(StargateContext context)
        {
            _context = context;
        }
        public Task Process(CreatePerson request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new BadHttpRequestException("Name is required");
            }
            
            var person = _context.People.AsNoTracking().FirstOrDefault(z => z.Name == request.Name);

            if (person is not null) throw new BadHttpRequestException("Person already exists");

            return Task.CompletedTask;
        }
    }

    public class CreatePersonHandler : IRequestHandler<CreatePerson, CreatePersonResult>
    {
        private readonly StargateContext _context;
        private readonly IProcessLogService _processLogService;

        public CreatePersonHandler(StargateContext context, IProcessLogService processLogService)
        {
            _context = context;
            _processLogService = processLogService;
        }
        public async Task<CreatePersonResult> Handle(CreatePerson request, CancellationToken cancellationToken)
        {

                var newPerson = new Person()
                {
                   Name = request.Name
                };

                await _context.People.AddAsync(newPerson);

                await _context.SaveChangesAsync(cancellationToken);

                await _processLogService.LogAsync("CreatePerson", "Success", $"Created person '{newPerson.Name}'", cancellationToken);

                return new CreatePersonResult()
                {
                    Id = newPerson.Id
                };
          
        }
    }

    public class CreatePersonResult : BaseResponse
    {
        public int Id { get; set; }
    }
}
