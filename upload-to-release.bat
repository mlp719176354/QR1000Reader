@echo off
chcp 65001 >nul
echo === 上传到 GitHub Release v2026.03.12 ===
echo.
echo 文件：publish\QR1000Reader.zip
echo 目标：https://github.com/mlp719176354/QR1000Reader/releases/tag/v2026.03.12
echo.

gh release upload v2026.03.12 publish\QR1000Reader.zip --repo mlp719176354/QR1000Reader --clobber

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
