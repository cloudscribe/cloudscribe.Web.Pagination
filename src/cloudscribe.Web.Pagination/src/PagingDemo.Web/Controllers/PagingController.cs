

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using PagingDemo.Web.Models;
using cloudscribe.Web.Pagination;


namespace PagingDemo.Web.Controllers
{
    public class PagingController : Controller
    {
        private const int DefaultPageSize = 10;
        private List<Product> allProducts = new List<Product>();
        private readonly string[] allCategories = new string[3] { "Shoes", "Electronics", "Food" };

        public PagingController()
        {
            InitializeProducts();
        }

        private void InitializeProducts()
        {
            // Create a list of products. 50% of them are in the Shoes category, 25% in the Electronics 
            // category and 25% in the Food category.
            for (var i = 0; i < 527; i++)
            {
                var product = new Product();
                product.Name = "Product " + (i + 1);
                var categoryIndex = i % 4;
                if (categoryIndex > 2)
                {
                    categoryIndex = categoryIndex - 3;
                }
                product.Category = allCategories[categoryIndex];
                allProducts.Add(product);
            }
        }


        public IActionResult Index(int? page)
        {
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            return View(this.allProducts.ToPagedList(currentPageIndex, DefaultPageSize));

            //return View();
        }

    }
}
