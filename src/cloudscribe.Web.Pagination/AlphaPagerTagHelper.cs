// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2015-10-29
// Last Modified:			2016-05-16
// 

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace cloudscribe.Web.Pagination
{
    [HtmlTargetElement("cs-alphapager")]
    public class AlphaPagerTagHelper : TagHelper
    {
        private const string ActionAttributeName = "asp-action";
        private const string ControllerAttributeName = "asp-controller";
        private const string FragmentAttributeName = "asp-fragment";
        private const string HostAttributeName = "asp-host";
        private const string ProtocolAttributeName = "asp-protocol";
        private const string RouteAttributeName = "asp-route";
        private const string RouteValuesDictionaryName = "asp-all-route-data";
        private const string RouteValuesPrefix = "asp-route-";

        public AlphaPagerTagHelper(
            IHtmlGenerator generator)
        {
            Generator = generator;
        }

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        protected IHtmlGenerator Generator { get; }

        [HtmlAttributeName("cs-alphabet")]
        public string Alphabet { get; set; } = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// a list of which alpha chars actually have results
        /// so that ones without data can be non links
        /// if not provided then always make a link for every letter
        /// </summary>
        [HtmlAttributeName("cs-populated-letters")]
        public IEnumerable<string> PopulatedLetters { get; set; } = null;

        [HtmlAttributeName("cs-selected-letter")]
        public string SelectedLetter { get; set; } = string.Empty;

        [HtmlAttributeName("cs-selected-letter-param")]
        public string SelectedLetterParam { get; set; } = "letter";

        [HtmlAttributeName("cs-all-label")]
        public string AllLabel { get; set; } = "All";

        [HtmlAttributeName("cs-all-value")]
        public string AllValue { get; set; } = string.Empty;

        [HtmlAttributeName("cs-include-numbers")]
        public bool IncludeNumbers { get; set; } = false;

        [HtmlAttributeName("cs-alphapager-ul-class")]
        public string UlCssClass { get; set; } = "pagination alpha";

        [HtmlAttributeName("cs-pager-li-current-class")]
        public string LiCurrentCssClass { get; set; } = "active";

        [HtmlAttributeName("cs-pager-li-non-active-class")]
        public string LiNonActiveCssClass { get; set; } = "inactive";

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


        private string baseHref = string.Empty;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            //setup
            
            List<string> alphabetList;
            if (string.IsNullOrEmpty(Alphabet))
            {
                alphabetList = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray().ToStringList();
            }
            else
            {
                alphabetList = Alphabet.ToCharArray().ToStringList();
            }

            if (string.IsNullOrEmpty(AllLabel))
            {
                AllLabel = "All";
            }
            alphabetList.Insert(0, AllLabel);
            if (IncludeNumbers)
            {
                alphabetList.Insert(1, "0-9");
            }

            var numbers = Enumerable.Range(0, 10).Select(i => i.ToString());

            //change the cs-alphapager element into a ul
            output.TagName = "ul";

            string querySeparator;

            //prepare things needed by generatpageeurl function
        
            TagBuilder linkTemplate = GenerateLinkTemplate();
            baseHref = linkTemplate.Attributes["href"];
            querySeparator = baseHref.Contains("?") ? "&" : "?";
            baseHref = baseHref + querySeparator + SelectedLetterParam + "=";

            foreach (var letter in alphabetList)
            {
                var li = new TagBuilder("li");
                
                if (letter == "All" || PopulatedLetters == null || PopulatedLetters.Contains(letter)
                    || (PopulatedLetters.Intersect(numbers).Any() && letter == "0-9"))
                {
                    if (SelectedLetter == letter || string.IsNullOrEmpty(SelectedLetter) && letter == "All")
                    {
                        li.AddCssClass(LiCurrentCssClass);
                        var span = new TagBuilder("span");
                        if(!string.IsNullOrWhiteSpace(LinkCurrentCssClass))
                        {
                            span.AddCssClass(LinkCurrentCssClass);
                        }
                        span.InnerHtml.Append(letter);
                        li.InnerHtml.AppendHtml(span);

                    }
                    else
                    {
                        var a = new TagBuilder("a");

                        if (letter == AllLabel)
                        {
                            if(AllValue.Length > 0)
                            {
                                a.MergeAttribute("href", GeneratePageUrl(AllValue));
                            }
                            else
                            {
                                a.MergeAttribute("href", linkTemplate.Attributes["href"]);
                            }
                            
                        }
                        else
                        {
                            a.MergeAttribute("href", GeneratePageUrl(letter));
                        }

                        if (!string.IsNullOrWhiteSpace(LinkOtherCssClass))
                        {
                            a.AddCssClass(LinkOtherCssClass);
                        }

                        a.InnerHtml.Append(letter);
                        li.InnerHtml.AppendHtml(a);

                    }
                }
                else
                {
                    li.AddCssClass(LiNonActiveCssClass);
                    var span = new TagBuilder("span");
                    span.InnerHtml.Append(letter);
                    li.InnerHtml.AppendHtml(span);

                }
                output.Content.AppendHtml(li);
            }


            output.Attributes.Clear(); // remove cs- attributes from output
            output.Attributes.Add("class", UlCssClass);
        }

        private string GeneratePageUrl(string letterValue)
        {
            return baseHref + letterValue;
        }

        private TagBuilder GenerateLinkTemplate()
        {
            // here I'm just letting the framework generate an actionlink
            // in order to resolve the link url from the routing info
            // there may be a better way to do this
            // if I could find the implementation for Generator.GenerateActionLink
            // maybe I would get a better idea

            var routeValues = RouteValues.ToDictionary(
                    kvp => kvp.Key,
                    kvp => (object)kvp.Value,
                    StringComparer.OrdinalIgnoreCase);

            TagBuilder tagBuilder;
            if (Route == null)
            {
                tagBuilder = Generator.GenerateActionLink(
                    ViewContext,
                    linkText: string.Empty,
                    actionName: Action,
                    controllerName: Controller,
                    protocol: Protocol,
                    hostname: Host,
                    fragment: Fragment,
                    routeValues: routeValues,
                    htmlAttributes: null);
            }
            else if (Action != null || Controller != null)
            {
                // Route and Action or Controller were specified. Can't determine the href attribute.
                throw new InvalidOperationException("not enough info to build pager links");
            }
            else
            {
                tagBuilder = Generator.GenerateRouteLink(
                    ViewContext,
                    linkText: string.Empty,
                    routeName: Route,
                    protocol: Protocol,
                    hostName: Host,
                    fragment: Fragment,
                    routeValues: routeValues,
                    htmlAttributes: null);
            }

            return tagBuilder;
        }

    }
}
