namespace Core.SpecificationsUseCase
{
    public class ValidationUseCase
    {
        public string Message { get; set; }

        public bool IsValid => !Errors.Any();

        public IEnumerable<ValidationError> Errors { get; private set; }

        public ValidationUseCase()
        {
            Errors = new ValidationError[0];
        }

        public void Add(ValidationError error)
        {
            List<ValidationError> errors = new List<ValidationError>(Errors) { error };
            SetErrors(errors);
        }

        public void Add(params ValidationUseCase[] validationResults)
        {
            List<ValidationError> list = new List<ValidationError>(Errors);
            foreach (ValidationUseCase validationResult in validationResults)
            {
                list.AddRange(validationResult.Errors);
            }

            SetErrors(list);
        }

        public void Remove(ValidationError error)
        {
            List<ValidationError> list = new List<ValidationError>(Errors);
            list.Remove(error);
            SetErrors(list);
        }

        private void SetErrors(List<ValidationError> errors)
        {
            Errors = errors;
            if (!IsValid)
            {
                Message = errors[0].Message;
            }
        }
    }
}
