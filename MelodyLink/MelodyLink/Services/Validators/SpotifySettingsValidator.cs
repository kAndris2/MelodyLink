using MelodyLink.Models;

namespace MelodyLink.Services.Validators
{
    public class SpotifySettingsValidator : ConfigValidatorBase<SpotifySettings>
    {
        protected override void ValidateSingleFields()
        {
            var requiredSingleFields = new List<string>()
            {
                nameof(config.ClientId),
                nameof(config.ClientSecret),
                nameof(config.RedirectUrl)
            };

            CheckSingleFields<SpotifySettings>(requiredSingleFields, config);
        }

        protected override void ValidateListFields()
        {
        }

        protected override void ValidateObjectFields()
        {
        }
    }
}