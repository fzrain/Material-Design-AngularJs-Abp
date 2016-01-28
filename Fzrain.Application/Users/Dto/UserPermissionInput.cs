using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace Fzrain.Users.Dto
{
    public  class UserPermissionInput:IdInput<long>
    {
        public List<string> Permissions { get; set; }
    }
}
