﻿using Microsoft.IdentityModel.Tokens;
using NLimit.WebApi.Services.UserAuthentication;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace NLimit.WebApi.Services.Middleware;

public class JWTMiddleware
{
    private readonly RequestDelegate next;
    private readonly IConfiguration configuration;
    private readonly IUserService userService;

    public JWTMiddleware(RequestDelegate next, IConfiguration configuration, IUserService userService)
    {
        this.next = next;
        this.configuration = configuration;
        this.userService = userService;
    }

    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"]
            .FirstOrDefault()?.Split(" ")
            .Last();

        if (token is not null)
        {
            attachAccountToContext(context, token);
        }

        await next(context);
    }

    private void attachAccountToContext(HttpContext context, string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;

            var accountId = jwtToken.Claims
                .First(x => x.Type == "id")
                .Value;

            var accountPassword = jwtToken.Claims
                .First(y => y.Type == "password")
                .Value;

            // attach account to context on successful jwt validation
            context.Items["User"] = userService.GetUserDetails(accountId, accountPassword, configuration);
        }
        catch
        {
            // do nothing if jwt validation fails
            // account is not attached to context so request won't have access to secure routes
        }
    }
}
