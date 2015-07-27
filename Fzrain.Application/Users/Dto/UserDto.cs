using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Fzrain.Users.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserDto : EntityRequestInput<long>
    {
        public string UserName { get; set; }

        public string Name { get; set; }
        
        public string Surname { get; set; }
        
        public string EmailAddress { get; set; }
    }
}