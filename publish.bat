@echo off
setlocal EnableExtensions
cd /d "%~dp0"

set "BACKUP=%TEMP%\ets2la-cn-plugin-backup"
if exist "publish\Plugins" (
  echo 备份 publish\Plugins ...
  robocopy "publish\Plugins" "%BACKUP%\Plugins" /E /NFL /NDL /NJH /NJS /nc /ns /np
)
if exist "publish\Libraries" (
  echo 备份 publish\Libraries ...
  robocopy "publish\Libraries" "%BACKUP%\Libraries" /E /NFL /NDL /NJH /NJS /nc /ns /np
)

dotnet build ETS2LA.sln -c Release --no-incremental
if errorlevel 1 exit /b 1
dotnet publish ETS2LA/ETS2LA.csproj --self-contained -o .\publish
if errorlevel 1 exit /b 1
xcopy /E /I /Y .\Assets .\publish\Assets

if exist "%BACKUP%\Plugins" (
  echo 还原 publish\Plugins ...
  robocopy "%BACKUP%\Plugins" "publish\Plugins" /E /NFL /NDL /NJH /NJS /nc /ns /np
)
if exist "%BACKUP%\Libraries" (
  echo 还原 publish\Libraries ...
  robocopy "%BACKUP%\Libraries" "publish\Libraries" /E /NFL /NDL /NJH /NJS /nc /ns /np
)

echo 发布完成: %CD%\publish\ETS2LA.exe
endlocal
