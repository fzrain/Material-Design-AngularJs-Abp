using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Fzrain.Users
{
   public  class User:FullAuditedEntity
   {
       public string  UserName { get; set; }
       public string  Password { get; set; }
     
   }
}
