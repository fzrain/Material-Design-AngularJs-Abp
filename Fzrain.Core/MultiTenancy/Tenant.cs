using System.Collections.Generic;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Fzrain.Authorization.Users;
using Fzrain.Configuration;

namespace Fzrain.MultiTenancy
{
    public class Tenant : FullAuditedEntity<int, User>, IPassivable
    {
        public const string DefaultTenantName = "Default";
        public const string TenancyNameRegex = "^[a-zA-Z][a-zA-Z0-9_-]{1,}$";

        /// <summary>
        /// Tenancy name. This property is the UNIQUE name of this Tenant.
        /// It can be used as subdomain name in a web application.
        /// </summary>  
        public virtual string TenancyName { get; set; }
    
        public virtual string Name { get; set; }

        /// <summary>
        /// Is this tenant active?
        /// If as tenant is not active, no user of this tenant can use the application.
        /// </summary>
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// Defined settings for this tenant.
        /// </summary>      
        public virtual ICollection<Setting> Settings { get; set; }    
    }
}