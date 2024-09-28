namespace Core.SpecificationsUseCase
{
    public class ValidationError
    {
        public string Name { get; set; }

        public string Message { get; set; }

        public ValidationError(string name, string message)
        {
            Name = name;
            Message = message;
        }
    }
}
