//using HealthMed.Presentation;

//public class Program
//{
//    public static void Main(string[] args)
//    {
//        CreateHostBuilder(args).Build().Run();
//    }

//    public static IHostBuilder CreateHostBuilder(string[] args) =>
//        Host.CreateDefaultBuilder(args)
//            .ConfigureAppConfiguration((context, config) =>
//            {
//                config.SetBasePath(Directory.GetCurrentDirectory());
//                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
//                config.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true);
//                config.AddEnvironmentVariables();

//                if (args != null)
//                {
//                    config.AddCommandLine(args);
//                }

//            })
//            .ConfigureWebHostDefaults(webBuilder =>
//            {
//                webBuilder.ConfigureKestrel(serverOptions =>
//                {
//                    serverOptions.ListenAnyIP(8080);  // HTTP port
//                    serverOptions.ListenAnyIP(8443, listenOptions =>  // HTTPS port
//                    {
//                        var certPath = Environment.GetEnvironmentVariable("ASPNETCORE_Kestrel__Certificates__Default__Path");
//                        var certPassword = Environment.GetEnvironmentVariable("ASPNETCORE_Kestrel__Certificates__Default__Password");
//                        listenOptions.UseHttps(certPath!, certPassword);
//                    });
//                });

//                webBuilder.UseStartup<Startup>();
//            });
//}

using HealthMed.Application.Contracts;
using HealthMed.Application.Services;
using HealthMed.Data;
using HealthMed.Persistence.Contract;
using HealthMed.Persistence.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IMedicoServices, MedicoServices>();
builder.Services.AddTransient<IMedicoRepository, MedicoRepository>();
builder.Services.AddTransient<IPacienteServices, PacienteServices>();
builder.Services.AddTransient<IPacienteRepository, PacienteRepository>();
builder.Services.AddTransient<IAgendaServices, AgendaServices>();
builder.Services.AddTransient<IAgendaRepository, AgendaRepository>();
builder.Services.AddTransient<IAuthenticationServices, AuthenticationServices>();
builder.Services.AddTransient<IAuthenticationRepository, AuthenticationRepository>();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Health Med API", Version = "v1" });

    // Configuração para suportar JWT no Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT no campo abaixo: Bearer {seu token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddDbContext<HealthMedContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HealthMedConnection")));

builder.Services.
    AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Configuração da autenticação JWT
var secretKey = builder.Configuration["Jwt:Key"];
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
