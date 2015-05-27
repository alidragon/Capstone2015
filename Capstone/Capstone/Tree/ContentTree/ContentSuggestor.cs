using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Tree.ContentTree;
using System.Collections;
using TextExtraction;

namespace TreeApi.Tree.ContentTree {
    [Serializable]
    public class WordSuggestor {
        private Dictionary<KeyValuePair<string, Document>, WordDocumentMetadata> PossibleContent { get; set; }
        private Dictionary<string, WordMetadata> WordDocumentAppearances = new Dictionary<string,WordMetadata>();
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
                    string temp = StringFunctions.Normalize(s);

                    d.WordCount++;
                    KeyValuePair<string, Document> kvp = new KeyValuePair<string, Document>(temp, d);
                    if(!PossibleContent.ContainsKey(kvp)) {
                        PossibleContent.Add(new KeyValuePair<string, Document>(temp, d), new WordDocumentMetadata());

                        if (!WordDocumentAppearances.ContainsKey(temp)) {
                            WordDocumentAppearances.Add(temp, new WordMetadata { DocumentAppearences = 0, IDF = -1 });
                        }
                        WordDocumentAppearances[temp].DocumentAppearences++;
                        WordDocumentAppearances[temp].Docs.Add(d.Name);
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
                    string temp = StringFunctions.Normalize(s);

                    //stem here
                    temp = StringFunctions.StemmedWord(temp);

                    d.WordCount++;

                    KeyValuePair<string, Document> kvp = new KeyValuePair<string, Document>(temp, d);
                    if(!PossibleContent.ContainsKey(kvp)) {
                        PossibleContent.Add(new KeyValuePair<string, Document>(temp, d), new WordDocumentMetadata());

                        if (!WordDocumentAppearances.ContainsKey(temp)) {
                            WordDocumentAppearances.Add(temp, new WordMetadata { DocumentAppearences = 0, IDF = -1 });
                        }
                        WordDocumentAppearances[temp].DocumentAppearences++;
                        WordDocumentAppearances[temp].Docs.Add(d.Name);
                    }
                    PossibleContent[kvp].Frequency++;

                }
            }
            calculateTFIDF();
        }

        public void calculateTFIDF() {
            foreach (KeyValuePair<string, Document> kvp in PossibleContent.Keys) {
                double wordCountInDoc = PossibleContent[kvp].Frequency;
                double docsContainingWord = WordDocumentAppearances[kvp.Key].DocumentAppearences;

                double tf = wordCountInDoc / kvp.Value.WordCount;
                
                PossibleContent[kvp].TF = tf;

                if (WordDocumentAppearances[kvp.Key].IDF == -1) {
                    double idf = Math.Abs(Math.Log(documents / docsContainingWord));
                    WordDocumentAppearances[kvp.Key].IDF = idf;
                }

                PossibleContent[kvp].TFIDF = tf * WordDocumentAppearances[kvp.Key].IDF;
            }
        }

        public IEnumerable<string> Suggestions(double percentage) {
            List<string> toReturn = new List<string>();

            //select the top 20% tf-idf scores, take only the distinct ones, and put only the words into a list
            toReturn = PossibleContent.OrderByDescending(x => x.Value.TFIDF).Take((int)(PossibleContent.Count * percentage)).Select(x => x.Key.Key).Distinct().ToList();

            return toReturn;
        }

        public IEnumerable<String> WordSuggestions(string word, double percentage) {
            if (!WordDocumentAppearances.Keys.Contains(word)) {
                return null;
            }
            Console.WriteLine("Creating contains word list");
            //return words which also have high appearances in the same documents
            var containsWord = PossibleContent.Where(x => x.Key.Key.Equals(word));
            Console.WriteLine("Creating docs containing word list");
            var docsContainingWord = containsWord.OrderByDescending(x => x.Value.TFIDF).Select(x => x.Key.Value).Take(1);
            Console.WriteLine(docsContainingWord.Count());
//            var docsContainingWord = containsWord.OrderByDescending(x => x.Value.TFIDF).Select(x => x.Key.Value).Take((int)(containsWord.Count() * percentage));
            Console.WriteLine("Creating all words in docs list");
            var allWordsInDocs = PossibleContent.Where(x => docsContainingWord.Contains(x.Key.Value)).ToList();

            Console.WriteLine("Creating highest docs all list " + allWordsInDocs.Count());
            var highestDocsAll = allWordsInDocs.OrderByDescending(x => x.Value.TFIDF).Take((int)(allWordsInDocs.Count() * percentage)).Select(x => x.Key.Key);

            Console.WriteLine("Returning results");
            return highestDocsAll;
        }

        [Serializable]
        private class WordDocumentMetadata {
            public int Frequency { get; set; }
            public double TFIDF { get; set; }
            public double TF { get; set; }
        }

        private class WordMetadata {
            public double IDF { get; set; }
            public int DocumentAppearences { get; set; }

            public List<string> Docs { get; set; }

            public WordMetadata() {
                Docs = new List<string>();
            }
        }

        public IBaseTree BuildTree() {
                                                                 //change the .05?
            var orderedByIDF = WordDocumentAppearances.Where(x => x.Value.IDF > .05).OrderBy(x => x.Value.IDF).ToList();
            IBaseTree tree = new BaseTree();
            tree.Rename(tree.Root, "the");

            //Make the cutoff greater than 0, so not quite all words are added? - taken care of above?
            while (orderedByIDF.Count > 0) {
                //decide next word to add
                string word = orderedByIDF.First().Key;
                Console.WriteLine("Adding word '" + word + "' to tree");

                //find lowest branch in which all significant occurences of the word/document happen
                int numDif = Int32.MaxValue;
                Node parent = null;
                foreach(Node n in tree) { //for every branch in the tree
                    Console.WriteLine("Checking branch");
                    //first check to make sure it is completely contained
                    bool contained = !WordDocumentAppearances[word].Docs.Except(WordDocumentAppearances[n.KeyWord].Docs).Any();

                    //if it is,
                    if(contained) {
                        Console.WriteLine("Determining fit");
                        int numDifTemp = 0;
                        foreach (string s2 in WordDocumentAppearances[n.KeyWord].Docs) { //for each document the branch's word appears in
                            if(!WordDocumentAppearances[word].Docs.Contains(s2)) { // if this word doesn't appear in a document which the other does
                                numDifTemp++;
                            }
                        }
                        //if the number of different branches is less than the last node's, 
                        if(numDifTemp < numDif) {
                            Console.WriteLine("New parent: " + n.KeyWord);
                            //make this the new parent of the new word
                            numDif = numDifTemp;
                            parent = n;
                        }
                    }
                }

                Console.WriteLine("Adding " + word + " to tree");
                //add to that branch
                tree.AddNode(parent, new Node(word));
                
                orderedByIDF.Remove(orderedByIDF.First());
            }

            return tree;
        }

    }
}
