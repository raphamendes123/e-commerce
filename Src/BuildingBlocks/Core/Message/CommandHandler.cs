using Core.Domain.Repository.Data;
using FluentValidation.Results;

namespace Core.Message
{
    public abstract class CommandHandler
    {
        protected ValidationResult ValidationResult;

        protected CommandHandler()
        {
            ValidationResult = new ValidationResult();
        }

        protected void AddError(string message)
        {
            ValidationResult.Errors.Add(new ValidationFailure(string.Empty, message));
        }
        protected void AddError(List<ValidationFailure> listValidationFailure)
        {
            ValidationResult.Errors.AddRange(listValidationFailure);
        }
        protected async Task<ValidationResult> PersistData(IUnitOfWork uow)
        {
            if (!await uow.Commit())
                AddError("Error commit - 0 rows affected in the database - An error occurred while trying to persist data");

            return ValidationResult;
        }
    }
}
