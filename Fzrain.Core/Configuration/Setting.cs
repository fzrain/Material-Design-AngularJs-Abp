using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;

namespace Fzrain.Configuration
{
    /// <summary>
    /// Represents a setting for a tenant or user.
    /// </summary>
    [Table("Settings")]
    public class Setting : AuditedEntity<long>
    {
        /// <summary>
        /// Maximum length of the <see cref="Name"/> property.
        /// </summary>
        public const int MaxNameLength = 256;

        /// <summary>
        /// Maximum length of the <see cref="Value"/> property.
        /// </summary>
        public const int MaxValueLength = 2000;

        /// <summary>
        /// TenantId for this setting.
        /// TenantId is null if this setting is not Tenant level.
        /// </summary>
        public virtual int? TenantId { get; set; }

        /// <summary>
        /// UserId for this setting.
        /// UserId is null if this setting is not user level.
        /// </summary>
        public virtual long? UserId { get; set; }

        /// <summary>
        /// Unique name of the setting.
        /// </summary>
        [Required]
        [MaxLength(MaxNameLength)]
        public virtual string Name { get; set; }

        /// <summary>
        /// Value of the setting.
        /// </summary>
        [MaxLength(MaxValueLength)]
        public virtual string Value { get; set; }

        /// <summary>
        /// Creates a new <see cref="Setting"/> object.
        /// </summary>
        public Setting()
        {

        }    
    }
}
