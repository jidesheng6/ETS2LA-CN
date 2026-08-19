using ETS2LA.UI.Localization;
using Xunit;

namespace ETS2LA.UI.Tests;

public class UiStringsTests
{
    [Theory]
    [InlineData("Light", "浅色")]
    [InlineData("Dark", "深色")]
    [InlineData("System", "跟随系统")]
    public void ThemeToDisplay_MapsEnglishToChinese(string input, string expected)
    {
        Assert.Equal(expected, UiStrings.ThemeToDisplay(input));
    }

    [Theory]
    [InlineData("浅色", "Light")]
    [InlineData("深色", "Dark")]
    [InlineData("跟随系统", "System")]
    public void ThemeToStorage_MapsChineseToEnglish(string input, string expected)
    {
        Assert.Equal(expected, UiStrings.ThemeToStorage(input));
    }

    [Fact]
    public void ChinesePlugins_ContainsCorePlugins()
    {
        Assert.True(UiStrings.ChinesePlugins.ContainsKey("tumppi066.adaptivecruisecontrol"));
        Assert.True(UiStrings.ChinesePlugins.ContainsKey("tumppi066.laneassist"));
    }

    [Theory]
    [InlineData("Low", "低")]
    [InlineData("MatchGame", "匹配游戏")]
    [InlineData("Metric", "公制")]
    public void FormatEnumOption_MapsKnownValues(string input, string expected)
    {
        Assert.Equal(expected, UiStrings.FormatEnumOption(input));
    }
}
