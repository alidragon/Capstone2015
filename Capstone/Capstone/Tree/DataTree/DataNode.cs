using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeApi.Tree {
    [Serializable]
    public class DataNode {
        public LinkedList<Connection> Children { get; set; }
        public string Keyword { get; set; }
        public DataNode(string word) {
            this.Keyword = word;
            Children = new LinkedList<Connection>();
        }

        public override bool Equals(object obj) {
            if (obj is DataNode) {
                DataNode node = obj as DataNode;
                return node.Keyword.Equals(this.Keyword);
            }
            return false;
        }
    }
}
