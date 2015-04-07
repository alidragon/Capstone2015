using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Tree {
    public class Node {
        public Node Parent { get; set; }
        public string KeyWord { get; set; }

        public Node() {

        }

        public Node(string keyword) {
            this.KeyWord = keyword;
        }
    }
}
