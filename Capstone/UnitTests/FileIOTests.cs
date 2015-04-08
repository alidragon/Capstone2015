
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextExtraction.IO;

namespace UnitTests {
    [TestClass]
    public class FileIOTests {
        string testpath = "G:\\Blogs\\blogs\\5114.male.25.indUnk.Scorpio.xml";

        [TestMethod]
        public void ReadFileTest() {
            IIO io = new FileIO();
            string result = io.ReadSource(testpath);
            Assert.IsFalse(string.IsNullOrEmpty(result));
            Assert.AreEqual(File.ReadAllText(testpath), result);
            Console.WriteLine(result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ReadNullSource() {
            IIO io = new FileIO();
            string result = io.ReadSource(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ReadSourceNonString() {
            IIO io = new FileIO();
            string result = io.ReadSource(new object());
        }
    }
}
