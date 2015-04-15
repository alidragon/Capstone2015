using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeApi.Tree {
    public interface IBaseTree : IEnumerable {
        Node Root { get; set; }

        void AddNode(Node parent, Node newNode);
        void AddWord(Node parent, string word);
        void AddWord(string parent, string word);
        bool Contains(string word);
        Node GetNode(string word);
        void Rename(string original, string newWord);
        /// <summary>
        /// Should remove the node and any child nodes from the tree
        /// </summary>
        /// <param name="toRemove"></param>
        void RemoveNode(Node toRemove);
    }
}
