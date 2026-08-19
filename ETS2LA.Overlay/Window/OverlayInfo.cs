using Hexa.NET.ImGui;
using ETS2LA.Controls;
using System.Numerics;

namespace ETS2LA.Overlay.Window;

class OverlayInfoWindow : InternalWindow
{
    public OverlayInfoWindow()
    {
        Definition = new WindowDefinition
        {
            Title = "覆盖层说明",
            Flags = ImGuiWindowFlags.AlwaysAutoResize,
        };

        IsWindowOpen = false;

        Render = () =>
        {
            ImGui.Text("这是 ETS2LA 的游戏内覆盖层。");
            ImGui.TextColored(new Vector4(0.7f, 0.7f, 0.7f, 1f), "插件可以使用 ImGui 在游戏画面上绘制窗口。默认只开启控制台，其它窗口需在交互模式下手动打开。");
            ImGui.Separator();
            ImGui.Text("按住以下按键可与覆盖层交互：");
            ImGui.SameLine();
            var controls = ControlsBackend.Current.GetRegisteredControls();        
            var interactKey = controls.FirstOrDefault(c => c.Definition.Id == OverlayHandler.Current.Interact.Id);

            ImGui.PushFont(OverlayHandler.Current.Fonts[FontStyle.Bold], 18f);
            if (interactKey != null)
                ImGui.TextColored(new Vector4(1f, 0.5f, 0.5f, 1f), interactKey.ControlId.ToString());
            else 
                ImGui.TextColored(new Vector4(1f, 0.5f, 0.5f, 1f), "未绑定");
            ImGui.PopFont();
            
            ImGui.SameLine();
            ImGui.Text("（可在设置中更改）");
        };
    }
}
