using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextExtraction.Extract {
    public class BeginMarkerExtraction : ITextExtractor, IEnumerator {
        private string tag;
        private IEnumerable<string> strings;
        private IEnumerator<string> content;

        public string Tag {
            get {
                return tag;
            }
            set {
                tag = value;
            }
        }

        public BeginMarkerExtraction(IEnumerable<string> content, string tag = null) {
            Construct(content, tag);
        }

        public BeginMarkerExtraction(string content, string tag = null) {
            Construct(new List<string> { content }, tag);
        }

        private void Construct(IEnumerable<string> content, string tag = null) {
            strings = content;
            this.Tag = tag;
            this.content = strings.GetEnumerator();
        }

        public string FindNextContent() {
            StringBuilder s = new StringBuilder();
            if (!HasNextContent()) {
                return null;
            }
            s.Append(content.Current.Substring(content.Current.IndexOf(tag) + tag.Length));
            while (content.MoveNext() && !content.Current.Contains(tag)) {
                s.Append("\n");
                s.Append(content.Current);
            }
            if (content.Current != null) {
                s.Append("\n");
                s.Append(content.Current.Split(tag.ToCharArray())[0]);
            }
            return s.ToString();
        }

        public bool HasNextContent() {
            bool found = false;
            if(content.Current == null) {
                if (!content.MoveNext()) {
                    return false;
                }
            }
            do {
                if (content.Current.Contains(tag)) {
                    found = true;
                }
            } while (!found && content.MoveNext());
            return found;
        }

        public object Current {
            get;
            set;
        }

        public bool MoveNext() {
            Current = FindNextContent();
            return Current == null;
        }

        public void Reset() {
            throw new NotImplementedException();
        }
    }
}
