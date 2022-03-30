namespace BooksRealm.Infrastructure
{
    using BooksRealm.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Threading.Tasks;
    using BooksRealm.Areas.Admin;
    using Microsoft.AspNetCore.Builder;
    using BooksRealm.Data;
    using Microsoft.EntityFrameworkCore;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;

            MigrateDatabase(services);

            SeedAdministrator(services);

            return app;
        }

        private static void MigrateDatabase(IServiceProvider services)
        {
            var data = services.GetRequiredService<BooksRealmDbContext>();

            data.Database.Migrate();
        }

        private static void SeedAdministrator(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<BooksRealmUser>>();
            var roleManager = services.GetRequiredService<RoleManager<BooksRealmRole>>();

            Task
                .Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync(AdminConstants.AdministratorRoleName))
                    {
                        return;
                    }

                    var role = new BooksRealmRole { Name = AdminConstants.AdministratorRoleName };

                    await roleManager.CreateAsync(role);

                    const string adminEmail = "admin@google.com";
                    const string adminPassword = "admin12";

                    var user = new BooksRealmUser
                    {
                        Email = adminEmail,
                        UserName = adminEmail,
                        
                    };

                    await userManager.CreateAsync(user, adminPassword);

                    await userManager.AddToRoleAsync(user, role.Name);
                })
                .GetAwaiter()
                .GetResult();
        }
    }
}
