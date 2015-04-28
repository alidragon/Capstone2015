using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeApi;

namespace UnitTests {
    [TestClass]
    public class BoyerMooreTests {
        const string pattern = "teammast";
        const string text1 = "welcometoteammast";
        const string text2 = "nope, don't have it";
        const string text3 = "teammastteammastteammast teammast";
        const string text4 = "some type of random text teammast which contains teammast";
        const string pattern2 = "some type";
        const string text5 = "some type of random text teammast which contains teammast";
        const string text6 = "some type of random text teammast which contains some type some type some type";

        [TestMethod]
        public void Test1() {
            Assert.AreEqual(1, text1.BoyerMooreMatchCount(pattern));
        }
        [TestMethod]
        public void Test2() {
            Assert.AreEqual(0, text2.BoyerMooreMatchCount(pattern));

        }

        [TestMethod]
        public void Test3() {
            Assert.AreEqual(4, text3.BoyerMooreMatchCount(pattern));

        }

        [TestMethod]
        public void Test4() {
            Assert.AreEqual(2, text4.BoyerMooreMatchCount(pattern));

        }

        [TestMethod]
        public void Test5() {
            Assert.AreEqual(1, text5.BoyerMooreMatchCount(pattern2));

        }

        [TestMethod]
        public void Test6() {
            Assert.AreEqual(4, text6.BoyerMooreMatchCount(pattern2));

        }
    }
}
