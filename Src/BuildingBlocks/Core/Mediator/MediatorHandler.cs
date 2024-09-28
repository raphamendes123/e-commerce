using Core.Message;
using FluentValidation.Results;
using MediatR;
namespace Core.Mediator
{
    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;

        public MediatorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        public async Task<ValidationResult> SendCommand<COMMAND>(COMMAND commando) where COMMAND : Command
        {
           return await _mediator.Send(commando);
        }

        public async Task PublishEvent<EVENTO>(EVENTO evento) where EVENTO : Event
        {
            await _mediator.Publish(evento);
        }
         
    }
}
