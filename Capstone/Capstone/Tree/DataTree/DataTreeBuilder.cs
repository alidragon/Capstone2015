using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeApi.Tree {
    public class DataTreeBuilder {

        public static IDataTree CreateDocumentMappedTree(IBaseTree baseTree) {
            if (baseTree == null) {
                throw new ArgumentNullException();
            }
            IDataTree toReturn = new DocumentMappedTree(baseTree);
            return toReturn;
        }

        public static IDataTree CreateDocumentMappedTree(IBaseTree baseTree, string content) {
            if (baseTree == null || content == null) {
                throw new ArgumentNullException();
            }
            IDataTree toReturn = CreateDocumentMappedTree(baseTree);
            AddToDataTree(toReturn, content);
            return toReturn;
        }

        public static void AddToDataTree(IDataTree dataTree, string content) {
            if (dataTree == null || content == null) {
                throw new ArgumentNullException();
            }
            try {
                string[] words = content.Split(' ');
                foreach (string w in words) {
                    dataTree.AddConnection(w);
                }
            } catch (NullReferenceException) {
                throw new InvalidOperationException("Data Tree's base tree cannot be null");
            }
        }

        public static void AddAllToDataTree(IDataTree tree, IEnumerable<string> content) {
            if (tree == null || content == null) {
                throw new ArgumentNullException();
            }
            foreach (string s in content) {
                AddToDataTree(tree, s);
            }
        }
    }
}
