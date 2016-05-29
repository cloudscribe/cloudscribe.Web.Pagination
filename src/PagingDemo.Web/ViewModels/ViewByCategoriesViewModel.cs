
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cloudscribe.Web.Pagination;
using PagingDemo.Web.Models;

namespace PagingDemo.Web.ViewModels
{
    public class ViewByCategoriesViewModel
    {
        public ViewByCategoriesViewModel()
        {
            Paging = new PaginationSettings();
            
        }
        public List<Product> Products { get; set; }
        public string[] AvailableCategories { get; set; }
        public string[] Categories { get; set; }

        public PaginationSettings Paging { get; set; }
    }
}
