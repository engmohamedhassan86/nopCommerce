using Nop.Core.Domain.Cms;
using Nop.Plugin.Widgets.MaroonTheme.Components;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.Widgets.MaroonTheme;

/// <summary>
/// Represents the Maroon Theme Color widget plugin
/// </summary>
public class MaroonThemePlugin : BasePlugin, IWidgetPlugin
{
    #region Fields

    private readonly ILocalizationService _localizationService;
    private readonly INopUrlHelper _nopUrlHelper;
    private readonly ISettingService _settingService;
    private readonly WidgetSettings _widgetSettings;

    #endregion

    #region Ctor

    public MaroonThemePlugin(ILocalizationService localizationService,
        INopUrlHelper nopUrlHelper,
        ISettingService settingService,
        WidgetSettings widgetSettings)
    {
        _localizationService = localizationService;
        _nopUrlHelper = nopUrlHelper;
        _settingService = settingService;
        _widgetSettings = widgetSettings;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Gets widget zones where this widget should be rendered
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the widget zones
    /// </returns>
    public Task<IList<string>> GetWidgetZonesAsync()
    {
        return Task.FromResult<IList<string>>(new List<string> { PublicWidgetZones.HeadHtmlTag });
    }

    /// <summary>
    /// Gets a configuration page URL
    /// </summary>
    public override string GetConfigurationPageUrl()
    {
        return _nopUrlHelper.RouteUrl(MaroonThemeDefaults.ConfigurationRouteName);
    }

    /// <summary>
    /// Gets a name of a view component for displaying widget
    /// </summary>
    /// <param name="widgetZone">Name of the widget zone</param>
    /// <returns>View component type</returns>
    public Type GetWidgetViewComponent(string widgetZone)
    {
        return typeof(WidgetsMaroonThemeViewComponent);
    }

    /// <summary>
    /// Install plugin
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public override async Task InstallAsync()
    {
        await _settingService.SaveSettingAsync(new MaroonThemeSettings
        {
            Enabled = true,
            PrimaryColor = MaroonThemeDefaults.DefaultPrimaryColor,
            HoverColor = MaroonThemeDefaults.DefaultHoverColor
        });

        if (!_widgetSettings.ActiveWidgetSystemNames.Contains(MaroonThemeDefaults.SystemName))
        {
            _widgetSettings.ActiveWidgetSystemNames.Add(MaroonThemeDefaults.SystemName);
            await _settingService.SaveSettingAsync(_widgetSettings);
        }

        await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
        {
            ["Plugins.Widgets.MaroonTheme.Enabled"] = "Enabled",
            ["Plugins.Widgets.MaroonTheme.Enabled.Hint"] = "Check to recolor the storefront's primary UI elements to the colors configured below.",
            ["Plugins.Widgets.MaroonTheme.PrimaryColor"] = "Primary color",
            ["Plugins.Widgets.MaroonTheme.PrimaryColor.Hint"] = "The main accent color applied to buttons, links, and highlights across the storefront. Use a 6-digit hex value, e.g. #800000.",
            ["Plugins.Widgets.MaroonTheme.PrimaryColor.Invalid"] = "Please enter a valid 6-digit hex color, e.g. #800000.",
            ["Plugins.Widgets.MaroonTheme.HoverColor"] = "Hover color",
            ["Plugins.Widgets.MaroonTheme.HoverColor.Hint"] = "The accent color used for hover/active states. Use a 6-digit hex value, e.g. #5c0000.",
            ["Plugins.Widgets.MaroonTheme.HoverColor.Invalid"] = "Please enter a valid 6-digit hex color, e.g. #5c0000.",
            ["Plugins.Widgets.MaroonTheme.Description"] = "<p>Recolors the storefront's primary UI color (header links, buttons, price highlights, footer accents, and form focus states) to the colors configured below. The Admin area is not affected.</p>"
        });

        await base.InstallAsync();
    }

    /// <summary>
    /// Uninstall plugin
    /// </summary>
    /// <returns>A task that represents the asynchronous operation</returns>
    public override async Task UninstallAsync()
    {
        await _settingService.DeleteSettingAsync<MaroonThemeSettings>();

        if (_widgetSettings.ActiveWidgetSystemNames.Contains(MaroonThemeDefaults.SystemName))
        {
            _widgetSettings.ActiveWidgetSystemNames.Remove(MaroonThemeDefaults.SystemName);
            await _settingService.SaveSettingAsync(_widgetSettings);
        }

        await _localizationService.DeleteLocaleResourcesAsync("Plugins.Widgets.MaroonTheme");

        await base.UninstallAsync();
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets a value indicating whether to hide this plugin on the widget list page in the admin area
    /// </summary>
    public bool HideInWidgetList => false;

    #endregion
}
