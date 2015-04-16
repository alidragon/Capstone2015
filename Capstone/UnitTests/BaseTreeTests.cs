using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TreeApi.Tree;
using System.Collections.Generic;

namespace UnitTests {
    [TestClass]
    public class BaseTreeTests {
        [TestMethod]
        public void AddNodesTest() {
            IBaseTree tree = new BaseTree();

            Assert.IsNull(tree.Root);
            Assert.IsFalse(tree.Contains("all"));

            tree.AddNode(null, new Node("All"));

            Assert.IsNotNull(tree.Root);
            Assert.AreEqual("all", tree.Root.KeyWord);
            Assert.IsTrue(tree.Contains("all"));
            Assert.AreEqual("all", tree.GetNode("all").KeyWord);
        }

        [TestMethod]
        public void AddWordTest() {
            IBaseTree tree = new BaseTree();

            Assert.IsNull(tree.Root);
            Assert.IsFalse(tree.Contains("all"));

            tree.AddWord((string)null, "all");

            Assert.IsNotNull(tree.Root);
            Assert.AreEqual("all", tree.Root.KeyWord);
            Assert.IsTrue(tree.Contains("all"));
            Assert.AreEqual("all", tree.GetNode("all").KeyWord);
        }

        [TestMethod]
        public void AddWordNodeParentTest() {
            IBaseTree tree = new BaseTree();

            Assert.IsNull(tree.Root);
            Assert.IsFalse(tree.Contains("all"));

            tree.AddWord((Node)null, "all");

            Assert.IsNotNull(tree.Root);
            Assert.AreEqual("all", tree.Root.KeyWord);
            Assert.IsTrue(tree.Contains("all"));
            Assert.AreEqual("all", tree.GetNode("all").KeyWord);
        }

        [TestMethod]
        public void AddParentedNodesTest() {
            IBaseTree tree = new BaseTree();

            String root = "world";
            String child = "us";

            tree.AddWord((string)null, root);
            tree.AddWord(root, child);

            Assert.IsTrue(tree.Contains(root));
            Assert.IsTrue(tree.Contains(child));
            Assert.AreEqual(root, tree.Root.KeyWord);
            Assert.AreEqual(root, tree.GetNode(root).KeyWord);
            Assert.AreEqual(child, tree.GetNode(child).KeyWord);
            Assert.AreEqual(root, tree.GetNode(child).Parent.KeyWord);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void GetNonexistentNodeTest() {
            IBaseTree tree = new BaseTree();
            tree.GetNode("Anything");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddNodeToNonexistentParentTest() {
            IBaseTree tree = new BaseTree();

            tree.AddWord("World", "US");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddNodeToNonexistentParentNodeTest() {
            IBaseTree tree = new BaseTree();

            tree.AddWord(new Node("World"), "US");
        }

        [TestMethod]
        public void RemoveNodeTest() {
            IBaseTree tree = new BaseTree();

            String root = "World";
            String child = "US";

            tree.AddWord((string)null, root);
            tree.AddWord(root, child);

            Assert.IsTrue(tree.Contains(root));
            Assert.IsTrue(tree.Contains(child));

            tree.RemoveNode(tree.GetNode(child));

            Assert.IsTrue(tree.Contains(root));
            Assert.IsFalse(tree.Contains(child));

            try {
                tree.GetNode(child);
                Assert.Fail();
            } catch (KeyNotFoundException) {

            }
        }

        [TestMethod]
        public void RemoveTieredNodeTest() {
            IBaseTree tree = new BaseTree();

            String root = "World";
            String child = "US";

            tree.AddWord((string)null, root);
            tree.AddWord(root, child);

            Assert.IsTrue(tree.Contains(root));
            Assert.IsTrue(tree.Contains(child));

            tree.RemoveNode(tree.GetNode(root));

            Assert.IsFalse(tree.Contains(root));
            Assert.IsFalse(tree.Contains(child));

            try {
                tree.GetNode(child);
                Assert.Fail();
            } catch (KeyNotFoundException) {

            }

            try {
                tree.GetNode(root);
                Assert.Fail();
            } catch (KeyNotFoundException) {

            }
        }

        [TestMethod]
        public void RemoveTieredNodeWithMultipleChildrenTest() {
            IBaseTree tree = new BaseTree();

            String root = "World";
            String child = "US";
            String child2 = "UK";
            String child3 = "France";
            String child4 = "Australia";
            String childt2 = "Colorado";
            String childt22 = "Utah";
            String childt23 = "California";

            tree.AddWord((string)null, root);
            tree.AddWord(root, child);
            tree.AddWord(root, child2);
            tree.AddWord(root, child3);
            tree.AddWord(root, child4);
            tree.AddWord(child, childt2);
            tree.AddWord(child, childt22);
            tree.AddWord(child, childt23);

            Assert.IsTrue(tree.Contains(root));
            Assert.IsTrue(tree.Contains(child));
            Assert.IsTrue(tree.Contains(child2));
            Assert.IsTrue(tree.Contains(child3));
            Assert.IsTrue(tree.Contains(child4));
            Assert.IsTrue(tree.Contains(childt2));
            Assert.IsTrue(tree.Contains(childt22));
            Assert.IsTrue(tree.Contains(childt23));

            tree.RemoveNode(tree.GetNode(root));

            Assert.IsFalse(tree.Contains(root));
            Assert.IsFalse(tree.Contains(child));
            Assert.IsFalse(tree.Contains(child2));
            Assert.IsFalse(tree.Contains(child3));
            Assert.IsFalse(tree.Contains(child4));
            Assert.IsFalse(tree.Contains(childt2));
            Assert.IsFalse(tree.Contains(childt22));
            Assert.IsFalse(tree.Contains(childt23));

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RemoveNonexistentNode() {
            IBaseTree tree = new BaseTree();

            tree.RemoveNode(new Node("hi"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RemoveNullTest() {
            IBaseTree tree = new BaseTree();

            tree.RemoveNode(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddDuplicateTest() {
            IBaseTree tree = new BaseTree();

            String root = "World";

            tree.AddWord((string)null, root);
            tree.AddWord(root, root);
        }

        [TestMethod]
        public void GetEnumeratorTest() {
            IBaseTree tree = new BaseTree();

            String root = "World";

            tree.AddWord((string)null, root);

            IEnumerator<string> e = (IEnumerator<string>)tree.GetEnumerator();
            Assert.IsNotNull(e);
            e.MoveNext();
            Assert.IsNotNull(e.Current);
        }

        [TestMethod]
        public void RenameTest() {
            IBaseTree tree = new BaseTree();

            string root = "all";
            string different = "different";

            tree.AddWord((string)null, root);
            Assert.AreEqual(root, tree.Root.KeyWord);

            tree.Rename(root, different);

            Assert.AreEqual(tree.Root.KeyWord, different);
        }
    }
}
