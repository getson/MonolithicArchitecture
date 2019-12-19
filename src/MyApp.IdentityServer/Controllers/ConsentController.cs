using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using IdentityServer4.Stores;
using IdentityServer4.Services;
using IdentityServer4.Models;
using MyApp.IdentityServer.Models;

namespace MyApp.IdentityServer.Controllers
{



    [ApiExplorerSettings(IgnoreApi = true)]
    public class ConsentController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IResourceStore _resourceStore;

        public ConsentController(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IResourceStore resourceStore)
        {
            _interaction = interaction;
            _clientStore = clientStore;
            _resourceStore = resourceStore;
        }

        [HttpGet("consent")]
        public async Task<IActionResult> Consent(string returnUrl)
        {
            var request = await _interaction.GetAuthorizationContextAsync(returnUrl);
            var client = await _clientStore.FindEnabledClientByIdAsync(request.ClientId);
            var resource = await _resourceStore.FindApiResourceAsync("api");

            var viewModel = new ConsentViewModel
            {
                ReturnUrl = returnUrl,
                ClientName = client.ClientName,
                ScopesRequested = resource.Scopes.Where(s => request.ScopesRequested.Contains(s.Name))
            };

            return View("Views/Consent.cshtml", viewModel);
        }

        [HttpPost("consent")]
        public async Task<IActionResult> Consent([FromForm]ConsentViewModel viewModel)
        {
            var request = await _interaction.GetAuthorizationContextAsync(viewModel.ReturnUrl);

            // Communicate outcome of consent back to identityserver
            var consentResponse = new ConsentResponse
            {
                ScopesConsented = viewModel.ScopesConsented
            };
            await _interaction.GrantConsentAsync(request, consentResponse);

            return Redirect(viewModel.ReturnUrl);
        }
    }
}

