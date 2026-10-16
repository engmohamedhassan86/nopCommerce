using Nop.Core.Caching;

namespace Nop.Plugin.Widgets.MaroonTheme;

/// <summary>
/// Represents plugin constants
/// </summary>
public class MaroonThemeDefaults
{
    /// <summary>
    /// Gets the plugin system name
    /// </summary>
    public static string SystemName => "Widgets.MaroonTheme";

    /// <summary>
    /// Gets the configuration route name
    /// </summary>
    public static string ConfigurationRouteName => "Plugin.Widgets.MaroonTheme.Configure";

    /// <summary>
    /// Gets the default primary color
    /// </summary>
    public static string DefaultPrimaryColor => "#800000";

    /// <summary>
    /// Gets the default hover color
    /// </summary>
    public static string DefaultHoverColor => "#5c0000";

    /// <summary>
    /// Gets a key pattern to clear the cached style block
    /// </summary>
    public static string StyleBlockPrefix => "Nop.plugins.widgets.maroontheme.styleblock";

    /// <summary>
    /// Gets the cache key for the generated style block
    /// </summary>
    /// <remarks>
    /// {0} : store id
    /// </remarks>
    public static CacheKey StyleBlockCacheKey => new($"{StyleBlockPrefix}-{{0}}");
}
