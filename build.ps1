# QR1000Reader Build Script
# Build and create release package for GitHub Releases

$ErrorActionPreference = "Stop"

Write-Host "=== QR1000Reader Build Script ===" -ForegroundColor Cyan

# 1. Publish framework-dependent single-file EXE
Write-Host "`n[1/3] Building single-file EXE (.NET 8)..." -ForegroundColor Yellow
dotnet publish -c Release -r win-x64 -o publish

if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed!" -ForegroundColor Red
    exit 1
}

# 2. Check EXE file
$exePath = "publish\QR1000Reader.exe"
if (Test-Path $exePath) {
    $exeSize = (Get-Item $exePath).Length / 1MB
    Write-Host "`n[2/3] EXE generated: $exePath" -ForegroundColor Green
    Write-Host ("          Size: {0:N2} MB" -f $exeSize) -ForegroundColor Green
} else {
    Write-Host "EXE file not found!" -ForegroundColor Red
    exit 1
}

# 3. Create Release package
Write-Host "`n[3/3] Creating release package..." -ForegroundColor Yellow
$zipPath = "publish\QR1000Reader.zip"

# Compress EXE and config file
Compress-Archive -Path "publish\QR1000Reader.exe","config.yaml" -DestinationPath $zipPath -Force

$zipSize = (Get-Item $zipPath).Length / 1MB
Write-Host "Package created: $zipPath" -ForegroundColor Green
Write-Host ("          Size: {0:N2} MB" -f $zipSize) -ForegroundColor Green

Write-Host "`n=== Build Complete ===" -ForegroundColor Green
Write-Host "Upload requirements:" -ForegroundColor Cyan
Write-Host "1. .NET 8 Desktop Runtime (x64): https://dotnet.microsoft.com/download/dotnet/8.0" -ForegroundColor White
Write-Host "2. Upload $zipPath to GitHub Releases" -ForegroundColor White

