![](Assets/markdown_logo.png)

# ETS2LA 中文优化版（非官方）

基于 [ETS2LA](https://github.com/ETS2LA/ETS2LA) `3.4.33` 的非官方中文与驾驶优化衍生版本。

## 特性

- 中文界面与本地化体验改进
- 保留 ETS2LA 的 GPL-3.0 许可证与原有版权声明
- 完整改动记录见 [CHANGELOG.md](CHANGELOG.md)

## 下载与安装

### 优化 DLL（可选）

- 链接：https://cnsa.lanzoub.com/b0mcqp2ad
- 密码：`45ub`

> 该优化 DLL 不在源码仓库内，需手动下载。

### 官方渠道

- [ETS2LA 官方下载](https://github.com/ETS2LA/ETS2LA/releases/latest)
- [Discord 社区](https://ets2la.com/discord)
- [官方网站](https://ets2la.com)
- [开发者文档](https://docs.ets2la.com)

## 从源码编译

### 环境要求

| 依赖 | 说明 |
|------|------|
| .NET SDK 10.0+ | [下载地址](https://dotnet.microsoft.com/download/dotnet/10.0) |
| Git | clone 时使用 `--recurse-submodules` |

### 克隆

```powershell
git clone --recurse-submodules -b zh-optimized-3.4.33 https://github.com/QuiYe666/ETS2LA-CN.git
cd ETS2LA-CN
```

### 编译

```powershell
# 方式一：一键发布（推荐）
.\publish.bat

# 方式二：手动分步
dotnet build ETS2LA.sln -c Release --no-incremental
dotnet publish ETS2LA/ETS2LA.csproj --self-contained -o .\publish
xcopy /E /I /Y .\Assets .\publish\Assets
```

编译产物：`publish\ETS2LA.exe`

### IDE 调试

1. 用 VS Code 或 Visual Studio 打开 `ETS2LA.sln`
2. 安装 **C# Dev Kit** 扩展
3. 按 **F5** 运行

## 常见问题

遇到问题请先查看 [FAQ](https://ets2la.com/faq)，或加入 [Discord](https://ets2la.com/discord) 支持频道。

## 开发环境（Agent / MCP）

Agent 开发工具链、MCP 服务、Skills 路径等详见 [DEV-SETUP.md](DEV-SETUP.md)。

## 致谢

- [SCS Software](https://scssoft.com) — Euro Truck Simulator 2 / American Truck Simulator
- [TruckLib](https://github.com/sk-zk/TruckLib) — 游戏文件提取
- [ts-map](https://github.com/dariowouters/ts-map) 与 [maps](https://github.com/truckermudgeon/maps) — 地图解析参考

## 许可证

GPL-3.0，详见 [LICENSE.txt](LICENSE.txt)。
