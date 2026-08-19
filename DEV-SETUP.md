# 开发环境与 Agent 工具配置

本文档记录 ETS2LA-CN 项目的编译方式、MCP 服务、Skills 及工具路径，供 Agent 与开发者参考。

## 一、编译方式

### 前置条件

| 依赖 | 版本 | 说明 |
|------|------|------|
| .NET SDK | 10.0+ | https://dotnet.microsoft.com/download/dotnet/10.0 |
| Git | 任意较新版本 | clone 时需 `--recurse-submodules` 拉取 TruckLib |

### 克隆仓库

```powershell
git clone --recurse-submodules -b zh-optimized-3.4.33 https://github.com/QuiYe666/ETS2LA-CN.git
cd ETS2LA-CN
```

### 编译命令

```powershell
# Release 构建
dotnet build ETS2LA.sln -c Release --no-incremental

# 完整自包含发布（推荐）
.\publish.bat
```

### 发布脚本说明（publish.bat）

1. `dotnet build ETS2LA.sln -c Release --no-incremental`
2. `dotnet publish ETS2LA/ETS2LA.csproj --self-contained -o .\publish`
3. `xcopy /E /I /Y .\Assets .\publish\Assets`

产物：`publish\ETS2LA.exe`（约 274 MB，含运行时）

### IDE 调试

- 打开 `ETS2LA.sln`
- 安装 C# Dev Kit 扩展
- 按 **F5** 启动

---

## 二、MCP 服务

### 配置文件

| 级别 | 路径 |
|------|------|
| 全局 | `C:\Users\pw\.cursor\mcp.json` |
| 项目 | `d:\Remove\ets2la-cn\.cursor\mcp.json` |

### agentmemory（持久记忆）

| 项 | 路径 / 地址 |
|----|-------------|
| MCP 命令 | `D:\Tools\npm-global\agentmemory-mcp.cmd` |
| 服务启动脚本 | `D:\Tools\start-agentmemory.ps1` |
| 数据目录 | `D:\Tools\agentmemory-data` |
| iii-engine | `D:\Tools\iii\iii.exe` |
| 健康检查 | http://localhost:3111/agentmemory/health |
| 可视化界面 | http://localhost:3113 |

**启动方式：**

```powershell
D:\Tools\start-agentmemory.ps1
# 或
D:\Tools\npm-global\agentmemory.cmd --data-dir D:\Tools\agentmemory-data
```

### codedb-mcp（代码库索引）

| 项 | 路径 |
|----|------|
| 可执行文件 | `D:\Tools\codedb-mcp\skills\codedb-mcp\assets\codebase-mcp.exe` |
| 仓库包根目录 | `D:\Tools\codedb-mcp` |
| 项目配置 | `d:\Remove\ets2la-cn\.codedb-mcp\codedb-mcp.toml` |
| 索引数据 | `d:\Remove\ets2la-cn\.codedb-mcp\` |

**索引命令：**

```powershell
D:\Tools\codedb-mcp\skills\codedb-mcp\assets\codebase-mcp.exe `
  --config d:\Remove\ets2la-cn\.codedb-mcp\codedb-mcp.toml `
  index d:\Remove\ets2la-cn
```

**状态检查：**

```powershell
D:\Tools\codedb-mcp\skills\codedb-mcp\assets\codebase-mcp.exe `
  --config d:\Remove\ets2la-cn\.codedb-mcp\codedb-mcp.toml `
  --root d:\Remove\ets2la-cn tool codedb_status '{}'
```

### MCP 一键检测脚本

```powershell
.\scripts\ensure-mcp.ps1
```

---

## 三、全局 Skills

安装目录：`C:\Users\pw\.cursor\skills\`

| Skill | 用途 |
|-------|------|
| `aspnet-core` | C# / ASP.NET 开发 |
| `codedb-mcp` | 代码库 MCP 工具使用 |
| `deepwiki` | 本地 DeepWiki 文档生成 |
| `code-module-atlas` | 模块依赖可视化 |
| `remember` / `recall` / `handoff` | agentmemory 记忆管理 |
| `reverse-skills` | IDA 逆向分析插件集 |
| `reverse-skill` | 逆向/渗透技能路由包 |
| `gh-fix-ci` | CI 修复 |
| `gh-address-comments` | PR 评论处理 |
| `security-best-practices` | 安全最佳实践 |
| `security-threat-model` | 威胁建模 |
| `playwright` | E2E 测试 |
| `pdf` / `jupyter-notebook` | 文档与 Notebook |

---

## 四、其他工具

| 工具 | 路径 |
|------|------|
| GitHub CLI | `D:\Tools\gh\gh.exe` |
| Node.js | `D:\Program Files\nodejs\` |
| npm 全局包 | `D:\Tools\npm-global\` |

---

## 五、Cursor 规则

项目规则位于 `.cursor/rules/`，全部标记为**强制执行**：

- `00-mandatory-agent.mdc` — 中文回复、MCP/Skills 使用、提交规范
- `01-build-and-dev.mdc` — 编译与发布流程

---

## 六、代理（可选）

如需通过代理访问 GitHub / npm：

```powershell
$env:HTTP_PROXY = "http://127.0.0.1:7890"
$env:HTTPS_PROXY = "http://127.0.0.1:7890"
```
