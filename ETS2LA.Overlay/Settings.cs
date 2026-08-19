using ETS2LA.Settings;

namespace ETS2LA.Overlay;

[Serializable]
public class OverlaySettings
{
    public bool LimitFramerate = true;
    public int MaxFramerate = 30;
    public bool SupportMultipleViewports = false;
    public bool ShowPerformanceOverlay = false;

    // AR：默认关闭，避免覆盖层在切出游戏时卡死、也减少默认占用。
    public bool RenderAR = false;
    public bool SimplifiedGraphics = false;
    public bool DontRenderWhenPaused = true;
    public float MaxARDistance = 150.0f;

    /// <summary>用于一次性把旧配置迁移到「默认仅控制台」。</summary>
    public bool AppliedConsoleOnlyDefaults = false;
}

public class OverlaySettingsHandler
{
    private static readonly Lazy<OverlaySettingsHandler> _instance = new(() => new OverlaySettingsHandler());
    public static OverlaySettingsHandler Current => _instance.Value;

    private SettingsHandler _settingsHandler;
    private OverlaySettings _settings;

    public event Action<OverlaySettings>? OnSettingsUpdated;

    public OverlaySettingsHandler()
    {
        _settingsHandler = new SettingsHandler();
        _settings = _settingsHandler.Load<OverlaySettings>("OverlaySettings.json");
        if (!_settings.AppliedConsoleOnlyDefaults)
        {
            _settings.RenderAR = false;
            _settings.ShowPerformanceOverlay = false;
            _settings.AppliedConsoleOnlyDefaults = true;
            _settingsHandler.Save("OverlaySettings.json", _settings);
        }
        _settingsHandler.RegisterListener<OverlaySettings>("OverlaySettings.json", OnSettingsChanged);
    }

    public void Save()
    {
        _settingsHandler.Save<OverlaySettings>("OverlaySettings.json", _settings);
    }

    public OverlaySettings GetSettings()
    {
        return _settings;
    }

    private void OnSettingsChanged(OverlaySettings overlaySettings)
    {
        _settings = overlaySettings;
        OnSettingsUpdated?.Invoke(_settings);
    }
}
