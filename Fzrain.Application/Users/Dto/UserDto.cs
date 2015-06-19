using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Fzrain.Users.Dto
{
    [AutoMapFrom(typeof(User))]
    public  class UserDto:IDto
    {
        public string  UserName { get; set; }
    }
}
