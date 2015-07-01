using EntityFramework.DynamicFilters;
using Fzrain.EntityFramework;
using Fzrain.Migrations.SeedData;
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

        protected override void Seed(FzrainDbContext context)
        {
            context.DisableAllFilters();
            new InitialDataBuilder(context).Build();       
            // This method will be called every time after migrating to the latest version.
            // You can add any seed data here...
        }
    }
}
