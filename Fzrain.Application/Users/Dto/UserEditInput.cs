using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Fzrain.Authorization.Users;

namespace Fzrain.Users.Dto
{
    [AutoMapTo(typeof(User))]
    public class UserEditInput : NullableIdInput<long>
    {
        public string Name { get; set; }
        public string MobilePhone { get; set; }
        public string  Password { get; set; }
        public string PasswordRepeat { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public bool IsActive { get; set; }
        public bool SetDefaultPassword { get; set; }
        public  string[] Roles { get; set; }
    }
}
