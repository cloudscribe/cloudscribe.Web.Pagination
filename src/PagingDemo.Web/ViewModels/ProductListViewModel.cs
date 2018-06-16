using cloudscribe.Pagination.Models;
using PagingDemo.Web.Models;

namespace PagingDemo.Web.ViewModels
{
    public class ProductListViewModel
    {
        public ProductListViewModel()
        {
            Products = new PagedResult<Product>();
        }

        public string Query { get; set; } = string.Empty;

        public PagedResult<Product> Products { get; set; } = null;

        
    }
}
