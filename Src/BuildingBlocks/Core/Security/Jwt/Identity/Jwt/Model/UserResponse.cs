﻿namespace Core.Security.Jwt.Identity.Jwt.Model
{
    public class UserResponse
    {
        public string AccessToken { get; set; }
        public double ExpiresIn { get; set; }
        public UserToken UserToken { get; set; }
        public string RefreshToken { get; set; }
    }
}