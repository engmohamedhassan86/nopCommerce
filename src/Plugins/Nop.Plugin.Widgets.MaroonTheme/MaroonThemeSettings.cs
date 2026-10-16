using Nop.Core.Configuration;

namespace Nop.Plugin.Widgets.MaroonTheme;

/// <summary>
/// Represents plugin settings
/// </summary>
public class MaroonThemeSettings : ISettings
{
    #region Properties

    /// <summary>
    /// Gets or sets the primary accent color (hex, e.g. #800000)
    /// </summary>
    public string PrimaryColor { get; set; }

    /// <summary>
    /// Gets or sets the hover/active accent color (hex, e.g. #5c0000)
    /// </summary>
    public string HoverColor { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the maroon theme color override is enabled
    /// </summary>
    public bool Enabled { get; set; }

    #endregion
}
