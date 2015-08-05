using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.MultiTenancy;
using Fzrain.Authorization.Users;
using Fzrain.Configuration;

namespace Fzrain.MultiTenancy
{
    public class Tenant : FullAuditedEntity<int, User>, IPassivable     
    {
         /// <summary>
        /// "Default".
        /// </summary>
        public const string DefaultTenantName = "Default";
       
        /// <summary>
        /// "^[a-zA-Z][a-zA-Z0-9_-]{1,}$".
        /// </summary>
        public const string TenancyNameRegex = "^[a-zA-Z][a-zA-Z0-9_-]{1,}$";

        /// <summary>
        /// Max length of the <see cref="TenancyName"/> property.
        /// </summary>
        public const int MaxTenancyNameLength = 64;

        /// <summary>
        /// Max length of the <see cref="Name"/> property.
        /// </summary>
        public const int MaxNameLength = 128;
        
        /// <summary>
        /// Tenancy name. This property is the UNIQUE name of this Tenant.
        /// It can be used as subdomain name in a web application.
        /// </summary>
        [Required]
        [StringLength(MaxTenancyNameLength)]
        public virtual string TenancyName { get; set; }

        /// <summary>
        /// Display name of the Tenant.
        /// </summary>
        [Required]
        [StringLength(MaxNameLength)]
        public virtual string Name { get; set; }

        /// <summary>
        /// Is this tenant active?
        /// If as tenant is not active, no user of this tenant can use the application.
        /// </summary>
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// Defined settings for this tenant.
        /// </summary>
        [ForeignKey("TenantId")]
        public virtual ICollection<Setting> Settings { get; set; }    
    }
}