using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeApi.Tree.DataTree {
    [Serializable]
    public class StemmedDocumentMap : IDataTree {
        private IBaseTree baseTree;
        private IDataTree tree;

        public StemmedDocumentMap(IDataTree tree) {
            baseTree = tree.GetBaseTree();
            this.tree = tree;

            var enumer = baseTree.GetEnumerator();
            List<Node> nodes = new List<Node>();
            while (enumer.MoveNext()) {
                nodes.Add((Node)enumer.Current);
            }
            for (int i = 0; i < nodes.Count; i++) {
                Node s = nodes[i];
                baseTree.Rename(s, StringFunctions.StemmedWord(s.KeyWord));
            }
        }

        public void AddConnection(string word) {
            word = StringFunctions.StemmedWord(word);
            tree.AddConnection(word);
        }

        public DataNode Root {
            get { return tree.Root; }
        }

        public string Name {
            get {
                return tree.Name;
            }
            set {
                tree.Name = value;
            }
        }

        public long MappedWords {
            get { return tree.MappedWords; }
        }

        public void SetBaseTree(IBaseTree tree) {
            baseTree = tree;
            this.tree.SetBaseTree(tree);

            var enumer = baseTree.GetEnumerator();
            List<Node> nodes = new List<Node>();
            while (enumer.MoveNext()) {
                nodes.Add((Node)enumer.Current);
            }
            for (int i = 0; i < nodes.Count; i++) {
                Node s = nodes[i];
                baseTree.Rename(s, StringFunctions.StemmedWord(s.KeyWord));
            }
        }

        public IBaseTree GetBaseTree() {
            return tree.GetBaseTree();
        }
    }
}
