using Avalonia;
using Avalonia.Controls;
using ETS2LA.Logging;
using Avalonia.Styling;
using Huskui.Avalonia;
using ETS2LA.UI.Settings;
using Avalonia.Interactivity;

namespace ETS2LA.UI.Views.Settings;

public partial class ThemeSettings : UserControl
{
    private ThemeVariant CurrentTheme = Application.Current?.RequestedThemeVariant ?? ThemeVariant.Default;
    private AccentColor CurrentAccent = AccentColor.Orange;
    private bool loaded = false;

    public ThemeSettings()
    {
        InitializeComponent();
        UpdateIndex();
        
        loaded = true;
        UISettings settings = UISettingsHandler.Current.GetSettings();
        ChangeTheme(settings.Theme);
        ChangeAccentColor(settings.AccentColor);

    }

    private void UpdateIndex()
    {
        string key = CurrentTheme.Key.ToString() ?? "System";
        int index = key.ToLower() switch
        {
            "light" => 1,
            "dark" => 2,
            _ => 0,
        };
        this.Find<ComboBox>("ThemeCombobox")?.SelectedIndex = index;
        this.Find<ComboBox>("AccentColorCombobox")?.SelectedIndex = (int)CurrentAccent;
    }

    private void ChangeTheme(string theme)
    {
        if (!loaded) return;
        theme = theme switch
        {
            "浅色" => "Light",
            "深色" => "Dark",
            "跟随系统" => "System",
            _ => theme
        };
        CurrentTheme = theme.ToLower() switch
        {
            "light" => ThemeVariant.Light,
            "dark" => ThemeVariant.Dark,
            _ => ThemeVariant.Default,
        };
        Application.Current?.RequestedThemeVariant = CurrentTheme;
        UISettingsHandler.Current.GetSettings().Theme = theme;
        UpdateIndex();
    }

    private void OnThemeChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count > 0)
        {
            var themeString = ((ComboBoxItem)e.AddedItems[0]).Content;
            if (themeString == null) return;
            ChangeTheme((string)themeString);
        }
    }

    private void UpdateHuskuiTheme()
    {
        if (Application.Current?.Styles is { } styles)
        {
            // Find and update the HuskuiTheme
            foreach (var t in styles)
            {
                if (t is HuskuiTheme huskuiTheme)
                {
                    huskuiTheme.Accent = CurrentAccent;
                    break;
                }
            }
        }
    }

    private void ChangeAccentColor(string accent)
    {
        if (!loaded) return;
        accent = accent switch
        {
            "跟随系统" => "System",
            "中性" => "Neutral",
            "番茄红" => "Tomato",
            "红色" => "Red",
            "宝石红" => "Ruby",
            "深红" => "Crimson",
            "粉色" => "Pink",
            "梅紫" => "Plum",
            "紫色" => "Purple",
            "紫罗兰" => "Violet",
            "鸢尾紫" => "Iris",
            "靛蓝" => "Indigo",
            "蓝色" => "Blue",
            "青色" => "Cyan",
            "蓝绿色" => "Teal",
            "玉绿色" => "Jade",
            "绿色" => "Green",
            "草绿色" => "Grass",
            "青铜色" => "Bronze",
            "金色" => "Gold",
            "橙色" => "Orange",
            "琥珀色" => "Amber",
            "黄色" => "Yellow",
            "青柠色" => "Lime",
            "薄荷色" => "Mint",
            "天空蓝" => "Sky",
            _ => accent
        };
        CurrentAccent = accent switch
        {
            "System" => AccentColor.System,
            "Neutral" => AccentColor.Neutral,
            "Tomato" => AccentColor.Tomato,
            "Red" => AccentColor.Red,
            "Ruby" => AccentColor.Ruby,
            "Crimson" => AccentColor.Crimson,
            "Pink" => AccentColor.Pink,
            "Plum" => AccentColor.Plum,
            "Purple" => AccentColor.Purple,
            "Violet" => AccentColor.Violet,
            "Iris" => AccentColor.Iris,
            "Indigo" => AccentColor.Indigo,
            "Blue" => AccentColor.Blue,
            "Cyan" => AccentColor.Cyan,
            "Teal" => AccentColor.Teal,
            "Jade" => AccentColor.Jade,
            "Green" => AccentColor.Green,
            "Grass" => AccentColor.Grass,
            "Bronze" => AccentColor.Bronze,
            "Gold" => AccentColor.Gold,
            "Orange" => AccentColor.Orange,
            "Amber" => AccentColor.Amber,
            "Yellow" => AccentColor.Yellow,
            "Lime" => AccentColor.Lime,
            "Mint" => AccentColor.Mint,
            "Sky" => AccentColor.Sky,
            _ => AccentColor.Orange,
        };
        UISettingsHandler.Current.GetSettings().AccentColor = accent;
        UpdateHuskuiTheme();
        UpdateIndex();
    }

    private void OnAccentColorChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count > 0)
        {
            var accentString = ((ComboBoxItem)e.AddedItems[0]).Content;
            if (accentString == null) return;
            ChangeAccentColor((string)accentString);
        }
    }
}
