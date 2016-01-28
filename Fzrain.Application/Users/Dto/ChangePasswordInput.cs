using Abp.Application.Services.Dto;

namespace Fzrain.Users.Dto
{
    public class ChangePasswordInput : IInputDto
    {
      
        public string OldPassword { get; set; }      
        public string NewPassword { get; set; }     
    }
}
