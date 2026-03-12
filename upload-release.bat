@echo off
echo === GitHub Release Upload ===
echo.
echo Uploading QR1000Reader.zip to GitHub Releases...
echo.

gh release create v2026.03.13 publish\QR1000Reader.zip ^
  --title "QR1000Reader v2026.03.13" ^
  --notes "Performance optimizations and .NET 10 update" ^
  --repo mlp719176354/QR1000Reader

if %errorlevel% equ 0 (
    echo.
    echo === Success ===
    echo Release created at:
    echo https://github.com/mlp719176354/QR1000Reader/releases/tag/v2026.03.13
) else (
    echo.
    echo === Error ===
    echo Failed to create release.
    echo Please make sure you are logged in with: gh auth login
)

pause
