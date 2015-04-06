using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Tree {
    public class BaseTree : IBaseTree {
        private Dictionary<string, Node> treeNodes;
        public Node Root { get; set; }

        public void AddNode(Node parent, Node newNode) {
            newNode.Parent = parent;
            treeNodes.Add(newNode.KeyWord, newNode);
        }

        public void AddWord(Node parent, string word) {
            Node newNode = new Node() { KeyWord = word };
            AddNode(parent, newNode);
        }

        public void AddWord(string parent, string word) {
            Node parentNode = treeNodes[parent];
            if (parentNode == null) {
                throw new InvalidOperationException();
            }
            AddWord(parentNode, word);
        }

        public bool Contains(string word) {
            return treeNodes.Keys.Contains(word);
        }

        public Node GetNode(string word) {
            return treeNodes[word];
        }

        public void RemoveNode(Node toRemove) {
            treeNodes.Remove(toRemove.KeyWord);
            List<Node> remove = new List<Node>();
            foreach (string key in treeNodes.Keys) {
                if (GetNode(key).Parent == toRemove) {
                    remove.Add(GetNode(key));
                }
            }
            foreach (Node n in remove) {
                RemoveNode(n);
            }
        }
    }
}
