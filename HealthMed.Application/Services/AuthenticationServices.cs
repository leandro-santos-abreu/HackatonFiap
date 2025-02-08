using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using HealthMed.Application.Contracts;
using HealthMed.Persistence.Contract;
using MeuProjeto.Domain.Validations;
using HealthMed.Domain.Validations;

namespace HealthMed.Application.Services
{
    public class AuthenticationServices(IAuthenticationRepository autenticationRepository, IConfiguration config) : IAuthenticationServices
    {
        public string Login(string TipoDoc, string usuario, string senha)
        {
            if (!string.IsNullOrEmpty(TipoDoc) && TipoDoc.Length > 10){
                DocumentValidator.IsValidCpf(TipoDoc);
            }DocumentValidator.IsValidCRM(TipoDoc);



            var (isValidUser, userId, role) = autenticationRepository.GetUserByLogin(TipoDoc, usuario, senha);

            if (isValidUser)
            {
                var token = GenerateToken(userId, usuario, role);
                return token;
            }

            return string.Empty; // Retorna vazio se o usuário não for autenticado
        }

        private string GenerateToken(int userId, string username, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(config["Jwt:Key"]);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()), // Agora armazenamos o ID do usuário corretamente
            new Claim(ClaimTypes.Name, username),
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
