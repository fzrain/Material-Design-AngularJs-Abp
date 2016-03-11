using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.AutoMapper;

namespace Fzrain.AuditLogs.Dto
{
    [AutoMapFrom(typeof(AuditLog))]
   public  class AuditLogDto: EntityDto<long>
    {      
        public virtual string ServiceName { get; set; }    
        public virtual string MethodName { get; set; }    
        public virtual string ClientIpAddress { get; set; }      
        public virtual string ClientName { get; set; }       
        public virtual string BrowserInfo { get; set; }
    }
}
