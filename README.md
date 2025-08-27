# NoteGenFontAttacker

一个自动化工具，用于在 **NoteGen** 启动时注入自定义字体样式。支持全局字体修改，同时单独替换代码字体。启动时自动打开 DevTools 注入 CSS 并关闭 DevTools，无需手动操作。
## 功能特点

* 自动启动 NoteGen 客户端
* 注入自定义全局字体（例如 `"HarmonyOS Sans SC"`，字体文件需自行准备或修改为其他已安装字体）
* 单独替换 `<span data-type="code">` 字体为 `"JetBrains Maple Mono"`
* 自动打开 DevTools 注入 CSS
* 注入完成后自动关闭 DevTools
* 完全自动化，无需手动 F12

---
## 预览
- Before
  <img width="1360" height="812" alt="image" src="https://github.com/user-attachments/assets/28dabda1-b45f-470e-913d-e1c9b750d076" />

- After
  <img width="1360" height="812" alt="image" src="https://github.com/user-attachments/assets/17221073-c66e-462b-8898-72903610971b" />

## 使用说明

### 1. 前置条件

* Windows 系统
* .NET 8 或更高版本（带 Windows Forms 支持）

### 2. 配置

1. 修改 `Program.cs` 中的客户端路径：

```csharp
string exePath = @"D:\02Applications\NoteGen\note-gen.exe";
```

2. 如需修改字体，调整：

```csharp
style.innerHTML='*{font-family:"HarmonyOS Sans SC",sans-serif!important;}span[data-type="code"] { font-family: "JetBrains Mono", monospace !important; }';
```

---

### 3. 编译运行

1. 使用 Visual Studio 打开项目，确保 `csproj` 已启用 Windows Forms：

```xml
<TargetFramework>net8.0-windows</TargetFramework>
<UseWindowsForms>true</UseWindowsForms>
```

2. 编译并运行，工具会自动启动 Note-Gen 并注入 CSS。

---

### 4. 注意事项

* 工具通过模拟键盘操作打开/关闭 DevTools，因此客户端窗口需保持前台。
* 注入 CSS 时，代码块字体和全局字体会立即生效。
* 若客户端更新或快捷键变更，可能需要调整脚本的按键操作或延迟时间。

---

## 免责声明 (Disclaimer)

NoteGen Font Injector 仅用于个人学习、测试和界面美化目的。使用本工具产生的任何问题，包括但不限于软件崩溃、数据丢失、账号封禁或安全风险，开发者 概不负责。

请仅在 你自己的设备和客户端副本 上使用本工具。不要将其用于侵犯他人权益、绕过软件安全机制或用于商业用途。

使用本工具即表示你已经阅读并同意上述条款。
