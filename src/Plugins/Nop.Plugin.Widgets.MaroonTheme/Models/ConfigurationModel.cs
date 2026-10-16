using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Widgets.MaroonTheme.Models;

public record ConfigurationModel : BaseNopModel
{
    public int ActiveStoreScopeConfiguration { get; set; }

    [NopResourceDisplayName("Plugins.Widgets.MaroonTheme.Enabled")]
    public bool Enabled { get; set; }
    public bool Enabled_OverrideForStore { get; set; }

    [NopResourceDisplayName("Plugins.Widgets.MaroonTheme.PrimaryColor")]
    public string PrimaryColor { get; set; }
    public bool PrimaryColor_OverrideForStore { get; set; }

    [NopResourceDisplayName("Plugins.Widgets.MaroonTheme.HoverColor")]
    public string HoverColor { get; set; }
    public bool HoverColor_OverrideForStore { get; set; }
}
