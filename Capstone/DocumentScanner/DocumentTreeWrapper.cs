using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeApi.Tree;
using System.Collections.ObjectModel;

namespace DocumentScanner {
    public class DocumentTreeWrapper {
        public ObservableCollection<WrapperNode> Tree { get; set; }

        public DocumentTreeWrapper(IDataTree tree) {
            WrapperNode baseWrapper = new WrapperNode();
            baseWrapper.Keyword = tree.Root.Keyword;
            baseWrapper.Weight = tree.MappedWords;
            Tree = new ObservableCollection<WrapperNode>();
            Tree.Add(baseWrapper);
            CreateNodes(tree.Root, baseWrapper);
        }

        private void CreateNodes(DataNode dataNode, WrapperNode baseWrapper) {
            if (dataNode.Children != null) {
                foreach (Connection c in dataNode.Children) {
                    WrapperNode wrap = new WrapperNode();
                    wrap.Keyword = c.EndPoint.Keyword;
                    wrap.Weight = c.Weight;
                    baseWrapper.Children.Add(wrap);
                    CreateNodes(c.EndPoint, wrap);
                }
            }
        }
    }
}
