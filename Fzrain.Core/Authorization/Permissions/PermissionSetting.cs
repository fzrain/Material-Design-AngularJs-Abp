using Abp.Domain.Entities.Auditing;

namespace Fzrain.Authorization.Permissions
{
    /// <summary>
    /// Used to grant/deny a permission for a role or user.
    /// </summary>
    public class PermissionSetting : CreationAuditedEntity<long>
    {     
        public virtual string Name { get; set; }     
        public bool IsGranted { get; set; } = true;
        public virtual long? UserId { get; set; }
        public virtual int? RoleId { get; set; }

     
    }
}
