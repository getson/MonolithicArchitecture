using BinaryOrigin.SeedWork.Messages;
using Microsoft.Extensions.Localization;

namespace App.WebApi.Localization
{
    public class LocalizerService : ILocalizerService
    {
        private readonly IStringLocalizer<Resources> _localizer;

        public LocalizerService(IStringLocalizer<Resources> localizer)
        {
            _localizer = localizer;
        }

        public string Localize(string message)
        {
            return _localizer.GetString(message).Value;
        }
    }
}
