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

        public long MappedWords { get; set; }

        internal DocumentMappedTree(IBaseTree tree) {
            BaseTree = tree;
            Root = new DataNode(tree.Root.KeyWord);
        }

        public void SetBaseTree(IBaseTree tree) {
            this.BaseTree = tree;
        }

        public IBaseTree GetBaseTree() {
            return this.BaseTree;
        }

        public void AddConnection(string word) {
            word = StringFunctions.Normalize(word);
            if (BaseTree.Contains(word)) {
                MappedWords++;
                if(word != Root.Keyword) {
                    Node parent = BaseTree.GetNode(word).Parent;
                    AddConnection(parent.KeyWord, word);
                }

            } //else... do nothing
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

        public string Name {
            get;
            set;
        }
    }
}
