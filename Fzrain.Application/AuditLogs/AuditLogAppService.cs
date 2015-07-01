using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Fzrain.AuditLogs.Dto;

namespace Fzrain.AuditLogs
{
    public class AuditLogAppService : ApplicationService, IAuditLogAppService
    {
        private readonly IRepository<AuditLog,long> auditRepository;

        public AuditLogAppService(IRepository<AuditLog, long> auditRepository)
        {
            this.auditRepository = auditRepository;
        }

        public PagedResultOutput<AuditLogDto> GetAuditLogs()
        {
            return new PagedResultOutput<AuditLogDto>
            {
                Items =auditRepository.GetAllList().MapTo<List<AuditLogDto>>(),
                TotalCount = auditRepository.GetAll().Count()
            };
        }
    }
}
