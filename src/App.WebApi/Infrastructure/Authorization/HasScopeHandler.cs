using App.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;

namespace App.WebApi.Infrastructure.Authorization
{
    /// <summary>
    /// This handler will be called for evaluating if the user has the specific
    /// scope(permission) in its claims
    /// </summary>
    public class HasScopeHandler : AuthorizationHandler<HasScopeRequirement>
    {
        private readonly AuthConfig _authConfig;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public HasScopeHandler(IOptions<AuthConfig> options)
        {
            _authConfig = options.Value;
        }
        ///<inheritdoc/>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasScopeRequirement requirement)
        {
            // If user does not have the scope claim, get out of here
            if (!context.User.HasClaim(c => c.Type == "scope" && c.Issuer == _authConfig.Authority))
                return Task.CompletedTask;

            // Split the scopes string into an array
            var scope = context.User.FindFirst(c => c.Type == "scope"
                                                  && c.Issuer == _authConfig.Authority
                                                  && c.Value == requirement.Scope);

            // Succeed if the scope exists
            if (scope != null)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
