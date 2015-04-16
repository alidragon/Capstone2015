using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TreeApi.Tree;
using TreeApi.Tree.IO;

namespace UnitTests {
    [TestClass]
    public class TreeIOTests {
        private string location = @"test.tst";
        private string toMap = "This is a whole series of words which are going to be mapped to a content tree.";

        public IBaseTree setUpBaseTree() {
            IBaseTree tree = new BaseTree();
            tree.AddWord((string)null, "Root");
            tree.AddWord("root", "tree");
            tree.AddWord("root", "Others");
            tree.AddWord("tree", "content");
            tree.AddWord("tree", "mapped");
            tree.AddWord("others", "series");
            tree.AddWord("others", "going");
            tree.AddWord("root", "nonexistent");

            return tree;
        }

        public IDataTree setUpDataTree(IBaseTree baseTree) {
            IDataTree data = DataTreeBuilder.CreateDocumentMappedTree(baseTree);
            DataTreeBuilder.AddToDataTree(data, toMap);
            return data;
        }

        [TestMethod]
        public void SaveBaseTreeTest() {
            ITreeIO io = new TreeIO();
            IBaseTree basetree = setUpBaseTree();

            io.SaveBaseTree(basetree, location);

            Assert.IsTrue(File.Exists(location));

            IBaseTree loadedBasetree = io.LoadBaseTree(location);

            Assert.AreNotSame(basetree, loadedBasetree);
            Assert.AreEqual(basetree.Root.KeyWord, loadedBasetree.Root.KeyWord);
        }

        [TestMethod]
        public void SaveDataTreeTest() {
            ITreeIO io = new TreeIO();
            IBaseTree basetree = setUpBaseTree();
            IDataTree datatree = setUpDataTree(basetree);

            io.SaveDataTree(datatree, location);

            Assert.IsTrue(File.Exists(location));

            IDataTree loadedDatatree = io.LoadDataTree(location);

            Assert.AreNotSame(basetree, loadedDatatree);
            Assert.AreEqual(basetree.Root.KeyWord, loadedDatatree.Root.Keyword);

            Assert.IsNull(loadedDatatree.GetBaseTree());
            Assert.IsNotNull(datatree.GetBaseTree());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidFileException))]
        public void LoadInvalidFileTest() {
            ITreeIO io = new TreeIO();
            IBaseTree basetree = setUpBaseTree();
            IDataTree datatree = setUpDataTree(basetree);

            io.SaveBaseTree(basetree, location);

            Assert.IsTrue(File.Exists(location));

            IDataTree loadedDatatree = io.LoadDataTree(location);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidFileException))]
        public void LoadInvalidFileTest2() {
            ITreeIO io = new TreeIO();
            IBaseTree basetree = setUpBaseTree();
            IDataTree datatree = setUpDataTree(basetree);

            io.SaveDataTree(datatree, location);

            Assert.IsTrue(File.Exists(location));

            IBaseTree loadedDatatree = io.LoadBaseTree(location);
        }
    }
}
