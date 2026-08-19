namespace ETS2LA.UI.Localization;

/// <summary>
/// 中文 UI 字符串与枚举展示映射。持久化/API 键保持英文，仅用于界面展示。
/// </summary>
public static class UiStrings
{
    public const string NotAvailable = "不可用";
    public const string Unknown = "未知";
    public const string NoLimit = "无限制";
    public const string Unbound = "未绑定";

    public static string FormatEnumOption(string option) => option switch
    {
        // 通用
        "Slow" => "慢",
        "Normal" => "正常",
        "Fast" => "快",
        "Near" => "近",
        "Far" => "远",
        "Off" => "关闭",
        "Medium" => "中等",
        "Low" => "低",
        "High" => "高",
        "Extreme" => "极高",
        // 辅助驾驶
        "SpeedLimit" => "限速",
        "CurrentSpeed" => "当前速度",
        "Visual" => "视觉提醒",
        "Chime" => "提示音",
        "Late" => "较晚",
        "Early" => "较早",
        // 显示/数据
        "MatchGame" => "匹配游戏",
        "Metric" => "公制",
        "Imperial" => "英制",
        "Scientific" => "科学",
        // 轴类型
        "Centered" => "居中",
        "Inverted" => "反转",
        "SplitNegative" => "分离负向",
        "SplitPositive" => "分离正向",
        "SplitNeg" => "分离负向",
        "SplitPos" => "分离正向",
        _ => option
    };

    public static string ThemeToDisplay(string theme) => theme switch
    {
        "Light" => "浅色",
        "Dark" => "深色",
        "System" => "跟随系统",
        "浅色" or "深色" or "跟随系统" => theme,
        _ => theme
    };

    public static string ThemeToStorage(string display) => display switch
    {
        "浅色" => "Light",
        "深色" => "Dark",
        "跟随系统" => "System",
        _ => display
    };

    public static string AccentToDisplay(string accent) => accent switch
    {
        "Orange" => "橙色",
        "Blue" => "蓝色",
        "Green" => "绿色",
        "Purple" => "紫色",
        "Red" => "红色",
        "Yellow" => "黄色",
        _ => accent
    };

    public static string AccentToStorage(string display) => display switch
    {
        "橙色" => "Orange",
        "蓝色" => "Blue",
        "绿色" => "Green",
        "紫色" => "Purple",
        "红色" => "Red",
        "黄色" => "Yellow",
        _ => display
    };

    public static string PluginCardAutomation(bool installed, string name) =>
        installed ? $"已安装插件卡片，{name}，按钮" : $"未安装插件卡片，{name}，按钮";

    /// <summary>已知插件 ID → (中文名, 中文描述)</summary>
    public static readonly Dictionary<string, (string Name, string Description)> ChinesePlugins = new()
    {
        ["tumppi066.pathlib"] = ("路径库", "为其他插件提供路径规划相关的工具和数据结构。"),
        ["tumppi066.pathfinding"] = ("路径规划", "发布已解析的导航路径数据，供其他插件使用。"),
        ["tumppi066.laneassist"] = ("车道辅助", "提供简单的车道辅助功能，使用路径规划数据进行车道保持。"),
        ["tumppi066.internalvisualization"] = ("内部可视化", "打开覆盖层，以易于阅读的方式显示内部地图、车辆和遥测数据。"),
        ["tumppi066.pidlib"] = ("PID 控制库", "为其他插件提供 PID 控制器实现。"),
        ["tumppi066.adaptivecruisecontrol"] = ("自适应巡航控制", "提供基于遥测和路径数据的自适应巡航控制功能。"),
        ["tumppi066.map"] = ("地图数据", "提供地图相关数据供其他插件使用。"),
        ["tumppi066.navigation"] = ("导航", "提供导航路径与路线规划支持。"),
        ["tumppi066.traffic"] = ("交通感知", "检测并处理周围交通车辆信息。"),
        ["tumppi066.visualization"] = ("可视化", "在覆盖层中显示调试与可视化信息。"),
        ["ets2la.sdk"] = ("SDK", "Euro Truck Simulator 2 / American Truck Simulator 游戏 SDK 接口。"),
        ["ets2la.telemetry"] = ("遥测", "读取游戏遥测数据并分发给其他插件。"),
    };
}
