namespace Core.Jwt
{
    public class JwksSettings
    {
        public string? AuthenticationJwksUrl { get; set; }
        public string? Issuer { get; set; } //Emissor
        public string? Audience { get; set; } //Audience
        public int? RefreshTokenExpiration { get; set; } //RefreshTokenExpiration
    }
}
