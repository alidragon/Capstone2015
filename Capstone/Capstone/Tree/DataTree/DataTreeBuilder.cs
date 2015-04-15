using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeApi.Tree {
    public class DataTreeBuilder {

        public static IDataTree CreateDocumentMappedTree(IBaseTree baseTree) {
            IDataTree toReturn = new DocumentMappedTree(baseTree);
            return toReturn;
        }

        public static IDataTree CreateDocumentMappedTree(IBaseTree baseTree, string content) {
            IDataTree toReturn = CreateDocumentMappedTree(baseTree);
            AddToDataTree(toReturn, content);
            return toReturn;
        }

        public static void AddToDataTree(IDataTree dataTree, string content) {
            string[] words = content.Split(' ');
            foreach (string w in words) {
                dataTree.AddConnection(w);
            }
        }

        public static void AddAllToDataTree(IDataTree tree, IEnumerable<string> content) {
            foreach (string s in content) {
                AddToDataTree(tree, s);
            }
        }
    }
}
