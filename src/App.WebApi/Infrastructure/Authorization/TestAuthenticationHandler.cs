using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace App.WebApi.Infrastructure.Authorization
{
    public class TestAuthenticationHandler : AuthenticationHandler<TestAuthenticationOptions>
    {
        public TestAuthenticationHandler(IOptionsMonitor<TestAuthenticationOptions> options, ILoggerFactory logger,
            UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authenticationTicket = new AuthenticationTicket(
                new ClaimsPrincipal(Options.Identity),
                new AuthenticationProperties(),
                "Test Scheme");

            return Task.FromResult(AuthenticateResult.Success(authenticationTicket));
        }
    }

    public static class TestAuthenticationExtensions
    {
        public static AuthenticationBuilder AddTestAuth(this AuthenticationBuilder builder, Action<TestAuthenticationOptions> configureOptions)
        {
            return builder.AddScheme<TestAuthenticationOptions, TestAuthenticationHandler>("Test Scheme", "Test Auth", configureOptions);
        }
    }

    public class TestAuthenticationOptions : AuthenticationSchemeOptions
    {
        public IEnumerable<string> Scopes { get; set; }
        public string Authority { get; set; }
        public virtual ClaimsIdentity Identity => new ClaimsIdentity(GetScopes());

        private IEnumerable<Claim> GetScopes()
        {
            var scopes = Scopes.Select(s =>
                           new Claim("scope", s, ClaimValueTypes.String, Authority)
                         ).ToList();

            scopes.Add(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", Guid.NewGuid().ToString()));

            return scopes;
        }

    }
}
