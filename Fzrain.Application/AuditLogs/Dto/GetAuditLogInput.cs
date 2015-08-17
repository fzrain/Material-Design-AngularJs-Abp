using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace Fzrain.AuditLogs.Dto
{
   public  class GetAuditLogInput : IInputDto, IPagedResultRequest
   {
       [Range(0, 1000)]
       public int MaxResultCount { get; set; } = 10;
        public int SkipCount { get; set; }
    }
}
