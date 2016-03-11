using EntityFramework.DynamicFilters;
using Fzrain.EntityFramework;

namespace Fzrain.Migrations.SeedData
{
    public class InitialDataBuilder
    {
        private readonly FzrainDbContext _context;

        public InitialDataBuilder(FzrainDbContext context)
        {
            _context = context;
        }

        public void Build()
        {
            _context.DisableAllFilters();

            new DefaultEditionsBuilder(_context).Build();
            new DefaultTenantRoleAndUserBuilder(_context).Build();
            new DefaultLanguagesBuilder(_context).Build();
        }
    }
}
