# QR1000Reader 发布文档

**发布日期**: 2026 年 3 月 13 日

---

## 📦 发布文件

### 压缩文件位置
```
publish\QR1000Reader_20260313_013027.zip
```

**文件大小**: 2.98 MB  
**包含内容**:
- QR1000Reader.exe (10.47 MB)
- config.yaml (1.6 KB)

### GitHub Release
**地址**: https://github.com/mlp719176354/QR1000Reader/releases/tag/v2026.03.12

**下载链接**:
```
https://github.com/mlp719176354/QR1000Reader/releases/download/v2026.03.12/QR1000Reader_20260313_013027.zip
```

---

## 🚀 使用方法

### 系统要求
- Windows 10 x64 或更高版本
- .NET 10 Desktop Runtime x64

### 安装步骤
1. 下载 .NET 10 Desktop Runtime
   - 地址：https://dotnet.microsoft.com/download/dotnet/10.0
   - 选择 "Desktop Runtime" - Windows x64

2. 下载并解压 QR1000Reader.zip

3. 运行 QR1000Reader.exe

---

## 📝 更新日志

### v2026.03.13 (2026-03-13)
- ✅ 优化定时器频率（500ms → 1000ms），减少 CPU 占用
- ✅ 优化 DataGridView 绘制，减少界面闪烁
- ✅ 优化数据加载流程
- ✅ 使用 .NET 10 编译，单文件 EXE 仅 10.47 MB

### v2026.03.12 (2026-03-12)
- ✅ 添加出发港/到达港/时间必填验证
- ✅ 添加重复记录检查
- ✅ 优化 EXE 体积

---

## 🔧 构建和发布脚本

### 1. 创建 ZIP 文件
```powershell
# 创建带时间戳的 ZIP 文件
powershell -ExecutionPolicy Bypass -File create-zip.ps1
```

**输出**: `publish\QR1000Reader_yyyyMMdd_HHmmss.zip`

### 2. 上传到 GitHub Release
```powershell
# 使用 Personal Access Token 上传
powershell -ExecutionPolicy Bypass -File upload.ps1
```

**需要**: GitHub Personal Access Token（repo 权限）

### 3. 完整构建脚本
```powershell
# 构建并创建 ZIP
powershell -ExecutionPolicy Bypass -File build.ps1

# 上传到 GitHub Release
powershell -ExecutionPolicy Bypass -File upload.ps1
```

---

## 📁 文件说明

### 源代码文件
| 文件 | 说明 |
|------|------|
| Form1.cs | 主窗体代码 |
| Form1.Designer.cs | 窗体设计器代码 |
| Program.cs | 程序入口 |
| ConfigHelper.cs | 配置帮助类 |
| DatabaseHelper.cs | 数据库帮助类 |
| ExcelHelper.cs | Excel 导出帮助类 |
| MrzParser.cs | MRZ 解析器 |
| SinosecuWebSocketClient.cs | WebSocket 客户端 |
| config.yaml | 配置文件 |
| QR1000Reader.csproj | 项目文件 |

### 脚本文件
| 文件 | 说明 |
|------|------|
| build.ps1 | 构建脚本 |
| create-zip.ps1 | 创建 ZIP 文件脚本 |
| upload.ps1 | 上传到 GitHub Release 脚本 |

### 输出目录
| 目录 | 说明 |
|------|------|
| publish/ | 发布文件输出目录 |
| bin/ | 编译输出（可删除） |
| obj/ | 编译临时文件（可删除） |

---

## ⚠️ 安全提示

**不要提交到 Git 的文件**:
- `.gitattributes` 中的 LFS 配置
- 包含 Personal Access Token 的文件
- 数据库文件 (*.db)
- 编译输出 (bin/, obj/)

**Token 安全**:
- Personal Access Token 应保存在安全位置
- 不要将 token 提交到 Git
- 定期更新 token

---

## 📊 版本信息

| 项目 | 值 |
|------|-----|
| 当前版本 | v2026.03.12 |
| .NET 版本 | .NET 10 |
| EXE 大小 | 10.47 MB |
| ZIP 大小 | ~3 MB |
| 最低系统 | Windows 10 x64 |

---

**作者**: Johnny Chan  
**GitHub**: https://github.com/mlp719176354/QR1000Reader  
**版权**: Copyright © 2026
