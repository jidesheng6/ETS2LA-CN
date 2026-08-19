# 检测并启动本项目所需的 MCP 服务
# 用法: .\scripts\ensure-mcp.ps1

$ErrorActionPreference = "Continue"
$repoRoot = Split-Path $PSScriptRoot -Parent

Write-Host "=== MCP 服务检测 ($repoRoot) ===" -ForegroundColor Cyan

# --- agentmemory ---
$amHealth = "http://localhost:3111/agentmemory/health"
$amOk = $false
try {
    $resp = Invoke-RestMethod -Uri $amHealth -TimeoutSec 5
    if ($resp.status -eq "healthy") { $amOk = $true }
} catch {}

if (-not $amOk) {
    Write-Host "[agentmemory] 未运行，正在启动..." -ForegroundColor Yellow
    $amScript = "D:\Tools\start-agentmemory.ps1"
    if (Test-Path $amScript) {
        Start-Process -WindowStyle Hidden -FilePath "powershell.exe" -ArgumentList "-NoProfile","-ExecutionPolicy","Bypass","-File",$amScript
    } else {
        Start-Process -WindowStyle Hidden -FilePath "D:\Tools\npm-global\agentmemory.cmd" -ArgumentList "--data-dir","D:\Tools\agentmemory-data"
    }
    Start-Sleep -Seconds 10
    try {
        $resp = Invoke-RestMethod -Uri $amHealth -TimeoutSec 10
        if ($resp.status -eq "healthy") { $amOk = $true }
    } catch {}
}

if ($amOk) {
    Write-Host "[agentmemory] OK - http://localhost:3111" -ForegroundColor Green
} else {
    Write-Host "[agentmemory] 启动失败，请手动运行: D:\Tools\start-agentmemory.ps1" -ForegroundColor Red
}

# --- codedb-mcp index ---
$codedbConfig = Join-Path $repoRoot ".codedb-mcp\codedb-mcp.toml"
$codedbExe = "D:\Tools\codedb-mcp\skills\codedb-mcp\assets\codebase-mcp.exe"
$codedbOk = $false

if ((Test-Path $codedbExe) -and (Test-Path $codedbConfig)) {
    $status = & $codedbExe --config $codedbConfig --root $repoRoot tool codedb_status '{}' 2>&1
    if ($LASTEXITCODE -eq 0) {
        $codedbOk = $true
        Write-Host "[codedb-mcp] OK - 索引就绪" -ForegroundColor Green
        $status | Select-String "files:|graph:" | ForEach-Object { Write-Host "  $_" }
    }
}

if (-not $codedbOk) {
    Write-Host "[codedb-mcp] 索引缺失或 exe 不可用，正在重建索引..." -ForegroundColor Yellow
    if (Test-Path $codedbExe) {
        $template = "D:\Tools\codedb-mcp\skills\codedb-mcp\assets\codedb-mcp.toml.template"
        $codedbDir = Join-Path $repoRoot ".codedb-mcp"
        New-Item -ItemType Directory -Force -Path $codedbDir | Out-Null
        $config = Join-Path $codedbDir "codedb-mcp.toml"
        if (-not (Test-Path $config) -and (Test-Path $template)) {
            Copy-Item $template $config
        }
        & $codedbExe --config $config index $repoRoot
        if ($LASTEXITCODE -eq 0) {
            Write-Host "[codedb-mcp] 索引完成" -ForegroundColor Green
            $codedbOk = $true
        }
    } else {
        Write-Host "[codedb-mcp] 未找到 $codedbExe" -ForegroundColor Red
    }
}

Write-Host "=== 检测完成 ===" -ForegroundColor Cyan
if (-not $amOk -or -not $codedbOk) { exit 1 }
exit 0
