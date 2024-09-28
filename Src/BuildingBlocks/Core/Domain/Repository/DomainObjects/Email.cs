using System.Text.RegularExpressions;

namespace Core.Domain.Repository.DomainObjects
{
    public class Email
    {
        public const int EmailMaxLength = 254;
        public string? Address { get; private set; }

        //Constructor do entityFramework
        protected Email()
        {

        }

        public Email(string address)
        {
            if (!IsValid(address))
                throw new DomainException("E-mail invalid.");

            Address = address;
        }

        public static bool IsValid(string address)
        {
            return new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").IsMatch(address);
        }

    }
}
