// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-07-02
// Last Modified:			2018-06-16
// 

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace cloudscribe.Web.Pagination
{
    [HtmlTargetElement("cs-pager", Attributes = PagingInfoAttributeName)]
    [HtmlTargetElement("cs-pager", Attributes = "cs-paging-pagenumber,cs-paging-totalitems")]
    public class PagerTagHelper : TagHelper
    {
        private const string PagingInfoAttributeName = "cs-paging-info";
        private const string PageSizeAttributeName = "cs-paging-pagesize";
        private const string PageNumberAttributeName = "cs-paging-pagenumber";
        private const string TotalItemsAttributeName = "cs-paging-totalitems";
        private const string MaxPagerItemsAttributeName = "cs-paging-maxpageritems";
        private const string AjaxTargetAttributeName = "cs-ajax-target";
        private const string AjaxModeAttributeName = "cs-ajax-mode";
        private const string AjaxSuccessAttributeName = "cs-ajax-success";
        private const string AjaxFailureAttributeName = "cs-ajax-failure";
        private const string AjaxBeginAttributeName = "cs-ajax-begin";
        private const string AjaxCompleteAttributeName = "cs-ajax-complete";
        private const string AjaxLoadingAttributeName = "cs-ajax-loading";
        private const string AjaxLoadingDurationAttributeName = "cs-ajax-loading-duration";
        private const string PageNumberParamAttributeName = "cs-pagenumber-param";

        private const string ActionAttributeName = "asp-action";
        private const string ControllerAttributeName = "asp-controller";
        private const string FragmentAttributeName = "asp-fragment";
        private const string HostAttributeName = "asp-host";
        private const string ProtocolAttributeName = "asp-protocol";
        private const string RouteAttributeName = "asp-route";
        private const string RouteValuesDictionaryName = "asp-all-route-data";
        private const string RouteValuesPrefix = "asp-route-";
        private const string BaseHrefAttributeName = "asp-basehref";

        public PagerTagHelper(
            IUrlHelperFactory urlHelperFactory,
            IBuildPaginationLinks linkBuilder = null)
        {
            this.linkBuilder = linkBuilder ?? new PaginationLinkBuilder();
            this.urlHelperFactory = urlHelperFactory;
        }

        private IUrlHelperFactory urlHelperFactory;

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        private IBuildPaginationLinks linkBuilder;

        [HtmlAttributeName(PagingInfoAttributeName)]
        public PaginationSettings PagingModel { get; set; } = null;

        [HtmlAttributeName(PageSizeAttributeName)]
        public int PageSize { get; set; } = 10;

        [HtmlAttributeName(PageNumberAttributeName)]
        public long PageNumber { get; set; } = 1;

        [HtmlAttributeName(TotalItemsAttributeName)]
        public long TotalItems { get; set; } = 1;

        [HtmlAttributeName(MaxPagerItemsAttributeName)]
        public long MaxPagerItems { get; set; } = 10;

        [HtmlAttributeName(AjaxTargetAttributeName)]
        public string AjaxTarget { get; set; } = string.Empty;

        [HtmlAttributeName(AjaxModeAttributeName)]
        public string AjaxMode { get; set; } = "replace";

        [HtmlAttributeName(AjaxSuccessAttributeName)]
        public string AjaxSuccess { get; set; } = string.Empty;

        [HtmlAttributeName(AjaxFailureAttributeName)]
        public string AjaxFailure { get; set; } = string.Empty;

        [HtmlAttributeName(AjaxBeginAttributeName)]
        public string AjaxBegin { get; set; } = string.Empty;

        [HtmlAttributeName(AjaxCompleteAttributeName)]
        public string AjaxComplete { get; set; } = string.Empty;

        [HtmlAttributeName(AjaxLoadingAttributeName)]
        public string AjaxLoading { get; set; } = string.Empty;

        [HtmlAttributeName(AjaxLoadingDurationAttributeName)]
        public string AjaxLoadingDuration { get; set; } = string.Empty;

        [HtmlAttributeName(PageNumberParamAttributeName)]
        public string PageNumberParam { get; set; } = "pageNumber";

        [HtmlAttributeName("cs-show-first-last")]
        public bool ShowFirstLast { get; set; } = false;

        [HtmlAttributeName("cs-show-numbered")]
        public bool ShowNumbered { get; set; } = true;

        [HtmlAttributeName("cs-use-reverse-increment")]
        public bool UseReverseIncrement { get; set; } = false;

        [HtmlAttributeName("cs-suppress-empty-nextprev")]
        public bool SuppressEmptyNextPrev { get; set; } = false;

        [HtmlAttributeName("cs-remove-nextprev-links")]
        public bool RemoveNextPrevLinks { get; set; } = false;

        [HtmlAttributeName("cs-suppress-inactive-firstlast")]
        public bool SuppressInActiveFirstLast { get; set; } = false;

        [HtmlAttributeName("cs-suppress-empty-pager")]
        public bool SuppressEmptyPager { get; set; } = true;
        
        [HtmlAttributeName("cs-first-page-text")]
        public string FirstPageText { get; set; } = "<";

        [HtmlAttributeName("cs-first-page-title")]
        public string FirstPageTitle { get; set; } = "First page";

        [HtmlAttributeName("cs-last-page-text")]
        public string LastPageText { get; set; } = ">";

        [HtmlAttributeName("cs-last-page-title")]
        public string LastPageTitle { get; set; } = "Last page";

        [HtmlAttributeName("cs-previous-page-text")]
        public string PreviousPageText { get; set; } = "«";

        [HtmlAttributeName("cs-previous-page-html")]
        public string PreviousPageHtml { get; set; } = "";

        [HtmlAttributeName("cs-previous-page-title")]
        public string PreviousPageTitle { get; set; } = "Previous page";

        [HtmlAttributeName("cs-next-page-text")]
        public string NextPageText { get; set; } = "»";

        [HtmlAttributeName("cs-next-page-html")]
        public string NextPageHtml { get; set; } = "";

        [HtmlAttributeName("cs-next-page-title")]
        public string NextPageTitle { get; set; } = "Next page";

        [HtmlAttributeName("cs-pager-ul-class")]
        public string UlCssClass { get; set; } = "pagination";

        [HtmlAttributeName("cs-pager-li-current-class")]
        public string LiCurrentCssClass { get; set; } = "active";

        [HtmlAttributeName("cs-pager-li-other-class")]
        public string LiOtherCssClass { get; set; } = "";

        [HtmlAttributeName("cs-pager-li-non-active-class")]
        public string LiNonActiveCssClass { get; set; } = "disabled";

        [HtmlAttributeName("cs-pager-link-current-class")]
        public string LinkCurrentCssClass { get; set; } = "";

        [HtmlAttributeName("cs-pager-link-other-class")]
        public string LinkOtherCssClass { get; set; } = "";

        /// <summary>
        /// The name of the action method.
        /// </summary>
        /// <remarks>Must be <c>null</c> if <see cref="Route"/> is non-<c>null</c>.</remarks>
        [HtmlAttributeName(ActionAttributeName)]
        public string Action { get; set; }

        /// <summary>
        /// The name of the controller.
        /// </summary>
        /// <remarks>Must be <c>null</c> if <see cref="Route"/> is non-<c>null</c>.</remarks>
        [HtmlAttributeName(ControllerAttributeName)]
        public string Controller { get; set; }

        /// <summary>
        /// The protocol for the URL, such as &quot;http&quot; or &quot;https&quot;.
        /// </summary>
        [HtmlAttributeName(ProtocolAttributeName)]
        public string Protocol { get; set; }

        /// <summary>
        /// The host name.
        /// </summary>
        [HtmlAttributeName(HostAttributeName)]
        public string Host { get; set; }

        /// <summary>
        /// The URL fragment name.
        /// </summary>
        [HtmlAttributeName(FragmentAttributeName)]
        public string Fragment { get; set; }

        /// <summary>
        /// Name of the route.
        /// </summary>
        /// <remarks>
        /// Must be <c>null</c> if <see cref="Action"/> or <see cref="Controller"/> is non-<c>null</c>.
        /// </remarks>
        [HtmlAttributeName(RouteAttributeName)]
        public string Route { get; set; }


        /// <summary>
        /// Additional parameters for the route.
        /// </summary>
        [HtmlAttributeName(RouteValuesDictionaryName, DictionaryAttributePrefix = RouteValuesPrefix)]
        public IDictionary<string, string> RouteValues
        { get; set; }
        = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// An alternative to Controller/Action or Route
        /// </summary>
        /// <remarks>
        /// <see cref="Route"/> and <see cref="Action"/> and <see cref="Controller"/> must be null to be effective
        /// </remarks>
        [HtmlAttributeName(BaseHrefAttributeName)]
        public string BaseHref { get; set; }

        //https://github.com/cloudscribe/cloudscribe.Web.Pagination/issues/47

        [HtmlAttributeName("cs-preserve-ambient-querystring")]
        public bool PreserveAmbientQueryString { get; set; } = true;

        private IUrlHelper urlHelper = null;
        
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
          
            if(PagingModel == null) 
            {
                // allow for passing in the settings separately
                PagingModel = new PaginationSettings();
                PagingModel.CurrentPage = PageNumber;
                PagingModel.ItemsPerPage = PageSize;
                PagingModel.TotalItems = TotalItems;
                PagingModel.MaxPagerItems = MaxPagerItems;
                PagingModel.SuppressEmptyNextPrev = SuppressEmptyNextPrev;
                PagingModel.SuppressInActiveFirstLast = SuppressInActiveFirstLast;
                PagingModel.RemoveNextPrevLinks = RemoveNextPrevLinks;
            }

            if(ShowFirstLast)
            {
                PagingModel.ShowFirstLast = true;
            }

            if(!ShowNumbered)
            {
                PagingModel.ShowNumbered = false;
            }

            if(UseReverseIncrement)
            {
                PagingModel.UseReverseIncrement = true;

                if(SuppressEmptyNextPrev)
                {
                    PagingModel.SuppressEmptyNextPrev = true;
                }
            }


            long totalPages = (long)Math.Ceiling(PagingModel.TotalItems / (double)PagingModel.ItemsPerPage);
            // don't render if only 1 page 
            if (SuppressEmptyPager && (totalPages <= 1))
            {
                output.SuppressOutput();
                return;
            }
            
            //change the cs-pager element into a ul
            output.TagName = "ul";
            
            
            //prepare things needed by generatpageeurl function

            urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
            
            List<PaginationLink> links = linkBuilder.BuildPaginationLinks(
                PagingModel,
                GeneratePageUrl,
                FirstPageText,
                FirstPageTitle,
                PreviousPageText,
                PreviousPageTitle,
                NextPageText,
                NextPageTitle,
                LastPageText,
                LastPageTitle,
                "...");

            foreach(PaginationLink link in links)
            {
                var li = new TagBuilder("li");

                if (link.IsCurrent)
                {
                    li.AddCssClass(LiCurrentCssClass);
                }
                else
                {
                    if (!link.Active)
                    {
                        li.AddCssClass(LiNonActiveCssClass);
                    }
                    else
                    {
                        if(!string.IsNullOrWhiteSpace(LiOtherCssClass))
                        {
                            li.AddCssClass(LiOtherCssClass);
                        }
                    }
                }

                if(link.Text == PreviousPageText && !string.IsNullOrWhiteSpace(PreviousPageHtml))
                {
                    if(string.IsNullOrEmpty(link.Url))
                    {
                        li.InnerHtml.AppendHtml(PreviousPageHtml);
                    }
                    else
                    {
                        li.InnerHtml.AppendHtml(PreviousPageHtml.Replace("#", link.Url));
                    }
                    
                }
                else if(link.Text == NextPageText && !string.IsNullOrWhiteSpace(NextPageHtml))
                {
                    if (string.IsNullOrEmpty(link.Url))
                    {
                        li.InnerHtml.AppendHtml(NextPageHtml);
                    }
                    else
                    {
                        li.InnerHtml.AppendHtml(NextPageHtml.Replace("#", link.Url));
                    }
                        
                }
                else
                {  
                    if(!link.IsCurrent && link.Active)
                    {
                        var a = new TagBuilder("a");

                        if(!string.IsNullOrWhiteSpace(LinkOtherCssClass))
                        {
                            a.AddCssClass(LinkOtherCssClass);
                        }

                        if (link.Active && (link.Url.Length > 0))
                        {
                            a.MergeAttribute("href", link.Url);
                        }
                        else
                        {
                            a.MergeAttribute("href", "#");
                        }


                        if (link.Text == "«")
                        {
                            a.InnerHtml.AppendHtml("&laquo;");
                        }
                        else if (link.Text == "»")
                        {
                            a.InnerHtml.AppendHtml("&raquo;");
                        }
                        else if (link.Text.Contains('<') && link.Text.Contains('>')) 
                        {
                            //if text is an html formatted icon and contains a <tag>
                            //ex. <span class='fa fa-chevron-right'></span>
                            a.InnerHtml.AppendHtml(link.Text);
                        }
                        else {
                            // if text should be html encoded
                            a.InnerHtml.Append(link.Text);
                        }

                        if (link.Title.Length > 0)
                        {
                            a.MergeAttribute("title", link.Title);
                        }

                        if (AjaxTarget.Length > 0)
                        {
                            a.MergeAttribute("data-ajax", "true");
                            a.MergeAttribute("data-ajax-mode", AjaxMode);
                            a.MergeAttribute("data-ajax-update", AjaxTarget);
                            if (AjaxSuccess.Length > 0)
                            {
                                a.MergeAttribute("data-ajax-success", AjaxSuccess);
                            }
                            if (AjaxFailure.Length > 0)
                            {
                                a.MergeAttribute("data-ajax-failure", AjaxFailure);
                            }
                            if (AjaxBegin.Length > 0)
                            {
                                a.MergeAttribute("data-ajax-begin", AjaxBegin);
                            }
                            if (AjaxComplete.Length > 0)
                            {
                                a.MergeAttribute("data-ajax-complete", AjaxComplete);
                            }
                            if (AjaxLoading.Length > 0)
                            {
                                a.MergeAttribute("data-ajax-loading", AjaxLoading);
                            }
                            if (AjaxLoadingDuration.Length > 0)
                            {
                                a.MergeAttribute("data-ajax-loading-duration", AjaxLoadingDuration);
                            }
                        }
                        li.InnerHtml.AppendHtml(a);
                    }
                    else
                    {
                        // current or not active
                        var span = new TagBuilder("span");

                        if (!string.IsNullOrWhiteSpace(LinkCurrentCssClass))
                        {
                            span.AddCssClass(LinkCurrentCssClass);
                        }

                        if (link.Text == "«")
                        {
                            span.InnerHtml.AppendHtml("&laquo;");

                        }
                        else if (link.Text == "»")
                        {
                            span.InnerHtml.AppendHtml("&raquo;");
                        }
                        else if (link.Text.Contains('<') && link.Text.Contains('>')) 
                        {
                            //if text is an html formatted icon and contains a <tag>
                            //ex. <span class='fa fa-chevron-right'></span>
                            span.InnerHtml.AppendHtml(link.Text);
                        }
                        else {
                            // if text should be html encoded
                            span.InnerHtml.Append(link.Text);
                        }

                        li.InnerHtml.AppendHtml(span);
                    }
                             
                }
       
                output.Content.AppendHtml(li);
            }

            output.Attributes.Clear();
            output.Attributes.Add("class", UlCssClass);
            
        }

        private string GeneratePageUrl(long pageNumber)
        {
            var routeValues = RouteValues.ToDictionary(
                    kvp => kvp.Key,
                    kvp => (object)kvp.Value,
                    StringComparer.OrdinalIgnoreCase);

            if (!routeValues.ContainsKey(PageNumberParam))
            {
                routeValues.Add(PageNumberParam, pageNumber);
            }

            IEnumerable<KeyValuePair<string, StringValues>> blockedQuery= Enumerable.Empty<KeyValuePair<string,StringValues>>();
            if (PreserveAmbientQueryString)
            {
                blockedQuery = AddCurrentQueryString(routeValues);
            }
            

            if (Route != null)
            {
                return AppendBlockedQuery(urlHelper.Link(Route, routeValues), blockedQuery); // endpoint route return null when no route be found
            }
            else if (BaseHref != null)
            {
                if (BaseHref.StartsWith("~/"))
                {
                    BaseHref = urlHelper.Content(BaseHref);
                }
                var start = "?";
                if (BaseHref.Contains("?")) start = "&";

                return $"{BaseHref}{start}{routeValues.Select(x => $"{x.Key}={x.Value}").Aggregate((current, next) => $"{current}&{next}")}";
            }

            //========================================
            //    it seems no need for the pure number
            //=========================================

            //return pageNumber.ToString();

            return AppendBlockedQuery(urlHelper.Action(Action, Controller, routeValues), blockedQuery); // endpoint route return null when no route be found
        }

        /// <summary>
        /// add Current Query item to routeValues
        /// </summary>
        /// <param name="current">current routeValues</param>
        /// <returns>a set of query can not be processed</returns>
        private IEnumerable<KeyValuePair<string,StringValues>> AddCurrentQueryString(Dictionary<string, object> current)
        {
            var blocked = Enumerable.Empty<KeyValuePair<string, StringValues>>();
            foreach (var item in urlHelper.ActionContext.HttpContext.Request.Query)
            {
                if (ViewContext.ActionDescriptor.RouteValues.ContainsKey(item.Key))
                {
                    // IN MVC  there are max to three values [controller,action,area], not allocate a list but an IEnumerable
                    blocked = blocked.Append(item);
                }
                else if (!current.ContainsKey(item.Key))
                {
                    current.Add(item.Key, item.Value);
                }
                
            }
            return blocked;
        }

        private string AppendBlockedQuery(string url, IEnumerable<KeyValuePair<string,StringValues>> blockedQuery)
        {
            if (string.IsNullOrEmpty(url))
            {
                return string.Empty; // fix NullReferenceException
            }
            if(blockedQuery == null && !blockedQuery.Any())
            {
                return url ?? string.Empty;
            }
            var flatedQuery = blockedQuery.OrderBy(x=>x.Key)
                .SelectMany(x => x.Value.Select(v => new KeyValuePair<string, string>(x.Key, v)))
                .Select(x=> $"{WebUtility.UrlEncode(x.Key)}={WebUtility.UrlEncode(x.Value)}");
            var queryString = string.Join("&", flatedQuery);
            if (string.IsNullOrEmpty(queryString))
            {
                return url ?? string.Empty;
            }
            if (url.Contains("?"))
            {
                return $"{url}&{queryString}";
            }
            else
            {
                return $"{url}?{queryString}";
            }

        }

    }
}
