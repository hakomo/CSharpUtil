
namespace Hakomo.Library {

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
