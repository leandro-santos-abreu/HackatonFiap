using HealthMed.Application.Contracts;
using HealthMed.Presentation.Model;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("auth")]
public class AuthenticationController(IAuthenticationServices authenticationServices) : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest loginRequest)
    {
        if(!string.IsNullOrEmpty(loginRequest.CPF) && !string.IsNullOrEmpty(loginRequest.CRM)) return Unauthorized("Login deve ser realizado por CPF ou CRM.");

        var token = authenticationServices.Login(loginRequest.CPF.Length > 10 ? loginRequest.CPF : loginRequest.CRM, loginRequest.Login, loginRequest.Senha);

        if (!string.IsNullOrEmpty(token))
        {
            return Ok(new { Token = token });
        }

        return Unauthorized("Usuário ou senha inválidos.");
    }
}



