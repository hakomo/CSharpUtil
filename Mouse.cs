using System.Drawing;
using System.Runtime.InteropServices;

namespace Hakomo.Library {

    public class Mouse {

        [DllImport("user32")]
        private static extern bool GetCursorPos(out Point p);
        [DllImport("user32")]
        private static extern short GetKeyState(int k);
        [DllImport("user32")]
        private static extern void mouse_event(int f, int x, int y, int d, int i);
        [DllImport("user32")]
        private static extern bool SetCursorPos(int x, int y);

        private const int MOUSEEVENTF_LEFTDOWN = 2, MOUSEEVENTF_LEFTUP = 4,
            MOUSEEVENTF_RIGHTDOWN = 8, MOUSEEVENTF_RIGHTUP = 0x10,
            MOUSEEVENTF_MIDDLEDOWN = 0x20, MOUSEEVENTF_MIDDLEUP = 0x40,
            VK_LBUTTON = 1, VK_RBUTTON = 2, VK_MBUTTON = 4;

        public static void Down() {
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
        }

        public static void Up() {
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        public static void RightDown() {
            mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
        }

        public static void RightUp() {
            mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
        }

        public static void MiddleDown() {
            mouse_event(MOUSEEVENTF_MIDDLEDOWN, 0, 0, 0, 0);
        }

        public static void MiddleUp() {
            mouse_event(MOUSEEVENTF_MIDDLEUP, 0, 0, 0, 0);
        }

        public static void Click() {
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        public static void Click(Point p) {
            Location = p;
            Click();
        }

        public static void Click(int x, int y) {
            Click(new Point(x, y));
        }

        public static void DoubleClick() {
            int i;
            for(i = 0; i < 2; ++i)
                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        public static void DoubleClick(Point p) {
            Location = p;
            DoubleClick();
        }

        public static void DoubleClick(int x, int y) {
            DoubleClick(new Point(x, y));
        }

        public static void RightClick() {
            mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
        }

        public static void RightClick(Point p) {
            Location = p;
            RightClick();
        }

        public static void RightClick(int x, int y) {
            RightClick(new Point(x, y));
        }

        public static void MiddleClick() {
            mouse_event(MOUSEEVENTF_MIDDLEDOWN | MOUSEEVENTF_MIDDLEUP, 0, 0, 0, 0);
        }

        public static void MiddleClick(Point p) {
            Location = p;
            MiddleClick();
        }

        public static void MiddleClick(int x, int y) {
            MiddleClick(new Point(x, y));
        }

        public static void DragAndDrop(Point to) {
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            Location = to;
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        public static void DragAndDrop(Point to, Point from) {
            Location = from;
            DragAndDrop(to);
        }

        public static void DragAndDrop(int toX, int toY) {
            DragAndDrop(new Point(toX, toY));
        }

        public static void DragAndDrop(int toX, int toY, int fromX, int fromY) {
            DragAndDrop(new Point(toX, toY), new Point(fromX, fromY));
        }

        public static Point Location {
            get {
                Point p;
                GetCursorPos(out p);
                return p;
            }
            set {
                SetCursorPos(value.X, value.Y);
            }
        }

        public static bool IsDown {
            get {
                return GetKeyState(VK_LBUTTON) < 0;
            }
        }

        public static bool IsRightDown {
            get {
                return GetKeyState(VK_RBUTTON) < 0;
            }
        }

        public static bool IsMiddleDown {
            get {
                return GetKeyState(VK_MBUTTON) < 0;
            }
        }
    }
}
