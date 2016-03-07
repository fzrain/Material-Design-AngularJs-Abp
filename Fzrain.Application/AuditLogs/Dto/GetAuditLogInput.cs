using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Fzrain.Auditing;
using Fzrain.Common;

namespace Fzrain.AuditLogs.Dto
{
   public  class GetAuditLogInput : BaseInputDto<AuditLogDto>
   {
     
   }
}
