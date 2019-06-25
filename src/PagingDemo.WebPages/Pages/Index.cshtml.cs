using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PagingDemo.WebPages.Models;

namespace PagingDemo.WebPages.Pages
{
    public class IndexModel : PageModel
    {
        private static IEnumerable<SampleData> Data = Enumerable.Range(1, 149)
            .Select(x => new SampleData { Id = x, Title = $"good Name ${x}" });

        public PagedResult<SampleData> List { get; set; }
        public void OnGet(int p = 1)
        {
            // do not use name "page" , your RazorPage take the `Page="/Index"` already
            var index = ToIndex(p);
            int pageSize = 8;
            List = new PagedResult<SampleData>
            {
                Data = Data.Skip(index * pageSize)
                .Take(pageSize).ToList(),
                PageSize = pageSize,
                PageNumber = p,
                TotalItems = Data.Count()
            };
        }

        private int ToIndex(int number)
        {
            var index = number - 1;
            return index < 0 ? 0 : index;
        }
    }
}
