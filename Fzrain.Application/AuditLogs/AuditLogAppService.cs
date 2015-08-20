using System.Collections.Generic;
using System.Linq;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Fzrain.Auditing;
using Fzrain.AuditLogs.Dto;
using Fzrain.Common.Application.Dtos;

namespace Fzrain.AuditLogs
{
    public class AuditLogAppService : ApplicationService, IAuditLogAppService
    {
        private readonly IRepository<AuditLog,long> auditRepository;

        public AuditLogAppService(IRepository<AuditLog, long> auditRepository)
        {
            this.auditRepository = auditRepository;
        }

        public PagedResultOutput<AuditLogDto> GetAuditLogs(GetAuditLogInput input)
        {
            var auditLogCount = auditRepository.Count();
            var auditLogs = auditRepository.GetAll().OrderByDescending(a=>a.ExecutionTime).PageBy(input).ToList();
            return new PagedResultOutput<AuditLogDto>
            {
                Items = auditLogs.MapTo<List<AuditLogDto>>(),
                TotalCount = auditLogCount,
            };
        }

        public AuditLogDto GetDetail(IdInput<long> input)
        {
            return auditRepository.Get(input.Id).MapTo<AuditLogDto>();
        }
    }
}
