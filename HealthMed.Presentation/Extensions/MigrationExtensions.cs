using HealthMed.Data;
using Microsoft.EntityFrameworkCore;

namespace HealthMed.Presentation.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder webApplication)
        {
            using var scope = webApplication.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<HealthMedContext>();

            dbContext.Database.Migrate();
        }
    }
}
