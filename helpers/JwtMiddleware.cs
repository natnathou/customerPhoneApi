using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using customerPhoneApi.Data;
using customerPhoneApi.models;
using customerPhoneApi.services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace customerPhoneApi.helpers
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IOptions<AppSettings> _appSettings;


        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {


            _appSettings = appSettings;
            _next = next;
        }

        public async Task Invoke(HttpContext context, DataContext dataContext)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split("bearer ").Last();
            if (token != null)
            {
                try
                {
                    attachUserToContext(context, token, dataContext);

                }
                catch
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return;

                }
                await _next(context);

            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }
        }

        private void attachUserToContext(HttpContext context, string token, DataContext dataContext)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Value.Secret);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);
            var jwtToken = (JwtSecurityToken)validatedToken;
            User currentUser = dataContext.Users.FirstOrDefault(u => u.Token == token);
            var identity = new ClaimsIdentity(jwtToken.Claims);
        }


    }
}