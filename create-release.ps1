# GitHub Release 发布脚本
# 自动创建 GitHub Release 并上传 ZIP 包

$ErrorActionPreference = "Stop"

Write-Host "=== GitHub Release 发布脚本 ===" -ForegroundColor Cyan

# 配置
$repoOwner = "mlp719176354"
$repoName = "QR1000Reader"
$zipPath = "publish\QR1000Reader.zip"
$version = "v2026.03.13"
$releaseTag = "v2026.03.13"

# 检查 ZIP 文件是否存在
if (-not (Test-Path $zipPath)) {
    Write-Host "ZIP 文件不存在：$zipPath" -ForegroundColor Red
    Write-Host "请先运行 build.ps1 创建 ZIP 包" -ForegroundColor Yellow
    exit 1
}

# 获取 GitHub Token
Write-Host "`n请输入 GitHub Personal Access Token:" -ForegroundColor Yellow
Write-Host "获取方式：https://github.com/settings/tokens" -ForegroundColor Gray
Write-Host "需要的权限：repo (完整控制)" -ForegroundColor Gray
$token = Read-Host "Token"

# 创建 Authorization Header
$authHeader = @{
    Authorization = "Bearer $token"
    Accept = "application/vnd.github.v3+json"
}

# 创建 Release
Write-Host "`n正在创建 Release $releaseTag..." -ForegroundColor Yellow

$releaseBody = @{
    tag_name = $releaseTag
    name = "QR1000Reader $version"
    body = @"
## QR1000Reader $version

### 更新内容
- ✅ 优化性能：定时器频率从 500ms 降低到 1000ms，减少 CPU 占用
- ✅ 优化 DataGridView 绘制，减少闪烁
- ✅ 优化数据加载流程，移除不必要的刷新
- ✅ 使用 .NET 10 编译，单文件 EXE 仅 10.47 MB

### 系统要求
- Windows 10 x64 或更高版本
- .NET 10 Desktop Runtime x64
- 下载地址：https://dotnet.microsoft.com/download/dotnet/10.0

### 安装说明
1. 下载 QR1000Reader.zip
2. 解压到任意目录
3. 运行 QR1000Reader.exe

### 文件信息
- EXE 大小：10.47 MB
- ZIP 大小：2.98 MB
- 编译时间：$(Get-Date -Format "yyyy-MM-dd HH:mm:ss")
"@
    draft = $false
    prerelease = $false
} | ConvertTo-Json

try {
    $response = Invoke-RestMethod -Uri "https://api.github.com/repos/$repoOwner/$repoName/releases" `
        -Method POST `
        -Headers $authHeader `
        -Body $releaseBody `
        -ContentType "application/json"
    
    $releaseId = $response.id
    $uploadUrl = $response.upload_url -replace "\{.*", ""
    
    Write-Host "Release 创建成功！" -ForegroundColor Green
    
    # 上传 ZIP 文件
    Write-Host "`n正在上传 ZIP 文件..." -ForegroundColor Yellow
    
    $zipBytes = [System.IO.File]::ReadAllBytes($zipPath)
    $zipSize = (Get-Item $zipPath).Length
    
    $uploadUrlWithFilename = "$uploadUrl?name=$(Split-Path $zipPath -Leaf)"
    
    Invoke-RestMethod -Uri $uploadUrlWithFilename `
        -Method POST `
        -Headers $authHeader `
        -Body $zipBytes `
        -ContentType "application/zip"
    
    Write-Host "ZIP 文件上传成功！" -ForegroundColor Green
    
    Write-Host "`n=== 发布完成 ===" -ForegroundColor Green
    Write-Host "Release 地址：" -NoNewline -ForegroundColor Cyan
    Write-Host "https://github.com/$repoOwner/$repoName/releases/tag/$releaseTag" -ForegroundColor White
    
} catch {
    Write-Host "`n错误：$($_.Exception.Message)" -ForegroundColor Red
    exit 1
}
