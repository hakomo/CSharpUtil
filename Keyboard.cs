using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace UtilN {

    public class Keyboard {

        [DllImport("user32")]
        private static extern void keybd_event(byte v, byte s, int f, int i);

        private const int KEYEVENTF_KEYUP = 2;

        public static void Input(Keys[] m, Keys k) {
            foreach(Keys l in m)
                keybd_event(Convert.ToByte(l), 0, 0, 0);
            keybd_event(Convert.ToByte(k), 0, 0, 0);
            keybd_event(Convert.ToByte(k), 0, KEYEVENTF_KEYUP, 0);
            foreach(Keys l in m)
                keybd_event(Convert.ToByte(l), 0, KEYEVENTF_KEYUP, 0);
        }
    }
}
