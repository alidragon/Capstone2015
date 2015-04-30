using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeApi.Tree.ContentTree {
    [Serializable]
    public class DuplicatesAllowedBaseTree : IBaseTree {
        private Dictionary<string, List<Node>> treeNodes;
        public Node Root { get; set; }

        public DuplicatesAllowedBaseTree() {
            treeNodes = new Dictionary<string, List<Node>>();
        }

        public void AddNode(Node parent, Node newNode) {
            if (parent == null) {
                Root = newNode;
            } else if (!Contains(parent.KeyWord)) {
                throw new InvalidOperationException("Parent node must be contained within tree");
            }
            newNode.Parent = parent;
            newNode.KeyWord = StringFunctions.Normalize(newNode.KeyWord);
            if (treeNodes.ContainsKey(newNode.KeyWord)) {
                treeNodes[newNode.KeyWord].Add(newNode);
            } else {
                treeNodes.Add(newNode.KeyWord, new List<Node>() { newNode });
            }
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
                parentNode = treeNodes[parent].First();
            }
            AddWord(parentNode, word);
        }

        public bool Contains(string word) {
            return treeNodes.ContainsKey(word);
        }

        /// <summary>
        /// returns the first node which has a keyword of word
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public Node GetNode(string word) {
            return treeNodes[word].First();
        }

        public void RemoveNode(Node toRemove) {
            if (toRemove == null || !treeNodes.ContainsKey(toRemove.KeyWord)) {
                throw new InvalidOperationException();
            }
            List<Node> keywordsList = treeNodes[toRemove.KeyWord];
            keywordsList.Remove(toRemove);
            if (keywordsList.Count == 0) {
                treeNodes.Remove(toRemove.KeyWord);
            }
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
            foreach (string s in treeNodes.Keys) {
                foreach (Node n in treeNodes[s]) {
                    yield return n;
                }
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }


        public void Rename(Node original, string newWord) {
            newWord = StringFunctions.Normalize(newWord);

            List<Node> keywordsList = treeNodes[original.KeyWord];
            keywordsList.Remove(original);
            if (keywordsList.Count == 0) {
                treeNodes.Remove(original.KeyWord);
            }

            original.KeyWord = newWord;
            if (treeNodes.ContainsKey(original.KeyWord)) {
                treeNodes[original.KeyWord].Add(original);
            } else {
                treeNodes.Add(original.KeyWord, new List<Node>() { original });
            }
        }
    }
}
