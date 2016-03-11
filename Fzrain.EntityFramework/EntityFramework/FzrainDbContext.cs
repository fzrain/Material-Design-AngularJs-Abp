using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;
using Abp.Zero.EntityFramework;
using Fzrain.Authorization.Roles;
using Fzrain.MultiTenancy;
using Fzrain.Permissions;
using Fzrain.Users;

namespace Fzrain.EntityFramework
{
    public class FzrainDbContext : AbpZeroDbContext<Tenant, Role, User>
    {
       
        //TODO: Define an IDbSet for each Entity...
     
        public virtual IDbSet<PermissionInfo> PermissionInfos { get; set; }
        /* NOTE: 
         *   Setting "Default" to base class helps us when working migration commands on Package Manager Console.
         *   But it may cause problems when working Migrate.exe of EF. If you will apply migrations on command line, do not
         *   pass connection string name to base classes. ABP works either way.
         */
        public FzrainDbContext()
            : base("Default")
        {
            //Configuration.LazyLoadingEnabled = false;
           // Configuration.ProxyCreationEnabled = false;
        }

        /* NOTE:
         *   This constructor is used by ABP to pass connection string defined in FzrainDataModule.PreInitialize.
         *   Notice that, actually you will not directly create an instance of FzrainDbContext since ABP automatically handles it.
         */
        public FzrainDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {  
            //动态加载配置信息
            //System.Type configType = typeof(LanguageMap);   //any of your configuration classes here
            //var typesToRegister = Assembly.GetAssembly(configType).GetTypes()
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => !string.IsNullOrEmpty(type.Namespace))
            .Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }
            //常规配置法
            //modelBuilder.Configurations.Add(new LanguageMap());
            base.OnModelCreating(modelBuilder);
            modelBuilder.ChangeAbpTablePrefix<Tenant, Role, User>("");
        }
    }
}
