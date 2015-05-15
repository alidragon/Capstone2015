using Iveonik.Stemmers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TreeApi {
    public static class StringFunctions {
        public static string Normalize(string s) {
            if (s == null) {
                return null;
            }
            s = s.ToLower();
            Regex notCharacter = new Regex(@"[^\w\s]");
            s = notCharacter.Replace(s, "");
            return s.Trim();
        }

        public static int BoyerMooreMatchCount(this string one, string pattern) {
            int count = 0;
            int position = pattern.Length - 1;
            pattern = pattern.Trim();
            Dictionary<char, int> table = MakeBoyerMooreTable(pattern);
            while (position < one.Length) {
                bool cont = true;
                for(int i = 0; i < pattern.Length && cont; i++) {
                    if (one[position - i] != pattern[pattern.Length - i - 1]) {
                        if (table.ContainsKey(one[position])) {
                            position += table[one[position]];
                        } else {
                            position += pattern.Length;
                        }
                        cont = false;
                    } else if (i == pattern.Length - 1) {
                        count++;
                        position += pattern.Length;
                        cont = false;
                    }
                }
            }

            return count;
        }


        private static Dictionary<char, int> MakeBoyerMooreTable(string pattern) {
            Dictionary<char, int> toReturn = new Dictionary<char, int>();
            for (int i = 0; i < pattern.Length - 1; i++) {
                toReturn[pattern[i]] = pattern.Length - i - 1;
            }
            toReturn[pattern[pattern.Length - 1]] = pattern.Length;
            return toReturn;
        }

        public static string StemmedWord(string s) {
            IStemmer stemmer = new EnglishStemmer();
            string[] words = s.Split(' ');
            StringBuilder sb = new StringBuilder();
            foreach (string word in words) {
                sb.Append(stemmer.Stem(word));
                sb.Append(" ");
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
    }
}
