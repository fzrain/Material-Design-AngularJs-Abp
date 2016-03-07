using System;
using Abp.Auditing;
using Abp.Domain.Entities;
using Abp.Extensions;

namespace Fzrain.Auditing
{
  
    public class AuditLog : Entity<long>, IMayHaveTenant
    {
           
        public  int? TenantId { get; set; }

        public  long? UserId { get; set; }

     
        public  string ServiceName { get; set; }

     
        public  string MethodName { get; set; }
        
        public  string Parameters { get; set; }

      
        public  DateTime ExecutionTime { get; set; }

        public  int ExecutionDuration { get; set; }

     
        public  string ClientIpAddress { get; set; }

        public  string ClientName { get; set; }

        public  string BrowserInfo { get; set; }   
      
        public  string Exception { get; set; }
        public static AuditLog CreateFromAuditInfo(AuditInfo auditInfo)
        {
            var exceptionMessage = auditInfo.Exception?.ToString();
            return new AuditLog
                   {
                       TenantId = auditInfo.TenantId,
                       UserId = auditInfo.UserId,
                       ServiceName = auditInfo.ServiceName.TruncateWithPostfix(256),
                       MethodName = auditInfo.MethodName.TruncateWithPostfix(256),
                       Parameters = auditInfo.Parameters.TruncateWithPostfix(1024),
                       ExecutionTime = auditInfo.ExecutionTime,
                       ExecutionDuration = auditInfo.ExecutionDuration,
                       ClientIpAddress = auditInfo.ClientIpAddress.TruncateWithPostfix(64),
                       ClientName = auditInfo.ClientName.TruncateWithPostfix(128),
                       BrowserInfo = auditInfo.BrowserInfo.TruncateWithPostfix(256),
                       Exception = exceptionMessage.TruncateWithPostfix(2000)
                   };
        }

        public override string ToString()
        {
            return
                $"AUDIT LOG: {ServiceName}.{MethodName} is executed by user {UserId} in {ExecutionDuration} ms from {ClientIpAddress} IP address.";
        }
    }
}
