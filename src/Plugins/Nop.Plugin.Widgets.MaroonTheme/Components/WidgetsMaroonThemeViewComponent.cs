using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Services.Configuration;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.MaroonTheme.Components;

/// <summary>
/// Represents the view component that injects the maroon color override stylesheet
/// </summary>
public class WidgetsMaroonThemeViewComponent : NopViewComponent
{
    #region Fields

    private readonly IStaticCacheManager _staticCacheManager;
    private readonly ISettingService _settingService;
    private readonly IStoreContext _storeContext;

    private static readonly Regex HexColorRegex = new("^#[0-9A-Fa-f]{6}$", RegexOptions.Compiled);

    #endregion

    #region Ctor

    public WidgetsMaroonThemeViewComponent(IStaticCacheManager staticCacheManager,
        ISettingService settingService,
        IStoreContext storeContext)
    {
        _staticCacheManager = staticCacheManager;
        _settingService = settingService;
        _storeContext = storeContext;
    }

    #endregion

    #region Utilities

    /// <summary>
    /// Ensures the color is a safe 6-digit hex value, falling back to the provided default otherwise
    /// </summary>
    private static string SanitizeHexColor(string color, string fallback)
    {
        return !string.IsNullOrEmpty(color) && HexColorRegex.IsMatch(color) ? color : fallback;
    }

    /// <summary>
    /// Converts a sanitized 6-digit hex color into an "r, g, b" triplet for use in rgba()
    /// </summary>
    private static string HexToRgbTriplet(string hexColor)
    {
        var r = int.Parse(hexColor.Substring(1, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        var g = int.Parse(hexColor.Substring(3, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        var b = int.Parse(hexColor.Substring(5, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);

        return $"{r}, {g}, {b}";
    }

    /// <returns>The generated style block markup</returns>
    private static string BuildStyleBlock(string primaryColor, string hoverColor)
    {
        var primaryRgb = HexToRgbTriplet(primaryColor);

        var css = new StringBuilder();
        css.AppendLine("<style>");
        css.AppendLine(":root {");
        css.AppendLine($"  --accent-blue-color: {primaryColor};");
        css.AppendLine($"  --accent-blue-active-color: {hoverColor};");
        css.AppendLine("}");
        //header / navigation hover states (DefaultClean's header links already use the variable above;
        //these cover the remaining hard-coded hover colors)
        css.AppendLine(".header-links a:hover, .header-links a:focus {");
        css.AppendLine($"  color: {hoverColor};");
        css.AppendLine("}");
        //bootstrap-style buttons used outside the theme's own .button-1/.button-2 (which already follow the variable)
        css.AppendLine(".btn-primary {");
        css.AppendLine($"  background-color: {primaryColor};");
        css.AppendLine($"  border-color: {primaryColor};");
        css.AppendLine("}");
        css.AppendLine(".btn-primary:hover, .btn-primary:focus, .btn-primary:active {");
        css.AppendLine($"  background-color: {hoverColor};");
        css.AppendLine($"  border-color: {hoverColor};");
        css.AppendLine("}");
        //product price highlight
        css.AppendLine(".overview .product-price, .variant-overview .product-price {");
        css.AppendLine($"  color: {primaryColor};");
        css.AppendLine("}");
        //footer accents (already follow the variable above; explicit hover colors for clarity)
        css.AppendLine(".footer-tax-shipping a:hover, .footer-tax-shipping a:focus,");
        css.AppendLine(".footer-powered-by a:hover, .footer-powered-by a:focus {");
        css.AppendLine($"  color: {hoverColor};");
        css.AppendLine("}");
        //form focus state: DefaultClean's storefront inputs use bare input[type=...]/textarea/select
        //selectors (no .form-control class, which is a Bootstrap/Admin-area convention), and its own
        //styles.css hard-codes border-color: #ccc on focus, so both are targeted here
        css.AppendLine(".form-control:focus,");
        css.AppendLine("input[type=\"text\"]:focus, input[type=\"email\"]:focus, input[type=\"tel\"]:focus,");
        css.AppendLine("input[type=\"password\"]:focus, input[type=\"number\"]:focus, textarea:focus, select:focus {");
        css.AppendLine($"  border-color: {primaryColor};");
        css.AppendLine($"  box-shadow: 0 0 0 .2rem rgba({primaryRgb}, .25);");
        css.AppendLine("}");
        css.AppendLine("</style>");

        return css.ToString();
    }

    #endregion

    #region Methods

    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        var store = await _storeContext.GetCurrentStoreAsync();
        var maroonThemeSettings = await _settingService.LoadSettingAsync<MaroonThemeSettings>(store.Id);

        if (!maroonThemeSettings.Enabled)
            return Content(string.Empty);

        var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(MaroonThemeDefaults.StyleBlockCacheKey, store.Id);

        var styleBlock = await _staticCacheManager.GetAsync(cacheKey, () =>
        {
            var primaryColor = SanitizeHexColor(maroonThemeSettings.PrimaryColor, MaroonThemeDefaults.DefaultPrimaryColor);
            var hoverColor = SanitizeHexColor(maroonThemeSettings.HoverColor, MaroonThemeDefaults.DefaultHoverColor);

            return BuildStyleBlock(primaryColor, hoverColor);
        });

        return new HtmlContentViewComponentResult(new HtmlString(styleBlock));
    }

    #endregion
}
