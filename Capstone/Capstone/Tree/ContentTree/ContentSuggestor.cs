using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Tree.ContentTree;
using System.Collections;
using TextExtraction;

namespace TreeApi.Tree.ContentTree {
    public class WordSuggestor {
        private Dictionary<KeyValuePair<string, Document>, WordDocumentMetadata> PossibleContent { get; set; }
        private Dictionary<string, int> WordDocumentAppearances = new Dictionary<string,int>();
        private int documents = 0;

        public WordSuggestor() {
            PossibleContent = new Dictionary<KeyValuePair<string, Document>, WordDocumentMetadata>();
        }

        public void addAll(IEnumerator<string> it) {
            while (it.MoveNext()) {
                string current = it.Current;
                Document d = new Document() { Name = current.GetNameWhenFirst() };
                documents++;
                current = current.ConsumeName();

                string[] words = current.Split(' ');
                foreach (string s in words) {
                    string temp = s.Normalize();

                    d.WordCount++;
                    KeyValuePair<string, Document> kvp = new KeyValuePair<string, Document>(temp, d);
                    if(!PossibleContent.ContainsKey(kvp)) {
                        PossibleContent.Add(new KeyValuePair<string, Document>(temp, d), new WordDocumentMetadata());

                        if (!WordDocumentAppearances.ContainsKey(temp)) {
                            WordDocumentAppearances.Add(temp, 0);
                        }
                        WordDocumentAppearances[temp]++;
                    }
                    PossibleContent[kvp].Frequency++;
                }
            }
            calculateTFIDF();
        }

        public void AddAllStemmed(IEnumerator<string> it) {
            while (it.MoveNext()) {
                string current = it.Current;
                Document d = new Document() { Name = current.GetNameWhenFirst() };
                documents++;
                current = current.ConsumeName();

                string[] words = current.Split(' ');
                foreach (string s in words) {
                    string temp = s.Normalize();

                    //stem here
                    temp = StringFunctions.StemmedWord(temp);

                    d.WordCount++;

                    KeyValuePair<string, Document> kvp = new KeyValuePair<string, Document>(temp, d);
                    if(!PossibleContent.ContainsKey(kvp)) {
                        PossibleContent.Add(new KeyValuePair<string, Document>(temp, d), new WordDocumentMetadata());

                        if (!WordDocumentAppearances.ContainsKey(temp)) {
                            WordDocumentAppearances.Add(temp, 0);
                        }
                        WordDocumentAppearances[temp]++;
                    }
                    PossibleContent[kvp].Frequency++;

                }
            }
            calculateTFIDF();
        }

        public void calculateTFIDF() {
            foreach (KeyValuePair<string, Document> kvp in PossibleContent.Keys) {
                double wordCountInDoc = PossibleContent[kvp].Frequency;
                double docsContainingWord = WordDocumentAppearances[kvp.Key];

                double tf = wordCountInDoc / kvp.Value.WordCount;
                double idf = Math.Log(documents / docsContainingWord);

                PossibleContent[kvp].TFIDF = tf * idf;
            }
        }

        public IEnumerable<string> Suggestions(double percentage) {
            List<string> toReturn = new List<string>();

            //select the top 20% tf-idf scores, take only the distinct ones, and put only the words into a list
            toReturn = PossibleContent.OrderByDescending(x => x.Value.TFIDF).Take((int)(PossibleContent.Count * percentage)).Select(x => x.Key.Key).Distinct().ToList();

            return toReturn;
        }

        private class WordDocumentMetadata {
            public int Frequency { get; set; }
            public double TFIDF { get; set; }
        }

    }
}
