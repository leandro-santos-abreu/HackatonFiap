using HealthMed.Application.Contracts;
using HealthMed.Application.Services;
using HealthMed.Data;
using HealthMed.Persistence.Contract;
using HealthMed.Persistence.Repository;
using MassTransit;
using Microsoft.IdentityModel.Tokens;
using Prometheus;
using HealthMed.Presentation.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;

namespace HealthMed.Presentation
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
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

            services.AddDbContext<HealthMedContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("HealthMedConnection")));

            services.AddScoped<IMedicoRepository, MedicoRepository>();
            services.AddScoped<IPacienteRepository, PacienteRepository>();
            services.AddScoped<IAgendaRepository, AgendaRepository>();
            services.AddScoped<IAutenticationRepository, AutenticationRepository>();

            services.AddScoped<IMedicoServices, MedicoServices>();
            services.AddScoped<IPacienteServices, PacienteServices>();
            services.AddScoped<IAgendaServices, AgendaServices>();
            services.AddScoped<IAuthenticationServices, AuthenticationServices>();


            // Configuração da autenticação JWT
            var secretKey = Configuration.GetSection("Jwt")["Key"] ?? string.Empty;
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
                    };
                });

            var servidor = Configuration.GetSection("MassTransit")["Servidor"] ?? string.Empty;
            var usuario = Configuration.GetSection("MassTransit")["Usuario"] ?? string.Empty;
            var senha = Configuration.GetSection("MassTransit")["Senha"] ?? string.Empty;

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(new Uri(servidor), "/", h =>
                    {
                        h.Username(usuario);
                        h.Password(senha);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseMetricServer("/metrics");

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = "swagger"; // Set to an empty string to serve Swagger at the app's root
            });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapMetrics();
            });

            app.ApplyMigrations();
        }
    }
}
