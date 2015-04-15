using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeApi.Tree;

namespace UnitTests {
    [TestClass]
    public class DocumentMappingTests {

        private string toMap = "This is a whole series of words which are going to be mapped to a content tree.";
        private string root = "Root";

        public IBaseTree setUpTree() {
            IBaseTree tree = new BaseTree();
            tree.AddWord((string)null, "Root");
            tree.AddWord(root, "tree");
            tree.AddWord(root, "Others");
            tree.AddWord("tree", "content");
            tree.AddWord("tree", "mapped");
            tree.AddWord("others", "series");
            tree.AddWord("others", "going");
            tree.AddWord(root, "nonexistent");

            return tree;
        }

        [TestMethod]
        public void DocumentMappingTest() {
            IBaseTree tree = setUpTree();

            IDataTree dataTree = DataTreeBuilder.CreateDocumentMappedTree(tree);
            DataTreeBuilder.AddToDataTree(dataTree, toMap);


        }
    }
}
