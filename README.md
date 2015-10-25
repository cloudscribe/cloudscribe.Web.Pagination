# cloudscribe.Web.Pagination
ASP.NET 5/MVC 6 TagHelper for Pagination

This was implemented in support of a larger project [cloudscribe.Core.Web](https://github.com/joeaudette/cloudscribe/) but has been moved to a separate repository since it has no dependencies on other "cloudscribe" components and should be useful in any ASP.NET 5/MVC 6 project that needs pagination.

Much of the credit for this project should go to [Martijn Boland](https://github.com/martijnboland), I borrowed ideas, logic, tests, and demo content from his [MVCPaging](https://github.com/martijnboland/MvcPaging) project.

In addition to the PagerTagHelper, this project also has an HtmlHelper for Alphabetic pagination that can be used to filter the paged content in conjuntion with the numeric pager.

You can download/clone this repo and run the PagingDemo.Web project to see multiple demo pages using the pager in various configurations including ajax and ajax inside a bootstrap modal.

## Installation

To install from nuget.org open the project.json file of your web application and in the dependencies section add:

    "cloudscribe.Web.Pagination": "1.0.0-*"
    
In your _ViewImports.cshtml file add:

    @addTagHelper "*, cloudscribe.Web.Pagination"

In a view where you need paging you would add something like this:

     <cs-pager cs-paging-pagesize="@Model.PageSize"
              cs-paging-pagenumber="@Model.PageNumber"
              cs-paging-totalitems="@Model.TotalItemCount"
              cs-pagenumber-param="page"
              asp-controller="Paging"
              asp-action="Index"></cs-pager>

If you need to preserve other route parameters in the pager link urls you can add them the same way you do with the AnchorTagHelper using asp-route-yourparamname="@Model.YourProperty" etc.

You can also add a PaginationSettings property on your ViewModel like this:

    public PaginationSettings Paging { get; set; }

and then set it from your controller and pass it in to the taghelper like this where Model.Paging is the PaginationSettings object:

    <cs-pager cs-paging-info="@Model.Paging" 
              cs-pagenumber-param="pageNumber"
              asp-controller="Paging"
              asp-action="ProductList" 
              asp-route-query="@Model.Query"
              asp-route-pagesize="@Model.Paging.ItemsPerPage"></cs-pager>
  
The above example passes in a PaginationSettings object which encapsulates the pagesize, pagenumber, totalitems etc. It also passes in 2 extra route parameters "query" and "pagesize".

For more details you can study the PagingDemo.Web project in this repo. If you have questions or find bugs, please [post in the issues](https://github.com/joeaudette/cloudscribe.Web.Pagination/issues)
