using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Fzrain.Auditing;

namespace Fzrain.AuditLogs.Dto
{
    [AutoMapFrom(typeof(AuditLog))]
   public  class AuditLogDto: EntityDto<long>
    {  
        public virtual int? TenantId { get; set; }
        public virtual long? UserId { get; set; }       
        public virtual string ServiceName { get; set; }    
        public virtual string MethodName { get; set; } 
        public virtual string Parameters { get; set; } 
        public virtual DateTime ExecutionTime { get; set; }   
        public virtual int ExecutionDuration { get; set; }    
        public virtual string ClientIpAddress { get; set; }      
        public virtual string ClientName { get; set; }       
        public virtual string BrowserInfo { get; set; }
        public virtual string Exception { get; set; }
    }
}
