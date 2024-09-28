﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.DependencyInjection;
using Core.Security.Core.Interfaces;

namespace Core.Security.AspNetCore;

public class JwtServiceValidationHandler : JwtSecurityTokenHandler
{
    private readonly IServiceProvider _serviceProvider;

    public JwtServiceValidationHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public override ClaimsPrincipal ValidateToken(string token, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var jwtService = scope.ServiceProvider.GetRequiredService<IJwtService>();

        //We can read the token before we've begun validating it.
        //JwtSecurityToken incomingToken = ReadJwtToken(token);

        //Retrieve the corresponding Public Key from our data store
        var keyMaterialTask = jwtService.GetLastKeys();
        Task.WaitAll(keyMaterialTask);
        validationParameters.IssuerSigningKeys = keyMaterialTask.Result.Select(s => s.GetSecurityKey());

        //And let the framework take it from here.
        //var handler = new JsonWebTokenHandler();
        //var result = handler.ValidateToken(token, validationParameters);
        //validatedToken = result.SecurityToken;

        //return new ClaimsPrincipal(result.ClaimsIdentity);
        return base.ValidateToken(token, validationParameters, out validatedToken);
    }
}