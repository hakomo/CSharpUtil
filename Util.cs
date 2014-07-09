using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace Hakomo.Library {

    public class Util {

        private static readonly Random r = new Random();

        public static void Add(Control c, Control d, int ix) {
            c.Controls.Add(d);
            c.Controls.SetChildIndex(d, ix);
        }

        public static int BinarySearch(int l, int r, Predicate<int> p) {
            while(r - l > 1) {
                if(p((l + r) / 2)) {
                    l = (l + r) / 2;
                } else {
                    r = (l + r) / 2;
                }
            }
            return l;
        }

        public static double Rand() {
            return r.NextDouble();
        }

        public static int Rand(int n) {
            return r.Next(n);
        }

        public static int Rand(int n, int m) {
            return r.Next(m - n) + n;
        }

        public static void Run<T>() where T : Form, new() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new T());
        }

        public static void Run<T>(string s) where T : Form, new() {
            Run<T>(s, delegate {
                Process p = Process.GetCurrentProcess();
                foreach(Process q in Process.GetProcessesByName(p.ProcessName)) {
                    if(p.Id != q.Id)
                        WinAPI.Activate(q.MainWindowHandle);
                }
            });
        }

        public static void Run<T>(string s, Action a) where T : Form, new() {
            using(Mutex m = new Mutex(false, s)) {
                if(m.WaitOne(0, false)) {
                    Run<T>();
                } else {
                    a();
                }
            }
        }

        public class StopWatch : IDisposable {

            private readonly string s;
            private readonly Stopwatch sw = new Stopwatch();

            public StopWatch(string s = "") {
                this.s = s;
                sw.Start();
            }

            public void Dispose() {
                sw.Stop();
                Console.WriteLine("{0, 5} ms : {1}", sw.ElapsedMilliseconds, s);
            }
        }
    }

    public class RadioMenuItem : MenuItem {

        public RadioMenuItem(string text, EventHandler onClick)
            : base(text, onClick) {
            RadioCheck = true;
        }
    }

    public class Redraw : IDisposable {

        private readonly Control c;

        public Redraw(Control c) {
            this.c = c;
            WinAPI.SuspendDraw(c.Handle);
            c.SuspendLayout();
        }

        public void Dispose() {
            c.ResumeLayout();
            WinAPI.ResumeDraw(c.Handle);
            c.Refresh();
        }
    }

    public class TemporaryCurrentDirectory : IDisposable {

        private readonly string path;

        public TemporaryCurrentDirectory(string path) {
            this.path = Environment.CurrentDirectory;
            Environment.CurrentDirectory = path;
        }

        public void Dispose() {
            Environment.CurrentDirectory = path;
        }
    }
}
