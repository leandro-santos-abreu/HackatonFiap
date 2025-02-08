using HealthMed.Application.Contracts;
using Microsoft.AspNetCore.Mvc;
using LoginRequest = HealthMed.Presentation.Model.LoginRequest;

[ApiController]
[Route("auth")]
public class AuthenticationController(IAuthenticationServices authenticationServices) : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest loginRequest)
    {
        if (!ValidatePacienteLoginRequest(loginRequest) && !ValidateMedicoLoginRequest(loginRequest)) return Unauthorized("Login deve ser realizado por CPF/Email ou CRM/Email.");

        var token = authenticationServices.Login(loginRequest.CPF.Length > 10 ? loginRequest.CPF : loginRequest.CRM, loginRequest.Login, loginRequest.Senha);

        if (!string.IsNullOrEmpty(token))
        {
            return Ok(new { Token = token });
        }

        return Unauthorized("Usuário ou senha inválidos.");
    }

    private static bool ValidatePacienteLoginRequest(LoginRequest loginRequest)
    {
        if (string.IsNullOrEmpty(loginRequest.CPF) && string.IsNullOrEmpty(loginRequest.Login))
        {
            return false;
        }
        return true;
    }

    private static bool ValidateMedicoLoginRequest(LoginRequest loginRequest)
    {
        if (string.IsNullOrEmpty(loginRequest.CRM) && string.IsNullOrEmpty(loginRequest.Login))
        {
            return false;
        }
        return true;
    }

}



