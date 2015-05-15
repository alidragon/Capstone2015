using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextExtraction {
    public static class Helpers {
        public static string GetNameWhenFirst(this string s) {
            s = s.Trim();
            string name = new string(s.Take(s.IndexOf(' ')).ToArray());
            return name;
        }

        public static string ConsumeName(this string content) {
            content = content.Trim();
            content = content.Substring(content.IndexOf(' '));
            return content;
        }
    }
}
