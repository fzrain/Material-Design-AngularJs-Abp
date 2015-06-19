using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Fzrain.Users.Dto;

namespace Fzrain.Users
{
   public  class UserAppService :ApplicationService, IUserAppService
    {
        private readonly IRepository<User> userRepository;

        public UserAppService(IRepository<User> userRepository)
        {
            this.userRepository = userRepository;
            
        }

        public ListResultOutput<UserDto> GetAllUsers()
        {
            return new ListResultOutput<UserDto>
            {
                Items =userRepository.GetAllList().MapTo<List<UserDto>>()
            };
        }
    }
}
