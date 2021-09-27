using System;
using System.Collections.Generic;

namespace Britannica.Application.Bases
{
    public class PaginatedResponse<T> : List<T>
    {
        public PaginatedResponse(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public int PageIndex { get; }
        public int TotalPages { get; }


        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;
    }
}
