using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using customerPhoneApi.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features.Authentication;
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
                catch (Exception ex)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    System.Console.WriteLine(ex.Message);
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

        private async void attachUserToContext(HttpContext context, string token, DataContext dataContext)
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


            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
            identity.AddClaims(jwtToken.Claims);
            var principal = new ClaimsPrincipal(identity);
            context.User = principal;
            await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties { IsPersistent = true });

        }


    }
};