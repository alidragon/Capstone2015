using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextExtraction.Extract;

namespace UnitTests {
    [TestClass]
    public class TextExtractorTests {

        private string testString1 = "<Blog><date>28,February,2001</date><post>\n\nSlashdot raises lots of  urlLink interesting thoughts about banner ads .  The idea is to let users control the ad delivery, and even to allow users to comment on ads.\n\n</post>" +
                                              "<date>27,February,2001</date><post>\n\nurlLink  The Merchants of Cool  , a Frontline documentary featuring Mindjack advisory board member Douglas Rushkoff, is on PBS tonight.  Check your local listings for the time.\n\n</post>" +
                                              "<date>26,February,2001</date><post>\n\nurlLink ATMs dispensing music?   I don't quite see the logic in that.  I'm not entirely against paying a nominal fee for music, or any other media, but if I do have to pay for it I'd be much more likely to buy stuff from my own PC.\n\n</post></Blog>";

        private string[] content = new string[] {
            "Slashdot raises lots of  urlLink interesting thoughts about banner ads .  The idea is to let users control the ad delivery, and even to allow users to comment on ads.",
            "urlLink  The Merchants of Cool  , a Frontline documentary featuring Mindjack advisory board member Douglas Rushkoff, is on PBS tonight.  Check your local listings for the time.",
            "urlLink ATMs dispensing music?   I don't quite see the logic in that.  I'm not entirely against paying a nominal fee for music, or any other media, but if I do have to pay for it I'd be much more likely to buy stuff from my own PC.",
        };

        private string tag = "post";

        private string tag2 = "date";

        private string[] tag2Content = new string[] {
            "28,February,2001",
            "27,February,2001",
            "26,February,2001"
        };

        private string nonexistentTag = "something";

        [TestMethod]
        public void TextExtractionTest() {
            ITextExtractor t = new TextExtractor(testString1, tag);
            Assert.IsTrue(t.HasNextContent());
            Assert.IsNotNull(t.EnclosingTag);

            for (int i = 0; i < 3; i++) {
                Assert.IsTrue(t.HasNextContent());
                string post = t.FindNextContent();
                Assert.AreEqual(content[i], post);
            }

            Assert.IsFalse(t.HasNextContent());
            Assert.IsNull(t.FindNextContent());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullContentTest() {
            ITextExtractor t = new TextExtractor(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullContentTestWithTagOperator() {
            ITextExtractor t = new TextExtractor(null, tag);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullTagTest() {
            ITextExtractor t = new TextExtractor(testString1, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EmptyTagTest() {
            ITextExtractor t = new TextExtractor(testString1, "");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void NullTagAssignedLaterTest() {
            ITextExtractor t = new TextExtractor(testString1);
            t.EnclosingTag = null;
            t.FindNextContent();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EmptyTagAssignedLaterTest() {
            ITextExtractor t = new TextExtractor(testString1);
            t.EnclosingTag = "";
            t.FindNextContent();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void NullTagAssignedLaterSecondConstTest() {
            ITextExtractor t = new TextExtractor(testString1, tag);
            t.EnclosingTag = null;
            t.FindNextContent();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EmptyTagAssignedLaterSecondConstTest() {
            ITextExtractor t = new TextExtractor(testString1, tag);
            t.EnclosingTag = "";
            t.FindNextContent();
        }

        [TestMethod]
        public void TagSwitchTest() {
            ITextExtractor t = new TextExtractor(testString1, tag);
            Assert.IsTrue(t.HasNextContent());
            Assert.IsNotNull(t.EnclosingTag);
            t.FindNextContent();
            t.EnclosingTag = tag2;

            for (int i = 1; i < 3; i++) {
                Assert.IsTrue(t.HasNextContent());
                string post = t.FindNextContent();
                Assert.AreEqual(tag2Content[i], post);
            }

            Assert.IsFalse(t.HasNextContent());
            Assert.IsNull(t.FindNextContent());
        }

        [TestMethod]
        public void NonexistentTagTest() {
            ITextExtractor t = new TextExtractor(testString1, nonexistentTag);
            Assert.IsFalse(t.HasNextContent());
            Assert.IsNull(t.FindNextContent());
        }
    }
}
