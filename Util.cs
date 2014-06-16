using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace UtilN {

    public class Util {

        private static Random r = new Random();

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

        public static MeasureTimer Mt(string s = "") {
            return new MeasureTimer(s);
        }

        public static int Rand(int n) {
            return r.Next(n);
        }

        public static int Rand(int n, int m) {
            return r.Next(m - n) + n;
        }

        public static void Run(Action a) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            a();
        }

        public static void RunMutex(Action a, string s) {
            using(Mutex m = new Mutex(false, s)) {
                if(m.WaitOne(0, false)) {
                    Run(a);
                } else {
                    Process p = Process.GetCurrentProcess();
                    foreach(Process q in Process.GetProcessesByName(p.ProcessName)) {
                        if(p.Id != q.Id)
                            WinAPI.Activate(q.MainWindowHandle);
                    }
                }
            }
        }

        public class MeasureTimer : IDisposable {

            private readonly string s;
            private readonly Stopwatch sw = new Stopwatch();

            public MeasureTimer(string s = "") {
                this.s = s;
                sw.Start();
            }

            public void Dispose() {
                sw.Stop();
                Console.WriteLine("{0, 5} ms : {1}", sw.ElapsedMilliseconds, s);
            }
        }
    }

    public class Bit {

        private readonly int[] a;

        public Bit(int n) {
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

    public class RadioMenuItem : MenuItem {

        public RadioMenuItem(string text, EventHandler onClick)
            : base(text, onClick) {
            RadioCheck = true;
        }
    }
}
