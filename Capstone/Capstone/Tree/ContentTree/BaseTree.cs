using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeApi.Tree {
    [Serializable]
    public class BaseTree : IBaseTree {
        private Dictionary<string, Node> treeNodes;
        public Node Root { get; set; }

        public BaseTree() {
            treeNodes = new Dictionary<string, Node>();
        }

        public void AddNode(Node parent, Node newNode) {
            if (parent == null) {
                Root = newNode;
            } else if (!Contains(parent.KeyWord)) {
                throw new InvalidOperationException("Parent node must be contained within tree");
            }
            newNode.Parent = parent;
            newNode.KeyWord = StringFunctions.Normalize(newNode.KeyWord);
            treeNodes.Add(newNode.KeyWord, newNode);
        }

        public void AddWord(Node parent, string word) {
            Node newNode = new Node() { KeyWord = StringFunctions.Normalize(word) };
            AddNode(parent, newNode);
        }

        public void AddWord(string parent, string word) {
            parent = StringFunctions.Normalize(parent);
            Node parentNode = null;
            if (parent != null && !Contains(parent)) {
                throw new InvalidOperationException("Parent node must be contained within tree");
            }
            if (parent != null) {
                parentNode = treeNodes[parent];
            }
            AddWord(parentNode, word);
        }

        public bool Contains(string word) {
            return treeNodes.Keys.Contains(StringFunctions.Normalize(word));
        }

        public Node GetNode(string word) {
            return treeNodes[StringFunctions.Normalize(word)];
        }

        public void RemoveNode(Node toRemove) {
            if (toRemove == null || !treeNodes.ContainsValue(toRemove)) {
                throw new InvalidOperationException();
            }
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
            if (treeNodes.Count == 0) {
                Root = null;
            }
        }

        public IEnumerator<Node> GetEnumerator() {
            return treeNodes.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }


        public void Rename(string original, string newWord) {
            newWord = StringFunctions.Normalize(newWord);
            Node node = GetNode(original);
            treeNodes.Remove(original);
            node.KeyWord = newWord;
            treeNodes.Add(node.KeyWord, node);
        }
    }
}
