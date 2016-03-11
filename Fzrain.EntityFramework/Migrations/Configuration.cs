using System.Data.Entity.Migrations;
using Fzrain.EntityFramework;
using Fzrain.Migrations.SeedData;

namespace Fzrain.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<FzrainDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Fzrain";
        }

        protected override void Seed(FzrainDbContext context)
        {
            new InitialDataBuilder(context).Build();
        }
    }
}
