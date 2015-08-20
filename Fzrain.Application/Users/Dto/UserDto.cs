using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Fzrain.Authorization.Users;

namespace Fzrain.Users.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserDto : EntityDto<long>
    {       
        public virtual string Name { get; set; }       
        public virtual string Surname { get; set; }    
        public virtual string UserName { get; set; }
        public virtual string EmailAddress { get; set; }
        public virtual bool IsEmailConfirmed { get; set; }    
        public virtual DateTime? LastLoginTime { get; set; }    
        public virtual bool IsActive { get; set; }
    }
}