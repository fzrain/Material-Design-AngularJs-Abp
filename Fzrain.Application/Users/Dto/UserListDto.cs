using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Fzrain.Users.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserListDto : EntityDto<long>
    {       
        public  string Name { get; set; }       
        public  string MobilePhone { get; set; }    
        public  string UserName { get; set; }
        public  string EmailAddress { get; set; }
        public  bool IsEmailConfirmed { get; set; }    
        public  DateTime? LastLoginTime { get; set; }    
        public  bool IsActive { get; set; }
      //  public  dynamic Roles { get; set; }
    }
}