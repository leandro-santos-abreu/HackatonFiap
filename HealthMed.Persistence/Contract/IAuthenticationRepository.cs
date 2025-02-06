namespace HealthMed.Persistence.Contract;
public interface IAuthenticationRepository
{
    (bool IsValidUser, string Role) GetUserByLogin(string TipoDoc, string usuario, string senha);
}
