namespace HealthMed.Persistence.Contract;
public interface IAutenticationRepository
{
    (bool IsValidUser, string Role) GetUserByLogin(string usuario, string senha);
}
