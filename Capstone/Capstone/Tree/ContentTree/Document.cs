using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Capstone.Tree.ContentTree {
    [Serializable]
    public class Document {
        public string Name { get; set; }
        public int WordCount { get; set; }

        public override bool Equals(object obj) {
            return this.Name.Equals((obj as Document).Name);
        }

        public override int GetHashCode() {
            return Name.GetHashCode();
        }
    }
}
