# QR1000Reader 构建脚本
# 用于编译并发布独立 EXE 到 GitHub Releases

$ErrorActionPreference = "Stop"

Write-Host "=== QR1000Reader Build Script ===" -ForegroundColor Cyan

# 1. 发布独立 EXE
Write-Host "`n[1/3] Building standalone EXE..." -ForegroundColor Yellow
dotnet publish -c Release -r win-x64 -o publish

if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed!" -ForegroundColor Red
    exit 1
}

# 2. 检查 EXE 文件
$exePath = "publish\QR1000Reader.exe"
if (Test-Path $exePath) {
    $exeSize = (Get-Item $exePath).Length / 1MB
    Write-Host "`n[2/3] EXE generated: $exePath" -ForegroundColor Green
    Write-Host ("          Size: {0:N2} MB" -f $exeSize) -ForegroundColor Green
} else {
    Write-Host "EXE file not found!" -ForegroundColor Red
    exit 1
}

# 3. 创建 Release 压缩包
Write-Host "`n[3/3] Creating release package..." -ForegroundColor Yellow
$zipPath = "publish\QR1000Reader.zip"

# 压缩 publish 目录（排除不必要的文件）
Compress-Archive -Path "publish\QR1000Reader.exe","config.yaml" -DestinationPath $zipPath -Force

$zipSize = (Get-Item $zipPath).Length / 1MB
Write-Host "Package created: $zipPath" -ForegroundColor Green
Write-Host ("          Size: {0:N2} MB" -f $zipSize) -ForegroundColor Green

Write-Host "`n=== Build Complete ===" -ForegroundColor Green
Write-Host "Please upload $zipPath to GitHub Releases:" -ForegroundColor Cyan
Write-Host "https://github.com/mlp719176354/QR1000Reader/releases/new" -ForegroundColor White

