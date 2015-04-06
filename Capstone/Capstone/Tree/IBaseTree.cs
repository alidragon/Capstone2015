using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Tree {
    public interface IBaseTree {
        public Node Root { get; set; }

        public void AddNode(Node parent, Node newNode);
        public void AddWord(Node parent, string word);
        public void AddWord(string parent, string word);
        public bool Contains(string word);
        public Node GetNode(string word);
    }
}
