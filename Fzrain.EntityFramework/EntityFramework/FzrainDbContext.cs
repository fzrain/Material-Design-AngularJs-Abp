using System.Data.Entity;
using Abp.EntityFramework;
using Abp.Zero.EntityFramework;
using Fzrain.Authorization.Roles;
using Fzrain.MultiTenancy;
using Fzrain.Users;

namespace Fzrain.EntityFramework
{
    public class FzrainDbContext : AbpZeroDbContext<Tenant,Role,User>
    {
        //TODO: Define an IDbSet for each Entity...

        //Example:
        //public virtual IDbSet<User> Users { get; set; }      
        /* NOTE: 
         *   Setting "Default" to base class helps us when working migration commands on Package Manager Console.
         *   But it may cause problems when working Migrate.exe of EF. If you will apply migrations on command line, do not
         *   pass connection string name to base classes. ABP works either way.
         */
        public FzrainDbContext()
            : base("Default")
        {

        }

        /* NOTE:
         *   This constructor is used by ABP to pass connection string defined in FzrainDataModule.PreInitialize.
         *   Notice that, actually you will not directly create an instance of FzrainDbContext since ABP automatically handles it.
         */
        public FzrainDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }
    }
}
