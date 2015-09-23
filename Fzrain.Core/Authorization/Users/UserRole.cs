using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace Fzrain.Authorization.Users
{
    public class UserRole : CreationAuditedEntity<long>
    {     
        public virtual long UserId { get; set; }      
        public virtual int RoleId { get; set; }     
    }
}