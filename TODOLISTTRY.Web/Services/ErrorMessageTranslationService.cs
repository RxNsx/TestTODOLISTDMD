using Microsoft.Extensions.Localization;

namespace TODOLISTTRY.Web.Services
{
    public class ErrorMessageTranslationService
    {
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;
        public ErrorMessageTranslationService(IStringLocalizer<SharedResource> sharedLocalizer)
        {
            _sharedLocalizer = sharedLocalizer;
        }

        public string GetLocalizedError(string errorKey)
        {
            return _sharedLocalizer[errorKey];
        }
    }
}
