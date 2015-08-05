using System.Collections.Generic;
using System.Linq;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Fzrain.Common.Application.Services;
using Fzrain.Users.Dto;

namespace Fzrain.Users
{
    public class UserAppService : CrudAppService<User,UserDto,long>, IUserAppService
    {
        private UserManager userManager;
        public UserAppService(IRepository<User, long> userRepository, UserManager userManager)
            : base(userRepository)
        {
            this.userManager = userManager;
        }
    }
}