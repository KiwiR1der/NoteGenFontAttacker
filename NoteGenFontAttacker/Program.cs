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
            Console.WriteLine("首次运行需打开控制台（键盘 F12）手动键入 “允许粘贴” 以允许外部 js 粘贴到浏览器中。。。");

            // 修改为你的 note-gen.exe 路径
            string exePath = @"D:\04Tools\NoteGen\note-gen.exe";

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

            ShowWindow(p.MainWindowHandle, SW_RESTORE); // 恢复窗口
            SetForegroundWindow(p.MainWindowHandle);    // 激活窗口

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
                style.innerHTML='*{font-family:""HarmonyOS Sans SC"",sans-serif!important;} span[data-type=""code""] { font-family: ""JetBrains Maple Mono"", monospace !important; }';
                document.head.appendChild(style);
            ";

            // 7) 放入剪贴板并粘贴执行
            Console.WriteLine("注入 CSS...");
            try
            {
                Clipboard.SetText(js);
                SendKeys.SendWait("^v");
                Thread.Sleep(500);
                SendKeys.SendWait("{ENTER}");

                // 注入完 CSS 后
                Console.WriteLine("完成注入，关闭 DevTools...");
                Thread.Sleep(500);  // 等待渲染完成
                SendKeys.SendWait("{F12}"); // 再按一次 F12 关闭 DevTools
            }
            catch { }
        }
    }
}
