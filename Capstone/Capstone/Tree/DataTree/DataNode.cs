using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeApi.Tree {
    public class DataNode {
        public LinkedList<Connection> Children { get; set; }
        public string Keyword { get; set; }
        public DataNode(string word) {
            this.Keyword = word;
            Children = new LinkedList<Connection>();
        }
    }
}
