using System.Data;

namespace HealthMed.Application.Contracts;
public interface IAuthenticationServices
{
    string Login(string usuario, string senha);
}
