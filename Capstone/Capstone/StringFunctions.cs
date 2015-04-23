using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TreeApi {
    public class StringFunctions {
        public static string Normalize(string s) {
            if (s == null) {
                return null;
            }
            s = s.ToLower();
            s.Trim();
            Regex notCharacter = new Regex("[^\\w\\s]");
            return notCharacter.Replace(s, "");
        }
    }
}
