using ETS2LA.Shared;
using Xunit;

namespace ETS2LA.UI.Tests;

public class AppVersionTests
{
    [Fact]
    public void GetDisplayVersion_IsNotEmptyOrNullLiteral()
    {
        var version = AppVersion.GetDisplayVersion();
        Assert.False(string.IsNullOrWhiteSpace(version));
        Assert.DoesNotContain("null", version, StringComparison.OrdinalIgnoreCase);
        Assert.NotEqual("v", version);
    }
}
