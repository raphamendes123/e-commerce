using Core.Message;
using FluentValidation.Results;

namespace Core.Mediator
{
    public interface IMediatorHandler
    {
        Task PublishEvent<EVENTO>(EVENTO evento) where EVENTO : Event;

        Task<ValidationResult> SendCommand<COMMAND>(COMMAND commando) where COMMAND : Command;
    }
}
