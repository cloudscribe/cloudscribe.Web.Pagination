using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cloudscribe.Web.Pagination;
using PagingDemo.Web.Models;

namespace PagingDemo.Web.ViewModels
{
    public class ProductListViewModel
    {
        public ProductListViewModel()
        {
            Paging = new PaginationSettings();
        }

        public string Query { get; set; } = string.Empty;

        public IPagedList<Product> Products { get; set; } = null;

        public PaginationSettings Paging { get; set; }
    }
}
