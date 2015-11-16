using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Fzrain.Configuration
{
    /// <summary>
    /// Represents a setting for a tenant or user.
    /// </summary>
    public class Setting : AuditedEntity<long>,IMayHaveTenant
    {
        public Setting(int? tenantId, long? userId, string name, string value)
        {
            TenantId = tenantId;
            UserId = userId;
            Name = name;
            Value = value;
        }

        public Setting()
        {
            
        }
        public  int? TenantId { get; set; }  
        public  long? UserId { get; set; }  
        public  string Name { get; set; }    
        public  string Value { get; set; }

    }
}
