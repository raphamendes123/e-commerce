using Core.Domain.ResponseResult;

namespace Front.MVC.Models
{
    public class UserLoginResponseViewModel
    {
        public string? AccessToken { get; set; }
        public double ExpiresIn { get; set; }
        public UsuarioToken? UsuarioToken { get; set; }
        public ResponseResult? ResponseResult { get; set; }
        public string RefreshToken { get; set; }
    }

    public class UsuarioToken
    {
        public string? Id { get; set; }
        public string? Email { get; set; }
        public IEnumerable<UsuarioClaim>? Claims { get; set; }
    }

    public class UsuarioClaim
    {
        public string? Value { get; set; }
        public string? Type { get; set; }
    }
}
