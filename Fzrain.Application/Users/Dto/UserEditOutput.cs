using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Fzrain.Users.Dto
{
    [AutoMapTo(typeof(User))]
    public class UserEditOutput : EntityResultOutput<long>
    {
        public string Name { get; set; }
        public string MobilePhone { get; set; }       
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public bool CanChangeUserName { get; set; }
        public bool IsActive { get; set; }
        public  List<dynamic> RoleInfos { get; set; }
    }
}
