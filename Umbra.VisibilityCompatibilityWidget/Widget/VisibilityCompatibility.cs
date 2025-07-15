using System.Collections.Generic;
using Umbra.Widgets;
using Umbra.Common;
using Dalamud.Plugin.Services;
using Dalamud.Game.Gui.Toast;
using Umbra.Game;
using FFXIVClientStructs.FFXIV.Common.Component.BGCollision;
using Una.Drawing;

namespace Umbra.VisibilityCompatibilityWidget.Widget;

[InteropToolbarWidget(
    "VisibilityCompatibility",
    "Visibility Widget",
    "A widget for Visibility plugin.",
    "Visibility"
)]
public class VisibilityCompatibility(
    WidgetInfo                  info,
    string?                     guid         = null,
    Dictionary<string, object>? configValues = null
) : StandardToolbarWidget(info, guid, configValues)
{
    public bool enabled = false;
    public override MenuPopup? Popup { get; } = null;

    private readonly IChatSender _chatSender = Framework.Service<IChatSender>();

    private IToastGui ToastGui { get; set; } = Framework.Service<IToastGui>();

    protected override StandardWidgetFeatures Features =>
        StandardWidgetFeatures.Text |
        StandardWidgetFeatures.Icon;

    protected override IEnumerable<IWidgetConfigVariable> GetConfigVariables()
    {
        return [
            ..base.GetConfigVariables(),
            new IntegerWidgetConfigVariable(
                "IconSize",
                I18N.Translate("Widgets.Standard.Config.IconSize.Name"),
                I18N.Translate("Widgets.Standard.Config.IconSize.Description"),
                0,
                0,
                42) { Category = I18N.Translate("Widget.ConfigCategory.WidgetAppearance") }
        ];
    }

    protected override void OnLoad()
    {
        SetGameIconId(60647);
        Node.OnClick += _ => SwitchVisibilityState();
        Node.OnRightClick += _ => OpenVisibilityConfig();
    }

    private void SwitchVisibilityState ()
    {
        if (enabled)
        {
            _chatSender.Send("/pvis enabled off");
            ToastGui.ShowNormal("Players visible", new ToastOptions { Speed = ToastSpeed.Fast });
            SetConfigValue("DesaturateIcon", true);
        }
        else
        {
            _chatSender.Send("/pvis enabled on");
            ToastGui.ShowNormal("Players not visible", new ToastOptions { Speed = ToastSpeed.Fast });
            SetConfigValue("DesaturateIcon", false);
        }

        enabled = !enabled;
    }

    private void OpenVisibilityConfig ()
    {
        _chatSender.Send("/pvis");
    }
}
