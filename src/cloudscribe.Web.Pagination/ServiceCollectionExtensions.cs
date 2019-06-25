using cloudscribe.Web.Pagination;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;

//namespace cloudscribe.Web.Pagination
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCloudscribePagination(this IServiceCollection services)
        {
            //services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.TryAddTransient<IBuildPaginationLinks, PaginationLinkBuilder>();

            return services;
        }
    }
}
