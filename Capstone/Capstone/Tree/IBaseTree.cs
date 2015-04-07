using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Tree {
    public interface IBaseTree {
        Node Root { get; set; }

        void AddNode(Node parent, Node newNode);
        void AddWord(Node parent, string word);
        void AddWord(string parent, string word);
        bool Contains(string word);
        Node GetNode(string word);
        /// <summary>
        /// Should remove the node and any child nodes from the tree
        /// </summary>
        /// <param name="toRemove"></param>
        void RemoveNode(Node toRemove);
    }
}
