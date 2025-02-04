using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using HealthMed.Application.Contracts;
using HealthMed.Persistence.Contract;

namespace HealthMed.Application.Services
{
    public class AuthenticationServices(IAutenticationRepository autenticationRepository, IConfiguration config) : IAuthenticationServices
    {
        public string Login(string usuario, string senha)
        {
            var (isValidUser, role) = autenticationRepository.GetUserByLogin(usuario, senha);

            if (isValidUser)
            {
                var token = GenerateToken(usuario, role);
                return token;
            }

            return string.Empty; // Retorna vazio se o usuário não for autenticado
        }

        private string GenerateToken(string username, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(config["Jwt:Key"]);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, username),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var token = new JwtSecurityToken(
                signingCredentials: credentials,
                expires: DateTime.UtcNow.AddMinutes(30),
                claims: claims
            );

            return tokenHandler.WriteToken(token);
        }
    }

}
