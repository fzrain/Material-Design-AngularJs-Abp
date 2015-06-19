using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Fzrain.Users.Dto;

namespace Fzrain.Users
{
   public  interface IUserAppService:IApplicationService
    {
        ListResultOutput<UserDto> GetAllUsers();
    }
}
