namespace Core.Security.Jwt.Identity.NovaPasta
{
    public class JwtSettings
    {
        public string? Secret { get; set; } //Segredo
        public int ExpirationHours { get; set; } //ExpiracaoHoras
        public string? Issuer { get; set; } //Emissor
        public string? Audience { get; set; } //Audiencia
    }
}
