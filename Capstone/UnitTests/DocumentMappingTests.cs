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
        private IEnumerable<string> toMapEnumerable = new List<string>() { 
            "This is",
            "a whole series of words", 
            "which are going to be mapped ",
            "to a content tree."
        };

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

            //make sure the root has the correct number of children
            Assert.AreEqual(dataTree.Root.Children.Count, 2);

            //make sure each of the children has the correct weight
            Assert.AreEqual(dataTree.Root.Children.Where(c => c.EndPoint.Keyword == "tree").FirstOrDefault().Weight, 3);
            Assert.AreEqual(dataTree.Root.Children.Where(c => c.EndPoint.Keyword == "others").FirstOrDefault().Weight, 2);

            //make sure a branch on the content tree which is not existent in the mapped tree does not exist
            Assert.AreEqual(dataTree.Root.Children.Where(c => c.EndPoint.Keyword == "nonexistent").FirstOrDefault(), null);

            //make sure each child's connections have their own correct weight
            Assert.AreEqual(dataTree.Root.Children.Where(c => c.EndPoint.Keyword == "tree").FirstOrDefault().EndPoint.Children.Where(x => x.EndPoint.Keyword == "content").FirstOrDefault().Weight, 1);
            Assert.AreEqual(dataTree.Root.Children.Where(c => c.EndPoint.Keyword == "tree").FirstOrDefault().EndPoint.Children.Where(x => x.EndPoint.Keyword == "mapped").FirstOrDefault().Weight, 1);

            Assert.AreEqual(dataTree.Root.Children.Where(c => c.EndPoint.Keyword == "others").FirstOrDefault().EndPoint.Children.Where(x => x.EndPoint.Keyword == "series").FirstOrDefault().Weight, 1);
            Assert.AreEqual(dataTree.Root.Children.Where(c => c.EndPoint.Keyword == "others").FirstOrDefault().EndPoint.Children.Where(x => x.EndPoint.Keyword == "going").FirstOrDefault().Weight, 1);

            //make sure children connections do not cross
            Assert.AreEqual(dataTree.Root.Children.Where(c => c.EndPoint.Keyword == "others").FirstOrDefault().EndPoint.Children.Where(x => x.EndPoint.Keyword == "content").FirstOrDefault(), null);

            //make sure leaf children do not have any connections
            Assert.AreEqual(dataTree.Root.Children.Where(c => c.EndPoint.Keyword == "tree").FirstOrDefault().EndPoint.Children.Where(x => x.EndPoint.Keyword == "content").FirstOrDefault().EndPoint.Children.Count, 0);
        }

        [TestMethod]
        public void DocumentMappingTest2() {
            IBaseTree tree = setUpTree();

            IDataTree dataTree = DataTreeBuilder.CreateDocumentMappedTree(tree, toMap);

            //make sure the root has the correct number of children
            Assert.AreEqual(dataTree.Root.Children.Count, 2);

            //make sure each of the children has the correct weight
            Assert.AreEqual(dataTree.Root.Children.Where(c => c.EndPoint.Keyword == "tree").FirstOrDefault().Weight, 3);
            Assert.AreEqual(dataTree.Root.Children.Where(c => c.EndPoint.Keyword == "others").FirstOrDefault().Weight, 2);

            //make sure a branch on the content tree which is not existent in the mapped tree does not exist
            Assert.AreEqual(dataTree.Root.Children.Where(c => c.EndPoint.Keyword == "nonexistent").FirstOrDefault(), null);

            //make sure each child's connections have their own correct weight
            Assert.AreEqual(dataTree.Root.Children.Where(c => c.EndPoint.Keyword == "tree").FirstOrDefault().EndPoint.Children.Where(x => x.EndPoint.Keyword == "content").FirstOrDefault().Weight, 1);
            Assert.AreEqual(dataTree.Root.Children.Where(c => c.EndPoint.Keyword == "tree").FirstOrDefault().EndPoint.Children.Where(x => x.EndPoint.Keyword == "mapped").FirstOrDefault().Weight, 1);

            Assert.AreEqual(dataTree.Root.Children.Where(c => c.EndPoint.Keyword == "others").FirstOrDefault().EndPoint.Children.Where(x => x.EndPoint.Keyword == "series").FirstOrDefault().Weight, 1);
            Assert.AreEqual(dataTree.Root.Children.Where(c => c.EndPoint.Keyword == "others").FirstOrDefault().EndPoint.Children.Where(x => x.EndPoint.Keyword == "going").FirstOrDefault().Weight, 1);

            //make sure children connections do not cross
            Assert.AreEqual(dataTree.Root.Children.Where(c => c.EndPoint.Keyword == "others").FirstOrDefault().EndPoint.Children.Where(x => x.EndPoint.Keyword == "content").FirstOrDefault(), null);

            //make sure leaf children do not have any connections
            Assert.AreEqual(dataTree.Root.Children.Where(c => c.EndPoint.Keyword == "tree").FirstOrDefault().EndPoint.Children.Where(x => x.EndPoint.Keyword == "content").FirstOrDefault().EndPoint.Children.Count, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DocumentMapNonexistentBaseTree() {
            IDataTree dataTree = DataTreeBuilder.CreateDocumentMappedTree(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DocumentMapNonexistentBaseTree2() {
            IDataTree dataTree = DataTreeBuilder.CreateDocumentMappedTree(null, toMap);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DocumentMapNonexistentContent2() {
            IBaseTree tree = setUpTree();
            IDataTree dataTree = DataTreeBuilder.CreateDocumentMappedTree(tree, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DocumentMapNonexistentDataTree() {
            DataTreeBuilder.AddToDataTree(null, toMap);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DocumentMapNonexistentContent() {
            IBaseTree tree = setUpTree();

            IDataTree dataTree = DataTreeBuilder.CreateDocumentMappedTree(tree);
            DataTreeBuilder.AddToDataTree(dataTree, null);
        }

        [TestMethod]
        public void DocumentMapStringEnumerable() {
            IBaseTree tree = setUpTree();

            IDataTree dataTree = DataTreeBuilder.CreateDocumentMappedTree(tree);
            DataTreeBuilder.AddAllToDataTree(dataTree, toMapEnumerable);

            //make sure the root has the correct number of children
            Assert.AreEqual(dataTree.Root.Children.Count, 2);

            //make sure each of the children has the correct weight
            Assert.AreEqual(dataTree.Root.Children.Where(c => c.EndPoint.Keyword == "tree").FirstOrDefault().Weight, 3);
            Assert.AreEqual(dataTree.Root.Children.Where(c => c.EndPoint.Keyword == "others").FirstOrDefault().Weight, 2);

            //make sure a branch on the content tree which is not existent in the mapped tree does not exist
            Assert.AreEqual(dataTree.Root.Children.Where(c => c.EndPoint.Keyword == "nonexistent").FirstOrDefault(), null);

            //make sure each child's connections have their own correct weight
            Assert.AreEqual(dataTree.Root.Children.Where(c => c.EndPoint.Keyword == "tree").FirstOrDefault().EndPoint.Children.Where(x => x.EndPoint.Keyword == "content").FirstOrDefault().Weight, 1);
            Assert.AreEqual(dataTree.Root.Children.Where(c => c.EndPoint.Keyword == "tree").FirstOrDefault().EndPoint.Children.Where(x => x.EndPoint.Keyword == "mapped").FirstOrDefault().Weight, 1);

            Assert.AreEqual(dataTree.Root.Children.Where(c => c.EndPoint.Keyword == "others").FirstOrDefault().EndPoint.Children.Where(x => x.EndPoint.Keyword == "series").FirstOrDefault().Weight, 1);
            Assert.AreEqual(dataTree.Root.Children.Where(c => c.EndPoint.Keyword == "others").FirstOrDefault().EndPoint.Children.Where(x => x.EndPoint.Keyword == "going").FirstOrDefault().Weight, 1);

            //make sure children connections do not cross
            Assert.AreEqual(dataTree.Root.Children.Where(c => c.EndPoint.Keyword == "others").FirstOrDefault().EndPoint.Children.Where(x => x.EndPoint.Keyword == "content").FirstOrDefault(), null);

            //make sure leaf children do not have any connections
            Assert.AreEqual(dataTree.Root.Children.Where(c => c.EndPoint.Keyword == "tree").FirstOrDefault().EndPoint.Children.Where(x => x.EndPoint.Keyword == "content").FirstOrDefault().EndPoint.Children.Count, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DocumentMapNonexistentDataTreeEnumerable() {
            DataTreeBuilder.AddAllToDataTree(null, toMapEnumerable);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DocumentMapNonexistentContentEnumberable() {
            IBaseTree tree = setUpTree();

            IDataTree dataTree = DataTreeBuilder.CreateDocumentMappedTree(tree);
            DataTreeBuilder.AddAllToDataTree(dataTree, null);
        }

        [TestMethod]
        public void DocumentMapChangeBaseTree() {
            IBaseTree tree = setUpTree();

            IDataTree dataTree = DataTreeBuilder.CreateDocumentMappedTree(tree);
            DataTreeBuilder.AddAllToDataTree(dataTree, toMapEnumerable);

            dataTree.SetBaseTree(null);
            Assert.AreEqual(dataTree.GetBaseTree(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void DocumentMapNullBaseTree() {
            IBaseTree tree = setUpTree();

            IDataTree dataTree = DataTreeBuilder.CreateDocumentMappedTree(tree);
            DataTreeBuilder.AddAllToDataTree(dataTree, toMapEnumerable);

            dataTree.SetBaseTree(null);
            Assert.AreEqual(dataTree.GetBaseTree(), null);

            DataTreeBuilder.AddToDataTree(dataTree, toMap);
        }
        
    }
}
