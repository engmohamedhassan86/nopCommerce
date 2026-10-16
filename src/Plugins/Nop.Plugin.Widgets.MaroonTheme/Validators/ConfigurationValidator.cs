using FluentValidation;
using Nop.Plugin.Widgets.MaroonTheme.Models;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.Widgets.MaroonTheme.Validators;

/// <summary>
/// Represents configuration model validator
/// </summary>
public class ConfigurationValidator : BaseNopValidator<ConfigurationModel>
{
    #region Fields

    private const string HexColorPattern = "^#[0-9A-Fa-f]{6}$";

    #endregion

    #region Ctor

    public ConfigurationValidator(ILocalizationService localizationService)
    {
        RuleFor(model => model.PrimaryColor)
            .Matches(HexColorPattern)
            .WithMessageAwait(localizationService.GetResourceAsync("Plugins.Widgets.MaroonTheme.PrimaryColor.Invalid"));

        RuleFor(model => model.HoverColor)
            .Matches(HexColorPattern)
            .WithMessageAwait(localizationService.GetResourceAsync("Plugins.Widgets.MaroonTheme.HoverColor.Invalid"));
    }

    #endregion
}
