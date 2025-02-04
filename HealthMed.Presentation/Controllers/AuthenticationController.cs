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
        var token = authenticationServices.Login(loginRequest.Login, loginRequest.Senha);

        if (!string.IsNullOrEmpty(token))
        {
            return Ok(new { Token = token });
        }

        return Unauthorized("Usuário ou senha inválidos.");
    }
}



