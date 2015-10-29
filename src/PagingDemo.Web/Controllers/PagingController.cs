

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using PagingDemo.Web.Models;
using PagingDemo.Web.ViewModels;
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

            
        }

        public IActionResult ProductList(
            int? pageNumber, 
            int? pageSize,
            string query = "")
        {
            int currentPageIndex = pageNumber.HasValue ? pageNumber.Value - 1 : 0;
            int itemsPerPage = pageSize.HasValue ? pageSize.Value : DefaultPageSize;

            var model = new ProductListViewModel();

            model.Products = this.allProducts.Where(p => 
            p.Category.StartsWith(query)
            ).ToPagedList(currentPageIndex, itemsPerPage);

            model.Paging.CurrentPage = pageNumber.HasValue ? pageNumber.Value : 1;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalItems = model.Products.TotalItemCount;
            model.Query = query; //TODO: sanitize

            return View(model);


        }

        public IActionResult ViewByCategory(string categoryName, int? page)
        {
            categoryName = categoryName ?? this.allCategories[0];
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;

            var productsByCategory = this.allProducts.Where(p => p.Category.Equals(categoryName)).ToPagedList(currentPageIndex,
                                                                                                              DefaultPageSize);
            ViewBag.CategoryName = new SelectList(this.allCategories, categoryName);
            ViewBag.CategoryDisplayName = categoryName;
            return View("ProductsByCategory", productsByCategory);
        }

        

        public IActionResult ViewByCategories(string[] categories, int? page)
        {
            // I have not figured out how to convert a string array to routeparam
            // in mvc6 yet so the categories may be passed as a single csv string

            if(categories != null)
            {
                if((categories.Length == 1) &&(categories[0].Contains(",")))
                {
                    categories = categories[0].Split(',');
                }
            }

            var model = new ViewByCategoriesViewModel();
            model.Categories = categories ?? new string[0];
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;

            model.Products = this.allProducts.Where(p => model.Categories.Contains(p.Category)).ToPagedList(currentPageIndex, DefaultPageSize);
            model.AvailableCategories = this.allCategories;

            model.Paging.CurrentPage = page.HasValue ? page.Value : 1;
            model.Paging.ItemsPerPage = DefaultPageSize;
            model.Paging.TotalItems = model.Products.TotalItemCount;
            model.Paging.ShowFirstLast = true;

            return View("ProductsByCategories", model);
        }

        public IActionResult IndexAjax()
        {
            int currentPageIndex = 0;
            var products = this.allProducts.ToPagedList(currentPageIndex, DefaultPageSize);
            if(HttpContext.Request.IsAjaxRequest())
            {
                return PartialView("_PagingModal", products);
            }
            return View(products);
        }

        public IActionResult AjaxPage(int? page)
        {
            ViewBag.Title = "Browse all products";
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            var products = this.allProducts.ToPagedList(currentPageIndex, DefaultPageSize);
            return PartialView("_ProductGrid", products);
        }


    }
}
