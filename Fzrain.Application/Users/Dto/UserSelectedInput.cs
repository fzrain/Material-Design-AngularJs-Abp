using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace Fzrain.Users.Dto
{
    public class UserSelectedInput : IPagedResultRequest
    {
        public int MaxResultCount { get; set; }
        public int SkipCount { get; set; }
    }
}
