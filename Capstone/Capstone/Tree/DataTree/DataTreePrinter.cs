using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeApi.Tree.DataTree {
    public static class DataTreePrinter {
        public static void PrintDataTree(this IDataTree tree) {
            Console.WriteLine(tree.Root.Keyword + " : " + tree.MappedWords);
            foreach (Connection c in tree.Root.Children) {
                c.PrintConnection("  ");
            }
        }

        private static void PrintConnection(this Connection c, string prefix) {
            Console.WriteLine(prefix + c.EndPoint.Keyword + " : " + c.Weight);
            foreach (Connection con in c.EndPoint.Children) {
                con.PrintConnection(prefix + "  ");
            }
        }
    }
}
