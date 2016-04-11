using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Fzrain.AuditLogs.Dto;

namespace Fzrain.AuditLogs
{
   public  interface IAuditLogAppService : IApplicationService
    {
       PagedResultOutput<AuditLogDto> GetAuditLogs(GetAuditLogInput input);
       Task<AuditLog> GetDetail(IdInput<long> input);
    }
}
