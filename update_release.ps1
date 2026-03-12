# QR1000Reader Release Script
# 使用环境变量 GITHUB_TOKEN 或在运行时输入 token

$token = $env:GITHUB_TOKEN
if (-not $token) {
    $token = Read-Host "请输入 GitHub Token"
}

$headers = @{
    Authorization = "token $token"
    Accept = 'application/vnd.github.v3+json'
}

try {
    $existingRelease = Invoke-RestMethod -Uri 'https://api.github.com/repos/mlp719176354/QR1000Reader/releases/tags/v2026.03.12' -Method Get -Headers $headers
    Invoke-RestMethod -Uri "https://api.github.com/repos/mlp719176354/QR1000Reader/releases/$($existingRelease.id)" -Method Delete -Headers $headers
    Write-Host 'Deleted existing release'
} catch {
    Write-Host 'No existing release found'
}

$releaseBody = 'QR1000Reader Release - includes exe and config.yaml'
$body = @{
    tag_name = 'v2026.03.12'
    name = 'v2026.03.12'
    body = $releaseBody
    draft = $false
    prerelease = $false
} | ConvertTo-Json

$release = Invoke-RestMethod -Uri 'https://api.github.com/repos/mlp719176354/QR1000Reader/releases' -Method Post -Headers $headers -Body $body
Write-Host 'Release created'

$uploadUrl = $release.upload_url -replace '\{.*$', '?name=Release.zip'
$filePath = 'publish\Release.zip'
$fileBytes = [System.IO.File]::ReadAllBytes($filePath)
$contentType = 'application/zip'
Invoke-RestMethod -Uri $uploadUrl -Method Post -Headers $headers -ContentType $contentType -Body $fileBytes
Write-Host 'Uploaded Release.zip'
Write-Host ('Release URL: ' + $release.html_url)
