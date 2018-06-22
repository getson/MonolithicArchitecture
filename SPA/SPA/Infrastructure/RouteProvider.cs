using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using MyApp.Web.Framework.Routing;

namespace SPA.Routing
{
    /// <inheritdoc />
    public partial class RouteProvider : IRouteProvider
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
