using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Fzrain.Authorization.Users;

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
