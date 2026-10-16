using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Widgets.MaroonTheme.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Widgets.MaroonTheme.Controllers;

[Area(AreaNames.ADMIN)]
[AuthorizeAdmin]
[AutoValidateAntiforgeryToken]
public class MaroonThemeController : BasePluginController
{
    #region Fields

    protected readonly ILocalizationService _localizationService;
    protected readonly INotificationService _notificationService;
    protected readonly ISettingService _settingService;
    protected readonly IStoreContext _storeContext;

    #endregion

    #region Ctor

    public MaroonThemeController(ILocalizationService localizationService,
        INotificationService notificationService,
        ISettingService settingService,
        IStoreContext storeContext)
    {
        _localizationService = localizationService;
        _notificationService = notificationService;
        _settingService = settingService;
        _storeContext = storeContext;
    }

    #endregion

    #region Methods

    [CheckPermission(StandardPermission.Configuration.MANAGE_WIDGETS)]
    public async Task<IActionResult> Configure()
    {
        //load settings for a chosen store scope
        var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
        var maroonThemeSettings = await _settingService.LoadSettingAsync<MaroonThemeSettings>(storeScope);

        var model = new ConfigurationModel
        {
            Enabled = maroonThemeSettings.Enabled,
            PrimaryColor = maroonThemeSettings.PrimaryColor,
            HoverColor = maroonThemeSettings.HoverColor,
            ActiveStoreScopeConfiguration = storeScope
        };

        if (storeScope > 0)
        {
            model.Enabled_OverrideForStore = await _settingService.SettingExistsAsync(maroonThemeSettings, x => x.Enabled, storeScope);
            model.PrimaryColor_OverrideForStore = await _settingService.SettingExistsAsync(maroonThemeSettings, x => x.PrimaryColor, storeScope);
            model.HoverColor_OverrideForStore = await _settingService.SettingExistsAsync(maroonThemeSettings, x => x.HoverColor, storeScope);
        }

        return View("~/Plugins/Widgets.MaroonTheme/Views/Configure.cshtml", model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Configuration.MANAGE_WIDGETS)]
    public async Task<IActionResult> Configure(ConfigurationModel model)
    {
        if (!ModelState.IsValid)
            return await Configure();

        //load settings for a chosen store scope
        var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
        var maroonThemeSettings = await _settingService.LoadSettingAsync<MaroonThemeSettings>(storeScope);

        maroonThemeSettings.Enabled = model.Enabled;
        maroonThemeSettings.PrimaryColor = model.PrimaryColor;
        maroonThemeSettings.HoverColor = model.HoverColor;

        await _settingService.SaveSettingOverridablePerStoreAsync(maroonThemeSettings, x => x.Enabled, model.Enabled_OverrideForStore, storeScope, false);
        await _settingService.SaveSettingOverridablePerStoreAsync(maroonThemeSettings, x => x.PrimaryColor, model.PrimaryColor_OverrideForStore, storeScope, false);
        await _settingService.SaveSettingOverridablePerStoreAsync(maroonThemeSettings, x => x.HoverColor, model.HoverColor_OverrideForStore, storeScope, false);

        //now clear settings cache; this also raises the Setting entity events that clear our cached style block
        await _settingService.ClearCacheAsync();

        _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

        return await Configure();
    }

    #endregion
}
