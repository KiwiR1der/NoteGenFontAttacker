using System.Diagnostics;
using System.Runtime.InteropServices;

namespace NoteGenFontInjector
{
    internal class Program
    {
        // Wind32 API 
        [DllImport("user32.dll")] static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")] static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        const int SW_RESTORE = 9;

        [STAThread]
        static void Main(string[] args)
        {
            // 修改为你的 note-gen.exe 路径
            string exePath = @"D:\02Applications\NoteGen\note-gen.exe";

            // 启动进程
            var psi = new ProcessStartInfo(exePath)
            {
                UseShellExecute = true,
                WorkingDirectory = System.IO.Path.GetDirectoryName(exePath)!
            };
            var p = Process.Start(psi);
            if (p == null)
            {
                Console.WriteLine("无法启动进程。请检查路径。");
                return;
            }

            // 等待主窗口就绪
            Console.WriteLine("等待窗口");
            for (int i = 0; i < 60; i++)
            {
                p.Refresh();
                if (p.MainWindowHandle != IntPtr.Zero) break;
                Thread.Sleep(500);
            }
            if (p.MainWindowHandle == IntPtr.Zero)
            {
                Console.WriteLine("未发现主窗口句柄，可能是无边框/延迟创建。仍尝试发送按键");
            }
            else
            {
                ShowWindow(p.MainWindowHandle, SW_RESTORE); // 恢复窗口
                SetForegroundWindow(p.MainWindowHandle);    // 激活窗口
            }

            // 等待UI完全加载
            Thread.Sleep(2000);

            Console.WriteLine("切换 Console...");
            // Ctrl+Shift+J
            SendKeys.SendWait("^+J");
            Thread.Sleep(800);
            // 如果你的客户端是 Ctrl+`，请改为：
            // SendKeys.SendWait("^{`}");
            // Thread.Sleep(800);

            // 6) 准备注入的脚本（全局替换字体）
            string js = @"
var style=document.createElement('style');
style.innerHTML='*{font-family:""HarmonyOS Sans SC"",sans-serif!important;}span[data-type=""code""] { font-family: ""JetBrains Maple Mono"", monospace !important; }';
document.head.appendChild(style);
";
            // 也可以更细：只改 body 或特定容器
            // style.innerHTML='body{font-family:""Microsoft YaHei"",sans-serif!important;}';

            // 7) 放入剪贴板并粘贴执行
            Console.WriteLine("注入 CSS...");
            try
            {
                // 设置剪贴板（STA 线程要求，Main 默认是 MTA，改为 STA 最简单）
                // 在 .csproj 里把 <OutputType>Exe</OutputType> 配合 <UseWindowsForms>true</UseWindowsForms> 即可；
                // 或者把 Main 标记为 [STAThread]
            }
            catch { }

            Clipboard.SetText(js);
            SendKeys.SendWait("^v");
            Thread.Sleep(100);
            SendKeys.SendWait("{ENTER}");

            //Console.WriteLine("完成。");

            //Console.WriteLine("退出 DevTool");
            //SendKeys.SendWait("^+I");

            // 注入完 CSS 后
            Console.WriteLine("完成注入，关闭 DevTools...");
            Thread.Sleep(500);  // 等待渲染完成
            SendKeys.SendWait("{F12}"); // 再按一次 F12 关闭 DevTools
        }
    }
}
