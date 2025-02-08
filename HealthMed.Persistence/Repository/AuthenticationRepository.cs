using HealthMed.Data;
using HealthMed.Domain.Entity;
using HealthMed.Persistence.Contract;
using Microsoft.EntityFrameworkCore;



namespace HealthMed.Persistence.Repository;
public class AuthenticationRepository(HealthMedContext db) : IAuthenticationRepository
{
    public (bool IsValidUser, int userId, string Role) GetUserByLogin(string TipoDoc, string usuario, string senha)
    {
        //VerificaUsuariosComSenhaHash();
        string doc = TipoDoc.Length > 10 ? "CPF" : "CRM";

        // Primeiro verifica se é um médico
        if(doc == "CPF")
        {
            var paciente = db.Paciente.FirstOrDefault(x => x.Email == usuario && x.CPF == TipoDoc);
            if (paciente != null && BCrypt.Net.BCrypt.Verify(senha, paciente.Senha))
            {
                return (true,paciente.IdPaciente, "paciente");
            }
        }
        else
        {
            var medico = db.Medico.FirstOrDefault(x => x.Email == usuario && x.CRM == TipoDoc);
            if (medico != null && BCrypt.Net.BCrypt.Verify(senha, medico.Senha))
            {
                return (true,medico.IdMedico, "medico");
            }
        }

        // Se não for médico, verifica se é um paciente
        return (false,0, string.Empty); // Usuário não encontrado ou senha incorreta
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

