using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace BinaryOrigin.SeedWork.WebApi.Authorization
{
    /// <summary>
    /// Authorization policy provider to automatically turn all permissions of a user into a ASP.NET Core authorization policy
    /// </summary>
    /// <seealso cref="DefaultAuthorizationPolicyProvider" />
    public class ScopeAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScopeAuthorizationPolicyProvider"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public ScopeAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets a <see cref="T:Microsoft.AspNetCore.Authorization.AuthorizationPolicy" /> from the given <paramref name="policyName" />
        /// </summary>
        /// <param name="policyName">The policy name to retrieve.</param>
        /// <returns>
        /// The named <see cref="T:Microsoft.AspNetCore.Authorization.AuthorizationPolicy" />.
        /// </returns>
        public async override Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            // check static policies first
            var policy = await base.GetPolicyAsync(policyName);

            if (policy == null)
            {
                policy = new AuthorizationPolicyBuilder()
                    .AddRequirements(new HasScopeRequirement(policyName))
                    .Build();
            }

            return policy;
        }

    }
}
