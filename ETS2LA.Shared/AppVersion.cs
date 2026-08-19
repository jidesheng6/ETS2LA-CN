using System.Reflection;

namespace ETS2LA.Shared;

/// <summary>
/// 统一解析应用版本号。优先 InformationalVersion（含 fork 后缀如 3.4.33-zh.4），
/// 再回退 AssemblyVersion / Velopack CurrentVersion。
/// </summary>
public static class AppVersion
{
    public static string GetDisplayVersion(bool withPrefix = true)
    {
        string version = ResolveRawVersion();
        if (string.IsNullOrWhiteSpace(version) || version == "0.0.0")
            version = "未知";

        return withPrefix && version != "未知" && !version.StartsWith('v')
            ? $"v{version}"
            : version;
    }

    /// <summary>用于插件兼容性比较的主版本号（major.minor.patch）。</summary>
    public static string GetCompatibilityVersion()
    {
        var asm = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
        return asm.GetName().Version?.ToString(3) ?? "0.0.0";
    }

    public static string ResolveRawVersion()
    {
        var asm = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();

        var informational = asm
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            ?.InformationalVersion;

        if (!string.IsNullOrWhiteSpace(informational))
        {
            // 去掉 SourceLink 附加的 "+commit"
            int plus = informational.IndexOf('+');
            return plus >= 0 ? informational[..plus] : informational;
        }

        return asm.GetName().Version?.ToString(3) ?? "0.0.0";
    }
}
