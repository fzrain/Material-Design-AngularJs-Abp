using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace Fzrain.Common
{
    public  class BaseInputDto<T>: IInputDto, IPagedResultRequest
    {
        [Range(0, 1000)]
        public int MaxResultCount { get; set; } = 10;
        public int SkipCount { get; set; }
        public T Filter { get; set; }      
    }
}
