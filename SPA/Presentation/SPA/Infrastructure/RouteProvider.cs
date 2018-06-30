using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using MyApp.Web.Framework.Routing;

namespace SPA.Infrastructure
{
    /// <inheritdoc />
    public class RouteProvider : IRouteProvider
    {
        /// <inheritdoc />
        public void RegisterRoutes(IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute(
                name: "default",
                template: "api/{controller}/{action=Index}/{id?}");
        }

        /// <inheritdoc />
        public int Priority => 0;
    }
}
