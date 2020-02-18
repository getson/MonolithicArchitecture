using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace BinaryOrigin.SeedWork.WebApi.Authorization
{
    /// <summary>
    /// This handler will be called for evaluating if the user has the specific
    /// scope(permission) in its claims
    /// </summary>
    public class HasScopeHandler : AuthorizationHandler<HasScopeRequirement>
    {
        private readonly string _issuer;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="issuer">Auth0 tenant</param>
        public HasScopeHandler(string issuer)
        {
            _issuer = issuer;
        }
        ///<inheritdoc/>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasScopeRequirement requirement)
        {
            // If user does not have the scope claim, get out of here
            if (!context.User.HasClaim(c => c.Type == "scope" && c.Issuer == _issuer))
                return Task.CompletedTask;

            // Split the scopes string into an array
            var scope = context.User.FindFirst(c => c.Type == "scope"
                                                  && c.Issuer == _issuer
                                                  && c.Value == requirement.Scope);

            // Succeed if the scope exists
            if (scope != null)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
