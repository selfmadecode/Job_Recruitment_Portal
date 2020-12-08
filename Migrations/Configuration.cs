namespace BigJobbs.Migrations
{
    using BigJobbs.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BigJobbs.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BigJobbs.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            if (!context.Roles.Any(r => r.Name == Roles.admin))
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);

                var role = new IdentityRole
                {
                    Name = Roles.admin,
                };

                roleManager.Create(role);
            }
            if(!context.Users.Any(u => u.UserName == "admin@bigjobbs.com"))
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                var passwordHasher = new PasswordHasher();

                var admin = new ApplicationUser
                {
                    UserName = "admin@bigjobbs.com",
                    Email = "admin@bigjobbs.com",
                    PasswordHash = passwordHasher.HashPassword("admin@bigjobbs.com")
                };
                userManager.Create(admin);
                userManager.AddToRole(admin.Id, Roles.admin);
            }
        }
    }
}
