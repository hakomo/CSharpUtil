using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Hakomo.Library {

    public class LayeredForm : Form {

        [DllImport("gdi32")]
        private static extern IntPtr CreateCompatibleDC(IntPtr hdc);
        [DllImport("gdi32")]
        private static extern int DeleteDC(IntPtr hdc);
        [DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr h);
        [DllImport("gdi32")]
        private static extern IntPtr SelectObject(IntPtr hdc, IntPtr h);

        [DllImport("user32")]
        private static extern IntPtr GetDC(IntPtr hw);
        [DllImport("user32")]
        private static extern int ReleaseDC(IntPtr hw, IntPtr hdc);
        [DllImport("user32")]
        private static extern int UpdateLayeredWindow(IntPtr hw, IntPtr hdcDst, ref Point pDst,
            ref Size s, IntPtr hdcSrc, ref Point pSrc, int c, ref BLENDFUNCTION b, int f);

        private const int WS_EX_LAYERED = 0x80000;

        [StructLayout(LayoutKind.Sequential)]
        private struct BLENDFUNCTION {
            public byte BlendOp, BlendFlags, SourceConstantAlpha, AlphaFormat;

            public static BLENDFUNCTION Default {
                get {
                    return new BLENDFUNCTION { SourceConstantAlpha = 255, AlphaFormat = 1 };
                }
            }
        }

        protected LayeredForm() {
            FormBorderStyle = FormBorderStyle.None;
        }

        public void SetLayeredBitmap(Point location, Bitmap bmp) {
            IntPtr hdcDst = IntPtr.Zero, hdcSrc = IntPtr.Zero, hbmpNew = IntPtr.Zero, hbmpOld = IntPtr.Zero;
            try {
                hdcDst = GetDC(IntPtr.Zero);
                hdcSrc = CreateCompatibleDC(hdcDst);
                hbmpNew = bmp.GetHbitmap(Color.FromArgb(0));
                hbmpOld = SelectObject(hdcSrc, hbmpNew);
                Bounds = new Rectangle(location, bmp.Size);
                Point p = new Point();
                Size s = bmp.Size;
                BLENDFUNCTION b = BLENDFUNCTION.Default;
                UpdateLayeredWindow(Handle, hdcDst, ref location, ref s, hdcSrc, ref p, 0, ref b, 2);
            } finally {
                if(hdcDst != IntPtr.Zero)
                    ReleaseDC(IntPtr.Zero, hdcDst);
                if(hdcSrc != IntPtr.Zero)
                    DeleteDC(hdcSrc);
                if(hbmpNew != IntPtr.Zero) {
                    SelectObject(hdcSrc, hbmpOld);
                    DeleteObject(hbmpNew);
                }
            }
        }

        protected override CreateParams CreateParams {
            get {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= WS_EX_LAYERED;
                return cp;
            }
        }
    }
}
