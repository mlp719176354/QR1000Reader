# GitHub Release Creator - Simple Version
$ErrorActionPreference = "Stop"

$repoOwner = "mlp719176354"
$repoName = "QR1000Reader"
$zipPath = "publish\QR1000Reader.zip"
$releaseTag = "v2026.03.13"
$releaseName = "QR1000Reader v2026.03.13"

Write-Host "=== GitHub Release Creator ===" -ForegroundColor Cyan

if (-not (Test-Path $zipPath)) {
    Write-Host "ZIP file not found: $zipPath" -ForegroundColor Red
    exit 1
}

Write-Host "`nEnter GitHub Personal Access Token:" -ForegroundColor Yellow
Write-Host "Get token: https://github.com/settings/tokens" -ForegroundColor Gray
Write-Host "Required scope: repo" -ForegroundColor Gray
$token = Read-Host "Token"

$authHeader = @{
    Authorization = "Bearer $token"
    Accept = "application/vnd.github.v3+json"
}

$releaseBody = @{
    tag_name = $releaseTag
    name = $releaseName
    body = "QR1000Reader v2026.03.13 Release`n`nPerformance optimizations and .NET 10 update."
    draft = $false
    prerelease = $false
} | ConvertTo-Json

try {
    Write-Host "`nCreating release..." -ForegroundColor Yellow
    
    $response = Invoke-RestMethod -Uri "https://api.github.com/repos/$repoOwner/$repoName/releases" `
        -Method POST `
        -Headers $authHeader `
        -Body $releaseBody `
        -ContentType "application/json"
    
    $releaseId = $response.id
    $uploadUrl = $response.upload_url -replace "\{.*", ""
    
    Write-Host "Release created!" -ForegroundColor Green
    
    Write-Host "`nUploading ZIP file..." -ForegroundColor Yellow
    
    $zipBytes = [System.IO.File]::ReadAllBytes($zipPath)
    $fileName = Split-Path $zipPath -Leaf
    $uploadUrlWithFilename = "$uploadUrl?name=$fileName"
    
    Invoke-RestMethod -Uri $uploadUrlWithFilename `
        -Method POST `
        -Headers $authHeader `
        -Body $zipBytes `
        -ContentType "application/zip"
    
    Write-Host "ZIP file uploaded!" -ForegroundColor Green
    
    Write-Host "`n=== Complete ===" -ForegroundColor Green
    Write-Host "Release URL: https://github.com/$repoOwner/$repoName/releases/tag/$releaseTag" -ForegroundColor Cyan
    
} catch {
    Write-Host "`nError: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}
