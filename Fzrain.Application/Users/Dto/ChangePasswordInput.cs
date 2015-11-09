using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace Fzrain.Users.Dto
{
    public class ChangePasswordInput : IInputDto
    {
      
        public string OldPassword { get; set; }      
        public string NewPassword { get; set; }     
        public string ConfirmPassword { get; set; }
    }
}
