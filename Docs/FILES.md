# QR1000Reader 文件清单

**更新日期**: 2026 年 3 月 13 日

---

## 📦 GitHub 仓库文件（15 个）

### 源代码文件（9 个）
- `Form1.cs` - 主窗体代码
- `Form1.Designer.cs` - 窗体设计器代码
- `Program.cs` - 程序入口
- `ConfigHelper.cs` - 配置帮助类
- `DatabaseHelper.cs` - 数据库帮助类
- `ExcelHelper.cs` - Excel 导出帮助类
- `MrzParser.cs` - MRZ 解析器
- `SinosecuWebSocketClient.cs` - WebSocket 客户端

### 配置文件（3 个）
- `QR1000Reader.csproj` - 项目文件
- `QR1000Reader.sln` - 解决方案文件
- `config.yaml` - 配置文件

### 文档文件（2 个）
- `README.md` - 项目说明
- `Docs/PUBLISH.md` - 发布文档

### Git 配置（1 个）
- `.gitignore` - Git 忽略文件
- `.gitattributes` - Git 属性文件

---

## 💻 本地文件（不上传到 GitHub）

### 构建脚本
- `build.ps1` - 构建脚本
- `create-zip.ps1` - 创建 ZIP 脚本
- `update_release.ps1` - 更新发布脚本

### 发布目录（publish/）
- `publish/QR1000Reader.exe` - 可执行文件
- `publish/config.yaml` - 配置文件
- `publish/QR1000Reader_*.zip` - 发布的 ZIP 包

### 编译输出（不上传）
- `bin/` - 编译输出目录
- `obj/` - 编译临时目录

### 备份目录（不上传）
- `Backup_*/` - 备份目录

### 数据库文件（不上传）
- `*.db` - 数据库文件

### 其他本地文件
- `RELEASE_NOTES.md` - 发布说明
- `Docs/CLEANUP_SUMMARY.md` - 清理总结
- `Demo/Sinosecu.html` - 演示文件

---

## 🔒 .gitignore 规则

以下文件类型被排除在 Git 之外：

```
# 编译输出
bin/
obj/
*.pdb

# 发布目录
publish/

# 脚本文件
*.ps1
*.bat
*.cmd

# 数据库
*.db

# 备份
Backup_*/

# 系统文件
.DS_Store
Thumbs.db

# 安全
*.token
*.secret
```

---

## 📊 统计

| 类别 | GitHub 文件数 | 本地文件数 |
|------|-------------|-----------|
| 源代码 | 9 | 0 |
| 配置文件 | 3 | 1 |
| 文档 | 2 | 3 |
| 脚本 | 0 | 3 |
| 发布文件 | 0 | 3+ |
| **总计** | **15** | **10+** |

---

## ⚠️ 重要提示

1. **脚本文件**：所有 `.ps1` 和 `.bat` 文件只在本地保存
2. **发布文件**：`publish/` 目录不上传到 Git
3. **数据库**：`.db` 文件不上传到 Git
4. **Token**：任何包含 Token 的文件都不上传

---

**本地文件请妥善保管，定期备份！**
