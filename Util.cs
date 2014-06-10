using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace UtilN {

    public class Util {

        public static void Add(Control c, Control d, int ix) {
            c.Controls.Add(d);
            c.Controls.SetChildIndex(d, ix);
        }

        public static MeasureTimer Mt(string s = "") {
            return new MeasureTimer(s);
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

    public class RadioMenuItem : MenuItem {

        public RadioMenuItem(string text, EventHandler onClick)
            : base(text, onClick) {
            RadioCheck = true;
        }
    }
}
