using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeApi.Tree;

namespace TreeBuilder {
    public class ViewableTree {
        IBaseTree tree;
        public ObservableCollection<ViewableTreeNode> Root { get; set; }

        public ViewableTree(IBaseTree basetree) {
            List<ViewableTreeNode> temp = new List<ViewableTreeNode>();
            List<Node> orphans = new List<Node>();
            Dictionary<Node, ViewableTreeNode> nodes = new Dictionary<Node, ViewableTreeNode>();
            
            tree = basetree;
            Root = new ObservableCollection<ViewableTreeNode>();
            ViewableTreeNode rootNode = new ViewableTreeNode(tree.Root);
            temp.Add(rootNode);
            nodes.Add(tree.Root, rootNode);
            Root.Add(rootNode);

            foreach (Node n in tree) {
                if (n != tree.Root) {
                    ViewableTreeNode node = new ViewableTreeNode(n);
                    temp.Add(node);
                    nodes.Add(n, node);
                    var result = temp.Where(t => n.Parent != null && t.Keyword == n.Parent.KeyWord).FirstOrDefault();
                    if (result != null) {
                        result.Children.Add(node);
                    } else {
                        orphans.Add(n);
                    }
                }
            }

            foreach (Node n in orphans) {
                var result = temp.Where(t => n.Parent != null && t.Keyword == n.Parent.KeyWord).FirstOrDefault();
                if (result != null) {
                    result.Children.Add(nodes[n]);
                }
            }
        }

        public void Add(ViewableTreeNode parent, string word) {
            Node parentNode = parent.treeNode;
            Node childNode = new Node(word);

            tree.AddNode(parentNode, childNode);

            ViewableTreeNode childViewableNode = new ViewableTreeNode(childNode);
            parent.Children.Add(childViewableNode);
        }

        public void Remove(ViewableTreeNode toRemove) {
            if (toRemove.Keyword == tree.Root.KeyWord) {
                throw new InvalidOperationException();
            }
            Node parent = toRemove.treeNode.Parent;

            tree.RemoveNode(toRemove.treeNode);

            ViewableTreeNode parentViewable = FindParent(Root[0], toRemove);
            parentViewable.Children.Remove(toRemove);
        }

        public void Rename(ViewableTreeNode node, string newName) {
            tree.Rename(node.treeNode, newName);
            node.Keyword = newName;
        }

        private ViewableTreeNode FindParent(ViewableTreeNode current, ViewableTreeNode child) {
            ViewableTreeNode toReturn = null;
            foreach (ViewableTreeNode node in current.Children) {
                if (toReturn == null) {
                    if (current.Children.Contains(child)) {
                        toReturn = current;
                    } else {
                        toReturn = FindParent(node, child);
                    }
                }
            }
            return toReturn;
        }

    }

    public class ViewableTreeNode : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _keyword;
        public string Keyword {
            get {
                return _keyword;
            }
            set {
                _keyword = value;
                if (PropertyChanged != null) {
                    PropertyChanged(this, new PropertyChangedEventArgs("Keyword"));
                }
            }
        }

        public ObservableCollection<ViewableTreeNode> Children { get; set; }

        public Node treeNode { get; set; }

        public ViewableTreeNode() {
            Children = new ObservableCollection<ViewableTreeNode>();
        }

        public ViewableTreeNode(Node n) {
            Children = new ObservableCollection<ViewableTreeNode>();
            this.Keyword = n.KeyWord;
            this.treeNode = n;
        }

    }

}
