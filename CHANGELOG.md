# 变更记录

## 3.4.33-zh.3 - 2026-08-19

- 建立 `ETS2LA.UI/Localization/UiStrings.cs` 集中管理 UI 字符串与枚举映射。
- 翻译 onboarding 教程（OnboardingPart1/2）为中文。
- 补齐 Program.cs、Utils.cs、Updates、AssistanceSettings 等剩余英文字符串。
- 扩展插件目录中文化映射（UiStrings.ChinesePlugins）。
- 优化 Vision 渲染：线程锁、顶点 buffer 复用、修正数组长度。
- 新增 `ETS2LA.UI.Tests` 单元测试（UiStrings 映射）。
- 规则与 DEV-SETUP 补充 Git remote、代理配置说明。
- 注：Avalonia.ReactiveUI 暂无 12.x 版本，保持 11.3.9 与 Avalonia 12.0.2 并存。

## 3.4.33-zh.2 - 2026-08-19

- 汉化 README，补充完整编译与发布说明。
- 新增 `DEV-SETUP.md`：记录 MCP、Skills、工具路径与启动方式。
- 新增 Cursor 规则（`.cursor/rules/`）与 MCP 检测脚本（`scripts/ensure-mcp.ps1`）。
- 同步版本号为 `3.4.33-zh.2`。
- 补齐部分 UI 英文字符串为中文。

## 3.4.33-zh.1 - 2026-08-11

- 基于 ETS2LA `3.4.33`（`7ccc341`）建立非官方中文优化分支。
- 补充主程序界面中文化和相关本地化支持。
- 不包含 ETS2/ATS 游戏资源、用户数据、构建产物或独立驾驶插件二进制文件。
