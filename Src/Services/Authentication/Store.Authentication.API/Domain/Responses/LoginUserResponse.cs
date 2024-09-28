﻿using Store.Authentication.API.Domain.Data;

namespace Store.Authentication.API.Domain.Responses
{
    public class LoginUserResponse
    {
        public string? AccessToken { get; set; }
        public double ExpiresIn { get; set; }
        public UsuarioToken? UsuarioToken { get; set; }
        public Guid? RefreshToken { get; set; }

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
