// borrowed from
// https://github.com/martijnboland/MvcPaging/blob/master/src/MvcPaging/IPagedList.cs
// under MIT license

using System.Collections.Generic;

namespace cloudscribe.Pagination.Models
{
    public interface IPagedList
    {
        int PageCount { get; }
        int TotalItemCount { get; }
        int PageIndex { get; }
        int PageNumber { get; }
        int PageSize { get; }
        bool HasPreviousPage { get; }
        bool HasNextPage { get; }
        bool IsFirstPage { get; }
        bool IsLastPage { get; }
        int ItemStart { get; }
        int ItemEnd { get; }
    }

    public interface IPagedList<out T> : IPagedList, IEnumerable<T>
    {
    }
}
