using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;

namespace Fzrain.Users
{
   public  class User:Entity<Guid>
   {
       public string  UserName { get; set; }
       public string  Password { get; set; }
    }
}
