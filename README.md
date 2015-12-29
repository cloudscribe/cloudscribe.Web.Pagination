# cloudscribe.Web.Pagination
ASP.NET 5/MVC 6 TagHelper for Pagination

[![Join the chat at https://gitter.im/joeaudette/cloudscribe](https://badges.gitter.im/joeaudette/cloudscribe.svg)](https://gitter.im/joeaudette/cloudscribe?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

This was implemented in support of a larger project [cloudscribe.Core.Web](https://github.com/joeaudette/cloudscribe/) but has been moved to a separate repository since it has no dependencies on other "cloudscribe" components and should be useful in any ASP.NET 5/MVC 6 project that needs pagination.

Much/most of the credit for this project should go to the [MvcPaging](https://github.com/martijnboland/MvcPaging) project. Some of the ideas, logic, tests, and demo content are borrowed from that project.

In addition to the PagerTagHelper, this project also has an AlphaPagerTagHelper for Alphabetic pagination that can be used to filter the paged content in conjunction with the numeric pager.

You can download/clone this repo and run the PagingDemo.Web project to see multiple demo pages using the pager in various configurations including ajax and ajax inside a bootstrap modal.

## Installation

Prerequisites:

*  [Visual Studio 2015](https://www.visualstudio.com/en-us/downloads) 
*  [ASP.NET 5 RC1 Tooling](https://get.asp.net/) 

To install from nuget.org open the project.json file of your web application and in the dependencies section add:

    "cloudscribe.Web.Pagination": "1.0.0-*"
    
Visual Studio 2015 should restore the package automatically, you could also open a command prompt and use dnu restore in your project folder.

In your Startup.cs you will need this at the top: 

    using cloudscribe.Web.Pagination;

and in ConfigureServices you will need this:

    services.TryAddTransient<IBuildPaginationLinks, PaginationLinkBuilder>();
    
Note that [PaginationLinkBuilder](https://github.com/joeaudette/cloudscribe.Web.Pagination/blob/master/src/cloudscribe.Web.Pagination/src/cloudscribe.Web.Pagination/PaginationLinkBuilder.cs) is where the logic/strategy for condensing the pagination links exists. For example if there are 500 pages, obviously you do not want 500 links, so some links have to be left out while still making it possible to navigate to any page. The logic there came from the MvcPaging project and works pretty well but if you wanted to use a different strategy you could implement and inject your own IBuildPaginationLinks.

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

#### PagerTagHelper Supported Attributes with their default values

* cs-paging-info - null - you can pass in an instance of PaginationSettings rather than pass in separate properties for cs-paging-pagesize, cs-paging-pagenumber, cs-paging-totalitems, and cs-paging-maxpageritems
* cs-paging-pagesize - 10
* cs-paging-pagenumber - 1
* cs-paging-totalitems - 1
* cs-paging-maxpageritems - 10
* cs-pagenumber-param - pageNumber
* cs-show-first-last - false
* cs-first-page-text - <
* cs-first-page-title - First Page
* cs-last-page-text - >
* cs-last-page-title - Last Page
* cs-previous-page-text - «
* cs-previous-page-title - Previous page
* cs-next-page-text - »
* cs-next-page-title - Next page
* cs-pager-ul-class - pagination
* cs-pager-li-current-class - active
* cs-pager-li-non-active-class - disabled
* cs-ajax-target - empty - should be the id of the html element to target for ajax updates
* cs-ajax-mode - replace

To use ajax, you must include jquery.unobtrusive-ajax.js in the page

To use the AlphaPagerTagHelper you would add smething like this in your view:

    <cs-alphapager cs-alphabet="ABCDEFGHIJKLMNOPQRSTUVWXYZ"
        cs-selected-letter="@Model.Query"
        cs-all-label="All"
        asp-controller="Paging"
        asp-action="ProductList" 
        cs-selected-letter-param="query"
        ></cs-alphapager>

#### AlphaPagerTagHelper Supported Attributes with their default values

* cs-alphabet - ABCDEFGHIJKLMNOPQRSTUVWXYZ
* cs-populated-letters - null - you can pass in an IEnumerable<string> indicating which letters actually have data so that ones without data can be non links. If not provided then every letter will be a link
* cs-selected-letter - empty string
* cs-selected-letter-param = "letter"
* cs-all-label  - "All"
* cs-all-value - empty string
* cs-include-numbers - false
* cs-alphapager-ul-class - "pagination alpha"

For more details you can study the PagingDemo.Web project in this repo. If you have questions or find bugs, please [post in the issues](https://github.com/joeaudette/cloudscribe.Web.Pagination/issues)
