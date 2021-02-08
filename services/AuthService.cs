using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using customerPhoneApi.Data;
using customerPhoneApi.Dtos;
using customerPhoneApi.helpers;
using customerPhoneApi.models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace customerPhoneApi.services
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly IOptions<AppSettings> _appSettings;
        public AuthService(IOptions<AppSettings> appSettings, DataContext context)
        {
            _appSettings = appSettings;
            _context = context;
        }

        public async Task<ResponseService<string>> Authenticate(UserLoginDto user)
        {
            System.Console.WriteLine(_appSettings);
            var response = new ResponseService<string>();
            User currentUser = _context.Users.FirstOrDefault(u => u.Username == user.Username);
            if (currentUser == null)
            {
                response.Success = false;
                response.Message = "User doesn't exist";
            }
            else if (!VerifyPasswordHash(user.Password, currentUser.PasswordHash, currentUser.Salt))
            {
                response.Success = false;
                response.Message = "Wrong password";
            }
            else
            {
                var token = createToken(currentUser);
                currentUser.Token = token;
                await _context.SaveChangesAsync();

                response.Data = token;
                response.Success = true;
                response.Message = "Authentication successfull!";
            }
            return response;

        }


        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computeHash.Length; i++)
                {
                    if (computeHash[i] != passwordHash[i])
                    {
                        return false;
                    }

                }
                return true;
            }
        }

        private string createToken(User user)
        {
            List<Claim> claims = new List<Claim>{
                new Claim("Id", user.Id.ToString()),
                new Claim("username", user.Username),
                new Claim("role", user.Role)
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(_appSettings.Value.Secret)
                        );
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}