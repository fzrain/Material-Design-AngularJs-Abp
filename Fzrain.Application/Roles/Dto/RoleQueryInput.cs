using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace Fzrain.Roles.Dto
{
   public  class RoleQueryInput: IPagedResultRequest
    {
        [Range(0, 1000)]
        public int MaxResultCount { get; set; }
       public int SkipCount { get; set; }
    }
}
