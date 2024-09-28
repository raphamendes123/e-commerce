namespace Core.Extensions
{
    public static class StringExtensions
    {
        public static string JustNumbers(this string value, string input)
        {
            return new string(input.Where(char.IsDigit).ToArray());
        }
    }
}
