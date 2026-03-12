# QR1000Reader - 护照阅读器

**版本**: v2026.03.12  
**发布日期**: 2026 年 3 月 13 日

---

## 📥 下载

### GitHub Release
访问：https://github.com/mlp719176354/QR1000Reader/releases/tag/v2026.03.12

下载文件：`QR1000Reader_20260313_013027.zip` (约 3 MB)

---

## 🚀 快速开始

### 1. 安装 .NET 10 Desktop Runtime
下载地址：https://dotnet.microsoft.com/download/dotnet/10.0
- 选择 "Desktop Runtime" - Windows x64

### 2. 解压并运行
1. 解压下载的 ZIP 文件
2. 运行 `QR1000Reader.exe`

---

## 📁 项目结构

```
QR1000Reader/
├── Form1.cs              # 主窗体代码
├── Form1.Designer.cs     # 窗体设计器代码
├── Program.cs            # 程序入口
├── ConfigHelper.cs       # 配置帮助类
├── DatabaseHelper.cs     # 数据库帮助类
├── ExcelHelper.cs        # Excel 导出帮助类
├── MrzParser.cs          # MRZ 解析器
├── SinosecuWebSocketClient.cs  # WebSocket 客户端
├── config.yaml           # 配置文件
├── README.md             # 项目说明
├── .gitignore            # Git 忽略文件
└── Docs/                 # 文档目录
    └── PUBLISH.md        # 发布文档
```

**注意**: 构建脚本和发布文件在本地保存，未提交到 Git。

---

## 🔧 开发

### 构建项目
```bash
dotnet build
```

### 发布
```bash
# 创建发布文件
dotnet publish -c Release -r win-x64 -o publish
```

**注意**: 构建脚本和发布文件保存在本地，未提交到 Git。

---

## 📝 功能

- ✅ 自动读取护照/证件信息
- ✅ WebSocket 连接设备
- ✅ 自动填充旅客信息
- ✅ 保存记录到数据库
- ✅ 导出 Excel
- ✅ 表单验证（出发港/到达港/时间必填）
- ✅ 重复记录检查

---

## 📊 系统要求

| 项目 | 要求 |
|------|------|
| 操作系统 | Windows 10 x64 或更高版本 |
| 运行时 | .NET 10 Desktop Runtime |
| 内存 | 建议 4GB 以上 |
| 硬盘 | 建议 SSD |

---

## 📖 文档

- [发布文档](Docs/PUBLISH.md) - 发布流程和使用说明

---

## 🔐 安全

- ✅ 不包含任何敏感的 Token
- ✅ 数据库文件已加入 .gitignore
- ✅ 编译输出已排除

---

## 📄 许可证

Copyright © 2026 Johnny Chan

---

## 📞 支持

GitHub Issues: https://github.com/mlp719176354/QR1000Reader/issues
