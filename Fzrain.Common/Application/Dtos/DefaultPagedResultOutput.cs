using Abp.Application.Services.Dto;

namespace Fzrain.Common.Application.Dtos
{
    public  class DefaultPagedResultOutput<T>: PagedResultOutput<T>
    {
        public int PageIndex { get; set; }
    }
}
