using System.Collections.Generic;
using IdentityServer4.Models;

namespace MyApp.IdentityServer.Models
{
    public class ConsentViewModel
    {
        public string ReturnUrl { get; set; }
        public string ClientName { get; set; }
        public IEnumerable<Scope> ScopesRequested { get; set; }
        public string[] ScopesConsented { get; set; }
    }
}

