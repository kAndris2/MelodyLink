using MelodyLink.Models;

namespace MelodyLink.Services.Validators
{
    public class LocalStorageSettingsValidator : ConfigValidatorBase<LocalStorageSettings>
    {
        protected override void ValidateSingleFields()
        {
            var requiredSingleFields = new List<string>()
            {
                nameof(config.FilePath)
            };

            CheckSingleFields<LocalStorageSettings>(requiredSingleFields, config);
        }

        protected override void ValidateListFields()
        {
            CheckListCount(config.Extensions?.Count, nameof(config.Extensions));
        }

        protected override void ValidateObjectFields()
        {
        }
    }
}