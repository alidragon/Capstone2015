using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeApi.Tree {
    [Serializable]
    public class DocumentMappedTree : IDataTree {

        public DataNode Root { get; private set; }

        [NonSerialized]
        private IBaseTree BaseTree;

        private int words;

        public DocumentMappedTree(IBaseTree tree) {
            BaseTree = tree;
            Root = new DataNode(tree.Root.KeyWord);
        }

        public void SetBaseTree(IBaseTree tree) {
            this.BaseTree = tree;
        }

        public void AddConnection(string word) {
            word = StringFunctions.Normalize(word);
            if (BaseTree.Contains(word)) {
                words++;
                if(word != Root.Keyword) {
                    Node parent = BaseTree.GetNode(word).Parent;
                    AddConnection(parent.KeyWord, word);
                }

            } //else ignore
        }

        private DataNode AddConnection(string parent, string child) {
            DataNode parentNode;
            if (parent == Root.Keyword.ToLower()) {
                parentNode = Root;
            } else {
                parentNode = AddConnection(BaseTree.GetNode(parent).Parent.KeyWord, parent);
            }
            DataNode childNode;
            Connection c = parentNode.Children.Where(x => x.EndPoint.Keyword == child).FirstOrDefault();
            if (c != null) {
                c.Weight++;
                childNode = c.EndPoint;
            } else {
                childNode = new DataNode(child);
                c = new Connection(childNode);
                c.Weight++;
                parentNode.Children.AddFirst(c);
            }
            return childNode;
        }
    }
}
