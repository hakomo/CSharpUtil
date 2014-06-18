using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace UtilN {

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

    public class BIT {

        private readonly int[] a;

        public BIT(int n) {
            a = new int[n];
        }

        public void Update(int i, int p) {
            for(; i < a.Length; i |= i + 1)
                a[i] += p;
        }

        public int this[int i] {
            get {
                int sm = 0;
                for(; i >= 0; i = (i & (i + 1)) - 1)
                    sm += a[i];
                return sm;
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

    public class UnionFind {

        private readonly int[] a, b;

        public UnionFind(int n) {
            int i;
            a = new int[n];
            b = new int[n];
            for(i = 0; i < n; ++i)
                a[i] = i;
        }

        private int Find(int i) {
            a[i] = i == a[i] ? i : Find(a[i]);
            return a[i];
        }

        public bool Same(int i, int j) {
            return Find(i) == Find(j);
        }

        public void Union(int i, int j) {
            i = Find(i);
            j = Find(j);
            if(b[i] < b[j]) {
                a[i] = j;
            } else {
                a[j] = i;
                b[i] += b[i] == b[j] ? 1 : 0;
            }
        }
    }
}
