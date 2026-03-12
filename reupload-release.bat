@echo off
chcp 65001 >nul
echo === 删除旧版本并上传新版本 ===
echo.
echo 目标 Release: v2026.03.12
echo.

:: 获取最新的 ZIP 文件名
for /f "delims=" %%i in ('dir /b /o-d publish\QR1000Reader_*.zip 2^>nul ^| findstr /r "\.zip$"') do (
    set "ZIPFILE=%%i"
    goto :found
)

:found
if "%ZIPFILE%"=="" (
    echo 错误：找不到 ZIP 文件
    echo 请先运行 create-zip.ps1 创建 ZIP 文件
    pause
    exit /b 1
)

echo 使用文件：%ZIPFILE%
echo.

:: 删除旧版本
echo [1/2] 删除旧版本 v2026.03.12...
gh release delete v2026.03.12 --repo mlp719176354/QR1000Reader --yes

if %errorlevel% neq 0 (
    echo.
    echo 警告：删除失败，可能版本不存在
    echo.
)

:: 创建新版本并上传
echo [2/2] 创建新版本并上传...
gh release create v2026.03.12 "publish\%ZIPFILE%" --repo mlp719176354/QR1000Reader --title "QR1000Reader v2026.03.12" --notes "Performance optimized version"

if %errorlevel% equ 0 (
    echo.
    echo === 上传成功 ===
    echo.
    echo 查看 Release: https://github.com/mlp719176354/QR1000Reader/releases/tag/v2026.03.12
) else (
    echo.
    echo === 上传失败 ===
    echo.
    echo 请先运行：gh auth login
    echo 或者安装 GitHub CLI: winget install GitHub.cli
)

pause
