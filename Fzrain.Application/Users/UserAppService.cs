using System.Collections.Generic;
using System.Linq;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Fzrain.Users.Dto;

namespace Fzrain.Users
{
    public class UserAppService : ApplicationService, IUserAppService
    {
        private readonly UserManager _userManager;

        public UserAppService(UserManager userManager)
        {
            
            _userManager = userManager;
        }

        public PagedResultOutput<UserDto> GetUsers()
        {
            return new PagedResultOutput<UserDto>
                   {
                       Items = _userManager.Users.ToList().MapTo<List<UserDto>>()
                   };
        }
    }
}