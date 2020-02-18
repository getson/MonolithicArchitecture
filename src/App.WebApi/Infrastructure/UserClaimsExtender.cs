using BinaryOrigin.SeedWork.Core;
using Microsoft.AspNetCore.Authentication;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace App.WebApi
{
    public class UserClaimsExtender : IClaimsTransformation
    {
        private readonly string _authority;

        public UserClaimsExtender(string authority)
        {
            _authority = authority;
        }

        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            // Here you can extend claims

            var identity = principal.Identities.FirstOrDefault();
            if (identity == null)
            {
                return null;
            }
            // actually all the scopes will be provided to every user
            // but normally this is not a real world example
            var claims = ReflectionHelper.GetConstants<string>(typeof(Scopes))
                            .Select(x => new Claim("scope", x, ClaimValueTypes.String, _authority))
                            .ToList();

            claims.AddRange(identity.Claims);

            var claimsIdentity = new ClaimsIdentity(claims, identity.AuthenticationType);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            return Task.FromResult(claimsPrincipal);
        }
    }
}