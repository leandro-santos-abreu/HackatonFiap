using HealthMed.Data;
using HealthMed.Domain.Entity;
using HealthMed.Persistence.Contract;
using Microsoft.EntityFrameworkCore;



namespace HealthMed.Persistence.Repository;
public class AutenticationRepository(HealthMedContext db) : IAutenticationRepository
{
    public (bool IsValidUser, string Role) GetUserByLogin(string usuario, string senha)
    {
        //VerificaUsuariosComSenhaHash();
        // Primeiro tenta encontrar como médico
        var medico = db.Medico.FirstOrDefault(x => x.Email == usuario);
        
        if (medico != null)
        {          

            if (BCrypt.Net.BCrypt.Verify(senha, medico.Senha))
            {
                return (true, "medico");
            }
            return (false, "medico"); // Usuário encontrado, mas senha errada
        }

        // Se não encontrou como médico, tenta como paciente
        var paciente = db.Paciente.FirstOrDefault(x => x.Email == usuario);
        if (paciente != null)
        {
            if (BCrypt.Net.BCrypt.Verify(senha, paciente.Senha))
            {
                return (true, "paciente");
            }
            return (false, "paciente"); // Usuário encontrado, mas senha errada
        }

        return (false, string.Empty); // Usuário não encontrado
    }

    internal void VerificaUsuariosComSenhaHash()
    {
        var usuarios = db.Medico.ToList();
        foreach (var usuario in usuarios)
        {
            usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);
        }
        db.SaveChanges();

        var pacientes = db.Paciente.ToList();
        foreach (var p in pacientes)
        {
            p.Senha = BCrypt.Net.BCrypt.HashPassword(p.Senha);
        }
        db.SaveChanges();
    }
}

