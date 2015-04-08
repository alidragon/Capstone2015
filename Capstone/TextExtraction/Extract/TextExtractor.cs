using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextExtraction.Extract {
    public class TextExtractor : ITextExtractor {
        private string startTag;
        private string endTag;
        private string tag;
        public string EnclosingTag {
            get {
                return tag;
            }
            set {
                startTag = "<" + value + ">";
                endTag = "</" + value + ">";
                tag = value;
            }
        }
        private string AllContent { get; set; }

        private int position = 0;
        public TextExtractor(string content) {
            if (content == null) {
                throw new ArgumentNullException("Content cannot be null.");
            }
            AllContent = content;
        }

        public TextExtractor(string content, string tag) {
            if (content == null) {
                throw new ArgumentNullException("Content cannot be null.");
            } else if (string.IsNullOrEmpty(tag)) {
                throw new ArgumentNullException("Tag may not be null or empty");
            }
            AllContent = content;
            EnclosingTag = tag;
        }

        public string FindNextContent() {
            if (AllContent == null || string.IsNullOrEmpty(tag)) {
                throw new InvalidOperationException("Cannot have null content or empty tag");
            }
            int nextPos = AllContent.IndexOf(startTag, position) + startTag.Length;
            int endPos = AllContent.IndexOf(endTag, position);
            string toReturn;
            if (nextPos == -1 || endPos == -1) {
                toReturn = null;
            } else {
                toReturn = AllContent.Substring(nextPos, endPos - nextPos);
                position = endPos + endTag.Length;
            }
            return toReturn == null ? toReturn : toReturn.Trim();
        }

        public bool HasNextContent() {
            int nextPos = AllContent.IndexOf(startTag, position) + startTag.Length;
            int endPos = AllContent.IndexOf(endTag, position);
            return nextPos != -1 && endPos != -1;
        }
    }
}
