using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace HealthMed.Presentation.Middleware;
public class TokenValidarionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TokenValidarionMiddleware> _logger;
    private readonly IConfiguration _config;

    public TokenValidarionMiddleware(RequestDelegate next, ILogger<TokenValidarionMiddleware> logger, IConfiguration config)
    {
        _next = next;
        _logger = logger;
        _config = config;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Permitir que requisições ao endpoint de login passem sem validação
        if (context.Request.Path.StartsWithSegments("/auth/login", StringComparison.OrdinalIgnoreCase))
        {
            await _next(context); // Ignora a validação do token para este endpoint
            return;
        }

        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (string.IsNullOrEmpty(token))
        {
            _logger.LogWarning("Token não fornecido.");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Token não fornecido.");
            return;
        }

        try
        {
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            }, out _);

            await _next(context); // Prossegue se o token for válido
        }
        catch (SecurityTokenException ex)
        {
            _logger.LogError($"Erro ao validar token: {ex.Message}");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Token inválido ou expirado.");
        }
    }
}
