# 创建带时间戳的 ZIP 文件
$timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
$zipName = "QR1000Reader_$timestamp.zip"

Write-Host "Creating ZIP file: $zipName" -ForegroundColor Cyan

Compress-Archive -Path "publish\QR1000Reader.exe","config.yaml" -DestinationPath "publish\$zipName" -Force

Write-Host "ZIP file created: publish\$zipName" -ForegroundColor Green
Write-Host "File size: $((Get-Item "publish\$zipName").Length / 1MB) MB" -ForegroundColor Green

# 输出文件名供批处理使用
$zipName | Out-File -FilePath "publish\latest_zip_name.txt" -Encoding UTF8
