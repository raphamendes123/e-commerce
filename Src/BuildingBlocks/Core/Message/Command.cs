using FluentValidation.Results;
using MediatR;

namespace Core.Message
{
    // Vai retornar uma validaçao - IRequest<ValidationResult>
    public abstract class Command : Message, IRequest<ValidationResult>
    {
        public DateTime Timestamp { get; set; }

        public ValidationResult ValidationResult { get; set; } = new ValidationResult();

        protected Command()
        {
            Timestamp = DateTime.Now;
        }

        //valite not implemented
        public virtual bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
