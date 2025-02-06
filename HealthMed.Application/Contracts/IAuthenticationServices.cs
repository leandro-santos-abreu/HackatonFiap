using System.Data;

namespace HealthMed.Application.Contracts;
public interface IAuthenticationServices
{
    string Login(string TipoDoc, string usuario, string senha);
}
