using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace UtilN {

    public class WinAPI {

        private delegate bool wep(IntPtr hw, int lp);

        [DllImport("shell32")]
        private static extern IntPtr ShellExecuteA(int hw, int o, string f, int p, int d, int c);

        [DllImport("user32")]
        private static extern bool AllowSetForegroundWindow(int pid);
        [DllImport("user32")]
        private static extern bool AttachThreadInput(int ida, int idat, bool a);
        [DllImport("user32")]
        private static extern bool DestroyIcon(IntPtr hi);
        [DllImport("user32")]
        private static extern bool EnumWindows(wep ewp, int lp);
        [DllImport("user32")]
        private static extern IntPtr GetClassLong(IntPtr hw, int i);
        [DllImport("user32")]
        public static extern IntPtr GetForegroundWindow();
        [DllImport("user32")]
        private static extern IntPtr GetWindow(IntPtr hw, int c);
        [DllImport("user32")]
        private static extern int GetWindowLong(IntPtr hw, int i);
        [DllImport("user32")]
        private static extern bool GetWindowPlacement(IntPtr hw, out WINDOWPLACEMENT wp);
        [DllImport("user32")]
        private static extern bool GetWindowRect(IntPtr hw, out RECT r);
        [DllImport("user32")]
        private static extern int GetWindowText(IntPtr hw, StringBuilder s, int mx);
        [DllImport("user32")]
        private static extern int GetWindowThreadProcessId(IntPtr hw, out int pid);
        [DllImport("user32")]
        public static extern bool IsIconic(IntPtr hw);
        [DllImport("user32")]
        private static extern bool IsWindowVisible(IntPtr hw);
        [DllImport("user32")]
        public static extern bool IsZoomed(IntPtr hw);
        [DllImport("user32")]
        public static extern int PostMessage(IntPtr hw, int m, int wp, int lp);
        [DllImport("user32")]
        public static extern bool RegisterHotKey(IntPtr hw, int id, int m, int k);
        [DllImport("user32")]
        private static extern IntPtr SendMessage(IntPtr hw, int m, int wp, int lp);
        [DllImport("user32")]
        public static extern bool SetForegroundWindow(IntPtr hw);
        [DllImport("user32")]
        private static extern bool SetWindowPos(IntPtr hw, IntPtr hwia, int x, int y, int w, int h, int f);
        [DllImport("user32")]
        private static extern bool SetWindowPlacement(IntPtr hw, ref WINDOWPLACEMENT wp);
        [DllImport("user32")]
        private static extern bool ShowWindow(IntPtr hw, int c);
        [DllImport("user32")]
        private static extern bool SystemParametersInfo(int ua, int up, IntPtr pp, int wi);
        [DllImport("user32")]
        public static extern bool UnregisterHotKey(IntPtr hw, int id);

        private const int GCLP_HICON = -14, GW_HWNDPREV = 3, GW_OWNER = 4, GWL_EXSTYLE = -20, GWL_STYLE = -16,
            ICON_BIG = 1, SW_RESTORE = 9, SW_SHOWMAXIMIZED = 3, SW_SHOWMINIMIZED = 2, SW_SHOWNORMAL = 1,
            SPI_GETFOREGROUNDLOCKTIMEOUT = 0x2000, SPI_SETFOREGROUNDLOCKTIMEOUT = 0x2001,
            SWP_NOACTIVATE = 0x10, SWP_NOMOVE = 2, SWP_NOSIZE = 1,
            WM_CLOSE = 0x10, WM_GETICON = 0x7f, WM_SETREDRAW = 11,
            WS_EX_TOOLWINDOW = 0x80, WS_MAXIMIZEBOX = 0x10000, WS_MINIMIZEBOX = 0x20000, WS_SIZEBOX = 0x40000;

        public static void Activate(IntPtr hw) {
            if(IsIconic(hw))
                ShowWindow(hw, SW_RESTORE);
            SetForegroundWindow(hw);
        }

        public static void BeforeActivate(IntPtr hw, IntPtr hwia) {
            if(IsIconic(hw)) {
                ShowWindow(hw, SW_RESTORE);
                SetForegroundWindow(hwia);
            } else {
                SetWindowPos(hw, hwia, 0, 0, 0, 0, SWP_NOACTIVATE | SWP_NOMOVE | SWP_NOSIZE);
            }
        }

        public static bool CanMaximize(IntPtr hw) {
            return (GetWindowLong(hw, GWL_STYLE) & WS_MAXIMIZEBOX) != 0;
        }

        public static bool CanMinimize(IntPtr hw) {
            return (GetWindowLong(hw, GWL_STYLE) & WS_MINIMIZEBOX) != 0;
        }

        public static void Close(IntPtr hw) {
            SendMessage(hw, WM_CLOSE, 0, 0);
        }

        public static void Execute(string p) {
            ShellExecuteA(0, 0, p, 0, 0, SW_SHOWNORMAL);
        }

        public static void ForceFore(IntPtr hw) {
            int pid;
            int f = GetWindowThreadProcessId(GetForegroundWindow(), out pid),
                t = GetWindowThreadProcessId(hw, out pid);
            AllowSetForegroundWindow(pid);
            AttachThreadInput(t, f, true);
            IntPtr s = IntPtr.Zero;
            SystemParametersInfo(SPI_GETFOREGROUNDLOCKTIMEOUT, 0, s, 0);
            SystemParametersInfo(SPI_SETFOREGROUNDLOCKTIMEOUT, 0, IntPtr.Zero, 0);
            Activate(hw);
            SystemParametersInfo(SPI_SETFOREGROUNDLOCKTIMEOUT, 0, s, 0);
            AttachThreadInput(t, f, false);
        }

        public static string GetCaption(IntPtr hw) {
            StringBuilder sb = new StringBuilder(90);
            GetWindowText(hw, sb, sb.Capacity);
            return sb.ToString();
        }

        public static Icon GetIcon(IntPtr hw) {
            IntPtr hi = SendMessage(hw, WM_GETICON, ICON_BIG, 0);
            if(hi == IntPtr.Zero)
                hi = GetClassLong(hw, GCLP_HICON);
            if(hi == IntPtr.Zero)
                return SystemIcons.Application;
            DestroyIcon(hi);
            return Icon.FromHandle(hi);
        }

        public static Process GetProcess(IntPtr hw) {
            int pid;
            GetWindowThreadProcessId(hw, out pid);
            return Process.GetProcessById(pid);
        }

        public static List<IntPtr> GetTasks() {
            List<IntPtr> hws = new List<IntPtr>();
            EnumWindows((hw, lp) => {
                if(IsWindowVisible(hw) && GetWindow(hw, GW_OWNER) == IntPtr.Zero &&
                    (GetWindowLong(hw, GWL_EXSTYLE) & WS_EX_TOOLWINDOW) == 0)
                    hws.Add(hw);
                return true;
            }, 0);
            return hws;
        }

        public static void Maximize(IntPtr hw) {
            ShowWindow(hw, SW_SHOWMAXIMIZED);
        }

        public static void Minimize(IntPtr hw) {
            ShowWindow(hw, SW_SHOWMINIMIZED);
        }

        public static void Move(IntPtr hw, IntPtr hwia, Point p) {
            ShowWindow(hw, SW_RESTORE);
            ShowWindow(hw, SW_RESTORE);
            SetWindowPos(hw, hwia, p.X, p.Y, 0, 0, SWP_NOACTIVATE | SWP_NOSIZE);
        }

        public static void Move(IntPtr hw, IntPtr hwia, Rectangle r) {
            ShowWindow(hw, SW_RESTORE);
            ShowWindow(hw, SW_RESTORE);
            SetWindowPos(hw, hwia, r.X, r.Y, r.Width, r.Height, SWP_NOACTIVATE |
                ((GetWindowLong(hw, GWL_STYLE) & WS_SIZEBOX) == 0 ? SWP_NOSIZE : 0));
        }

        public static void Restore(IntPtr hw) {
            ShowWindow(hw, SW_RESTORE);
        }

        public static void ResumeDraw(IntPtr hw) {
            SendMessage(hw, WM_SETREDRAW, 1, 0);
        }

        public static void SuspendDraw(IntPtr hw) {
            SendMessage(hw, WM_SETREDRAW, 0, 0);
        }

        public class Placement {

            private readonly IntPtr hw, hwia;

            private RECT r;
            private WINDOWPLACEMENT wp;

            public Placement(IntPtr hw) {
                this.hw = hw;
                this.hwia = GetWindow(hw, GW_HWNDPREV);
                GetWindowRect(hw, out r);
                wp.Length = Marshal.SizeOf(typeof(WINDOWPLACEMENT));
                GetWindowPlacement(hw, out wp);
            }

            public void Restore() {
                SetWindowPlacement(hw, ref wp);
                SetWindowPos(hw, hwia, r.Left, r.Top, r.Right - r.Left, r.Bottom - r.Top, SWP_NOACTIVATE);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT {
            public int Left, Top, Right, Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct WINDOWPLACEMENT {
            public int Length, Flags, ShowCmd;
            public Point MinPosition, MaxPosition;
            public RECT NormalPosition;
        }
    }
}
