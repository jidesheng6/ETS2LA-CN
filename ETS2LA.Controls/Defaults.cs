namespace ETS2LA.Controls.Defaults;

public static class DefaultControls
{
    public static ControlDefinition Assist { get; } = new ControlDefinition
    {
        Id = "ETS2LA.Controls.Assist",
        Name = "辅助功能",
        Description = "切换 ETS2LA 辅助功能的启用状态，不会更新目标速度；如需更新速度，请使用 SET。可在辅助功能设置中调整此按键和 SET 的行为。",
        DefaultKeybind = "N",
        Type = ControlType.Boolean
    };

    public static ControlDefinition SET { get; } = new ControlDefinition
    {
        Id = "ETS2LA.Controls.SET",
        Name = "SET/确认",
        Description = "与辅助功能键类似，但会按辅助功能设置中选择的方式执行；此按键也用于确认操作。",
        DefaultKeybind = "Left",
        Type = ControlType.Boolean
    };

    public static ControlDefinition Next { get; } = new ControlDefinition
    {
        Id = "ETS2LA.Controls.Next",
        Name = "下一步/取消",
        Description = "用于在 ETS2LA 菜单中前进，也可在确认操作时作为取消键。",
        DefaultKeybind = "Right",
        Type = ControlType.Boolean
    };

    public static ControlDefinition Increase { get; } = new ControlDefinition
    {
        Id = "ETS2LA.Controls.Increase",
        Name = "增加",
        Description = "将当前数值（例如目标速度）增加一个步长。未使用界面修饰键时，会按设置的速度步长提高目标速度。",
        DefaultKeybind = "Up",
        Type = ControlType.Boolean
    };

    public static ControlDefinition Decrease { get; } = new ControlDefinition
    {
        Id = "ETS2LA.Controls.Decrease",
        Name = "减少",
        Description = "将当前数值（例如目标速度）减少一个步长。未使用界面修饰键时，会按设置的速度步长降低目标速度。",
        DefaultKeybind = "Down",
        Type = ControlType.Boolean
    };

}
