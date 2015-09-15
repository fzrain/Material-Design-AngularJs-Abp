using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace Fzrain.Users.Dto
{
    public  class UserPermissionInput:IdInput<long>
    {
        public List<string> Permissions { get; set; }
    }
}
