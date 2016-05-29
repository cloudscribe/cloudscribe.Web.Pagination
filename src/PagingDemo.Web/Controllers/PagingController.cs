

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PagingDemo.Web.Models;
using PagingDemo.Web.ViewModels;



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
            
            var currentPageNum = page.HasValue ? page.Value : 1;
            var offset = (DefaultPageSize * currentPageNum) - DefaultPageSize;
            var model = new ProductListViewModel();
            model.Products = this.allProducts
                .Skip(offset)
                .Take(DefaultPageSize)
                .ToList();

            model.Paging.CurrentPage = currentPageNum;
            model.Paging.ItemsPerPage = DefaultPageSize;
            model.Paging.TotalItems = allProducts.Count;

            
            return View(model);

            
        }

        public IActionResult ProductList(
            int? pageNumber, 
            int? pageSize,
            string query = "")
        {
            var itemsPerPage = pageSize.HasValue ? pageSize.Value : DefaultPageSize;
            var currentPageNum = pageNumber.HasValue ? pageNumber.Value  : 1;
            var offset = (itemsPerPage * currentPageNum) - itemsPerPage;
            var model = new ProductListViewModel();

            var filtered = this.allProducts.Where(p =>
                p.Category.StartsWith(query)
                );

            model.Products = filtered
                .Skip(offset)
                .Take(itemsPerPage)
                .ToList();

            model.Paging.CurrentPage = currentPageNum;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalItems = filtered.ToList().Count;
            model.Query = query; //TODO: sanitize

            return View(model);


        }

        public IActionResult BlogPagingDemo(
            int? pageNumber,
            string query = "")
        { 
            int itemsPerPage = 1;

            var currentPageNum = pageNumber.HasValue ? pageNumber.Value : 1;
            var offset = (itemsPerPage * currentPageNum) - itemsPerPage;

            var filtered = allProducts.Where(p =>
                p.Category.StartsWith(query)
            )
            .OrderByDescending(p => p.CreatedUtc)
            .ToList();
            
            var model = new ProductListViewModel();

            model.Products = filtered
            .Skip(offset)
            .Take(itemsPerPage)
            .ToList();

            model.Paging.CurrentPage = currentPageNum;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalItems = filtered.Count;
            //model.Paging.UseReverseIncrement = true;
            model.Query = query; //TODO: sanitize

            return View(model);


        }

        public IActionResult ViewByCategory(string categoryName, int? page)
        {
            categoryName = categoryName ?? this.allCategories[0]; 
            var currentPageNum = page.HasValue ? page.Value : 1;
            var offset = (DefaultPageSize * currentPageNum) - DefaultPageSize;

            var filtered = this.allProducts.Where(p =>
                p.Category.Equals(categoryName)
                ).ToList();

            var model = new ProductListViewModel();

            model.Products = filtered
                .Skip(offset)
                .Take(DefaultPageSize)
                .ToList();

            model.Paging.CurrentPage = currentPageNum;
            model.Paging.ItemsPerPage = DefaultPageSize;
            model.Paging.TotalItems = filtered.Count;

            ViewBag.CategoryName = new SelectList(this.allCategories, categoryName);
            ViewBag.CategoryDisplayName = categoryName;

            return View("ProductsByCategory", model);
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

            var currentPageNum = page.HasValue ? page.Value : 1;
            var offset = (DefaultPageSize * currentPageNum) - DefaultPageSize;

            var model = new ViewByCategoriesViewModel();
            model.Categories = categories ?? new string[0];
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;

            var filtered = this.allProducts.Where(p =>
                model.Categories.Contains(p.Category)
                ).ToList();

            model.Products = filtered
                .Skip(offset)
                .Take(DefaultPageSize)
                .ToList();
                
            model.AvailableCategories = this.allCategories;

            model.Paging.CurrentPage = currentPageNum;
            model.Paging.ItemsPerPage = DefaultPageSize;
            model.Paging.TotalItems = filtered.Count;
            model.Paging.ShowFirstLast = true;

            return View("ProductsByCategories", model);
        }

        public IActionResult IndexAjax()
        {
            
            var model = new ProductListViewModel();

            model.Products = this.allProducts
                .Take(DefaultPageSize)
                .ToList();
            

            model.Paging.CurrentPage = 1;
            model.Paging.ItemsPerPage = DefaultPageSize;
            model.Paging.TotalItems = allProducts.Count;

            if (HttpContext.Request.IsAjaxRequest())
            {
                return PartialView("_PagingModal", model);
            }
            return View(model);
        }

        public IActionResult AjaxPage(int? page)
        {
            ViewBag.Title = "Browse all products";
            
            var currentPageNum = page.HasValue ? page.Value : 1;
            var offset = (DefaultPageSize * currentPageNum) - DefaultPageSize;

            var model = new ProductListViewModel();

            model.Products = this.allProducts
                .Skip(offset)
                .Take(DefaultPageSize)
                .ToList();

            model.Paging.CurrentPage = currentPageNum;
            model.Paging.ItemsPerPage = DefaultPageSize;
            model.Paging.TotalItems = allProducts.Count;
            
            return PartialView("_ProductGrid", model);
        }


    }
}
