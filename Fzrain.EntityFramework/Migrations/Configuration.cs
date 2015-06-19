using Fzrain.EntityFramework;
using Fzrain.Users;

namespace Fzrain.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FzrainDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        
        }

        protected override void Seed(Fzrain.EntityFramework.FzrainDbContext context)
        {         
            context.Users.AddOrUpdate(u => u.UserName, new User {UserName = "Admin",Id=1});
            context.SaveChanges();
            // This method will be called every time after migrating to the latest version.
            // You can add any seed data here...
        }
    }
}
