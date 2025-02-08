namespace HealthMed.Persistence.Contract;
public interface IAuthenticationRepository
{
    (bool IsValidUser, int userId, string Role) GetUserByLogin(string TipoDoc, string usuario, string senha);
}
