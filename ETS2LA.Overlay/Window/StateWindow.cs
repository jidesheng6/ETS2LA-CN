using Hexa.NET.ImGui;
using ETS2LA.Backend.Events;
using ETS2LA.Controls;
using ETS2LA.State;
using System.Numerics;

namespace ETS2LA.Overlay.Window;

class StateWindow : InternalWindow
{
    private float accConstraintTargetSpeed = float.NaN;
    private string accActiveConstraintSource = "None";

    private void Text(string text)
    {
        ImGui.TextColored(new Vector4(0.9f, 0.9f, 0.9f, 1f), text);
    }

    private void DescriptionText(string text)
    {
        ImGui.TextColored(new Vector4(0.7f, 0.7f, 0.7f, 1f), text);
    }
    
    private void ColoredBoolean(bool value, bool invert = false)
    {
        if (invert) value = !value;
        Vector4 color = value ? new Vector4(0.5f, 1f, 0.5f, 1f) : new Vector4(1f, 0.5f, 0.5f, 1f);
        if (invert) value = !value; // revert back to original value for text
        ImGui.TextColored(color, value ? "是" : "否");
    }

    private static string SteeringLevelText(SteeringAssists level)
    {
        return level switch
        {
            SteeringAssists.None => "未启用",
            SteeringAssists.LaneKeep => "车道保持",
            SteeringAssists.Full => "全功能",
            _ => level.ToString()
        };
    }

    private static string LongitudinalLevelText(LongitudinalAssists level)
    {
        return level switch
        {
            LongitudinalAssists.None => "未启用",
            LongitudinalAssists.EmergencyBraking => "紧急制动",
            LongitudinalAssists.AdaptiveCruiseControl => "自适应巡航",
            _ => level.ToString()
        };
    }

    private static string UnitsText(Units units)
    {
        return units switch
        {
            Units.Metric => "公制",
            Units.Imperial => "英制",
            _ => units.ToString()
        };
    }

    private void UpdateAccConstraintTarget(float speed)
    {
        accConstraintTargetSpeed = speed;
    }

    private void UpdateAccConstraintSource(string source)
    {
        accActiveConstraintSource = source;
    }

    public StateWindow()
    {
        Definition = new WindowDefinition
        {
            Title = "状态信息",
            Flags = ImGuiWindowFlags.AlwaysAutoResize,
        };

        IsWindowOpen = false;

        Events.Current.Subscribe<float>("AdaptiveCruiseControl.ConstraintTargetSpeed", UpdateAccConstraintTarget);
        Events.Current.Subscribe<string>("AdaptiveCruiseControl.ActiveConstraintSource", UpdateAccConstraintSource);

        Render = () =>
        {
            DescriptionText("目标转向等级："); ImGui.SameLine(); Text(SteeringLevelText(ApplicationState.Current.DesiredSteeringLevel));

            DescriptionText("暂停转向辅助："); ImGui.SameLine(); ColoredBoolean(ApplicationState.Current.PauseSteeringAssist, invert: true);

            DescriptionText("目标纵向控制："); ImGui.SameLine(); Text(LongitudinalLevelText(ApplicationState.Current.DesiredLongitudinalLevel));

            DescriptionText("暂停纵向辅助："); ImGui.SameLine(); ColoredBoolean(ApplicationState.Current.PauseLongitudinalAssist, invert: true);

            float speed = ApplicationState.Current.DesiredSpeed;
            DescriptionText("目标速度："); ImGui.SameLine(); Text($"{speed * 3.6f:0.0} km/h");

            float constraintTarget = accConstraintTargetSpeed;
            if (float.IsFinite(constraintTarget))
            {
                DescriptionText("ACC约束目标："); ImGui.SameLine(); Text($"{constraintTarget * 3.6f:0.0} km/h");
                DescriptionText("约束来源："); ImGui.SameLine(); Text(accActiveConstraintSource);
            }
        };
    }
}
