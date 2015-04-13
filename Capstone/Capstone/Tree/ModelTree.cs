using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeApi.Tree {
    public class ModelTree : IDataTree {

        public DataNode Root { get; private set; }

        private IBaseTree baseTree;
        private int words;

        public ModelTree(IBaseTree tree) {
            baseTree = tree;
        }

        public void AddConnection(string word) {
            if (baseTree.Contains(word)) {

            } //else ignore
        }
    }
}
