using MelodyLink.Models;

namespace MelodyLink.Services.Validators
{
    public class SyncSettingsValidator : ConfigValidatorBase<SyncSettings>
    {
        protected override void ValidateSingleFields()
        {
            var requiredSingleFields = new List<string>()
            {
                nameof(config.Source),
                nameof(config.Target)
            };

            CheckSingleFields<SyncSettings>(requiredSingleFields, config);
        }

        protected override void ValidateListFields()
        {
        }

        protected override void ValidateObjectFields()
        {
        }
    }
}