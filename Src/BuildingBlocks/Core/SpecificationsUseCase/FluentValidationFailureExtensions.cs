using FluentValidation.Results;

namespace Core.SpecificationsUseCase
{
    public static class FluentValidationFailureExtensions
    {
        public static List<ValidationFailure> ToValidationFailure(this IEnumerable<ValidationError> errors)
        {
            return errors.Select(a => new ValidationFailure(a.Name, a.Message)).ToList();
        }
    }
}