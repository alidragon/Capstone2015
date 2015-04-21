using Capstone.Tree.DataTree.Comparison;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeApi.Tree;

namespace UnitTests {
    [TestClass]
    public class ComparatorTests {

        private string toMap = "This is a whole series of words which are going to be mapped to a content tree.";
        private string shouldProbablyMatch = "This is a series of words which may or may not match the original tree. I guess I should include mapped and tree once or twice more.";
        private string shouldProbablyNotMatch = "This sentence is nonexistent, and should never be matched.";
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
        public void CompareTrees() {
            IBaseTree tree = setUpTree();

            IDataTree originalDataTree = DataTreeBuilder.CreateDocumentMappedTree(tree);
            DataTreeBuilder.AddToDataTree(originalDataTree, toMap);

            IDataTree probablyMatches = DataTreeBuilder.CreateDocumentMappedTree(tree);
            DataTreeBuilder.AddToDataTree(probablyMatches, shouldProbablyMatch);

            IDataTree notAMatch = DataTreeBuilder.CreateDocumentMappedTree(tree);
            DataTreeBuilder.AddToDataTree(notAMatch, shouldProbablyNotMatch);

            Assert.IsTrue(originalDataTree.CompareTo(originalDataTree));
            Assert.IsTrue(originalDataTree.CompareTo(probablyMatches));
            Assert.IsFalse(originalDataTree.CompareTo(notAMatch));
        }
    }
}
