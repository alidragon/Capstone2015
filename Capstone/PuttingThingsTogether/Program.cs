using Capstone.Tree.DataTree.Comparison;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TextExtraction;
using TextExtraction.Extract;
using TextExtraction.IO;
using TreeApi.Tree;
using TreeApi.Tree.IO;
using TreeApi.Tree.DataTree;
using TreeApi.Tree.ContentTree;

namespace PuttingThingsTogether {
    public class Program {
        static string testpath = @"G:\Data\time\";

        static void Main(string[] args) {
            //RunComparison("TreeV5.tree");
            //MakeTrees("flatTree.tree");
            //getDocsForQuery("TreeV4.tree");
            //CompareAll();
            TestSuggestions();
        }

        public static void RunComparison(string contentTreeName) {
            MakeTrees(contentTreeName);
            Compare(contentTreeName);
        }

        public static void Compare(string contentTreeName) {
            Console.SetBufferSize(100, 2000);
            IIO io = new FileIO();
            IEnumerable<string> file = io.ReadSourceIterable(testpath + "TIME.QUE");
            IEnumerable<string> expectedResults = io.ReadSourceIterable(testpath + "TIME.REL");
            var resultsEnum = expectedResults.GetEnumerator();
            ITextExtractor it = new BeginMarkerExtraction(file, "*FIND");

            ITreeIO tio = new TreeIO();
            IBaseTree tree = tio.LoadBaseTree(testpath + contentTreeName);
            double totalRecall = 0;
            double totalPrecision = 0;
            double bestRecall = -1;
            double worstRecall = 2;
            double bestPrecision = -1;
            double worstPrecision = 2;
            double count = 0;
            while (it.HasNextContent()) {
                string query = it.FindNextContent();
                Console.WriteLine("---------------------------------");
                string queryName = Helpers.GetNameWhenFirst(query);
                Console.WriteLine("Query: " + queryName);
                query = Helpers.ConsumeName(query);

                Console.WriteLine(query);

                IDataTree queryTree = DataTreeBuilder.CreateStemmedDocumentMapTree(tree);
                DataTreeBuilder.AddToDataTreeBoyerMoore(queryTree, query);

                Console.WriteLine("Expected Results: ");
                while(string.IsNullOrEmpty(resultsEnum.Current))
                    resultsEnum.MoveNext();
                string expected = Helpers.ConsumeName(resultsEnum.Current);
                Console.WriteLine(expected);
                resultsEnum.MoveNext();

                expected = expected.Trim();
                string[] expectedArray = expected.Split(' ');
                double relevant = 0;
                double totalRetrieved = 0;

                Console.WriteLine("Actual Results: ");
                foreach(String s in Directory.EnumerateFiles(testpath + @"\datatrees")) {
                    IDataTree docTree = tio.LoadDataTree(s);
                    if (queryTree.CompareTo(docTree)) {
                        Console.Write(" " + docTree.Name);
                        totalRetrieved++;
                        if (expectedArray.Contains(docTree.Name)) {
                            relevant++;
                        }
                    }
                }
                Console.WriteLine();
                Console.WriteLine("Precision: " + relevant + "/" + totalRetrieved );
                Console.WriteLine("Recall: " + relevant + "/" + (expectedArray.Length));
                Console.WriteLine();

                count++;
                double recall = relevant / expectedArray.Length;
                double precision = 0;
                if (totalRetrieved > 0) {
                    precision = relevant / totalRetrieved;
                }
                totalPrecision += precision;
                totalRecall += recall;

                if (precision > bestPrecision) {
                    bestPrecision = precision;
                }
                if (precision < worstPrecision) {
                    worstPrecision = precision;
                }

                if(recall > bestRecall) {
                    bestRecall = recall;
                }
                if(recall < worstRecall) {
                    worstRecall = recall;
                }
            }

            Console.WriteLine("-------------------");
            Console.WriteLine("Average Precision: " + totalPrecision / count);
            Console.WriteLine("Average Recall: " + totalRecall / count);
            Console.WriteLine("Worst Precision: " + worstPrecision);
            Console.WriteLine("Worst Recall: " + worstRecall);
            Console.WriteLine("Best Precision: " + bestPrecision);
            Console.WriteLine("Best Recall: " + bestRecall);
        }

        public static void MakeTrees(string contentTreeName) {
            IIO io = new FileIO();
            IEnumerable<string> file = io.ReadSourceIterable(testpath + "TIME.ALL");
            ITextExtractor it = new BeginMarkerExtraction(file, "*TEXT");

            ITreeIO tio = new TreeIO();
            IBaseTree tree = tio.LoadBaseTree(testpath + contentTreeName);
            int count = 1;
            while (it.HasNextContent()) {
                string content = it.FindNextContent();
                //Console.WriteLine("-----");
                string name = "" + count;
                //Console.WriteLine(name);
                content = Helpers.ConsumeName(content);
                //Console.WriteLine(content);

                IDataTree datatree = DataTreeBuilder.CreateStemmedDocumentMapTree(tree);
                DataTreeBuilder.AddToDataTreeBoyerMoore(datatree, content);
                datatree.Name = name;

                tio.SaveDataTree(datatree, testpath + @"\datatrees\" + name + ".dtree");

                //Console.WriteLine(datatree.MappedWords);
                count++;
            }
        }

        public static void getDocsForQuery(string contentTreeName) {
            Console.SetBufferSize(100, 2000);
            IIO io = new FileIO();
            IEnumerable<string> file = io.ReadSourceIterable(testpath + "TIME.QUE");
            IEnumerable<string> expectedResults = io.ReadSourceIterable(testpath + "TIME.REL");
            var resultsEnum = expectedResults.GetEnumerator();
            ITextExtractor it = new BeginMarkerExtraction(file, "*FIND");

            ITreeIO tio = new TreeIO();
            IBaseTree tree = tio.LoadBaseTree(testpath + contentTreeName);

            
            
            string query = it.FindNextContent();
            Console.WriteLine("---------------------------------");
            string queryName = Helpers.GetNameWhenFirst(query);
            Console.WriteLine("Query: " + queryName);
            query = Helpers.ConsumeName(query);

            Console.WriteLine(query);

            IDataTree queryTree = DataTreeBuilder.CreateStemmedDocumentMapTree(tree);
            DataTreeBuilder.AddToDataTreeBoyerMoore(queryTree, query);
            queryTree.PrintDataTree();

            Console.WriteLine("Expected Results: ");
            while (string.IsNullOrEmpty(resultsEnum.Current))
                resultsEnum.MoveNext();
            string expected = Helpers.ConsumeName(resultsEnum.Current);
            Console.WriteLine(expected);
            resultsEnum.MoveNext();

            expected = expected.Trim();
            string[] expectedArray = expected.Split(' ');
            double relevant = 0;
            double totalRetrieved = 0;

            Console.WriteLine("Actual Results: ");
            List<string> retrieved = new List<string>();
            foreach (String s in Directory.EnumerateFiles(testpath + @"\datatrees")) {
                IDataTree docTree = tio.LoadDataTree(s);
                if (queryTree.CompareTo(docTree)) {
                    Console.Write(" " + docTree.Name);
                    retrieved.Add(docTree.Name);
                    totalRetrieved++;
                    if (expectedArray.Contains(docTree.Name)) {
                        relevant++;
                    }
                }
                if (expectedArray.Contains(docTree.Name)) {
                    Console.WriteLine("---");
                    Console.WriteLine(docTree.Name);
                    docTree.PrintDataTree();
                    Console.WriteLine("---");
                }

            }

            Console.WriteLine();
            Console.WriteLine("Precision: " + relevant + "/" + totalRetrieved);
            Console.WriteLine("Recall: " + relevant + "/" + (expectedArray.Length));
            Console.WriteLine();

            Console.WriteLine("---------------------------------");
            Thread.Sleep(10000);

            IEnumerable<string> fileAll = io.ReadSourceIterable(testpath + "TIME.ALL");
            ITextExtractor itAll = new BeginMarkerExtraction(fileAll, "*TEXT");

            int count = 1;
            while (itAll.HasNextContent()) {
                string content = itAll.FindNextContent();
                string name = "" + count;
                if (retrieved.Contains(name) || expectedArray.ToList().Contains(name)) {
                    Console.WriteLine("------------------------------------------------------------");
                    Console.WriteLine("------------------------------------------------------------");
                    Console.WriteLine(name);
                    content = Helpers.ConsumeName(content);
                    Console.WriteLine(content);
                    Console.WriteLine("------------------------------------------------------------");
                    Console.WriteLine("------------------------------------------------------------");
                }

                count++;
            }
        }


        public static void CompareAll() {
            Console.SetBufferSize(100, 2000);
            IIO io = new FileIO();
            IEnumerable<string> expectedResults = io.ReadSourceIterable(testpath + "TIME.REL");

            List<IEnumerable<string>> resPerLine = new List<IEnumerable<string>>();
            foreach (string s in expectedResults) {
                if (!string.IsNullOrEmpty(s)) {
                    string answers = Helpers.ConsumeName(s);
                    resPerLine.Add(answers.Split(' '));
                }
            }

            double avgPrecision = 0;
            double avgRecall = 0;
            int numCounted = 0;

            TreeIO tio = new TreeIO();
            foreach (String s in Directory.EnumerateFiles(testpath + @"\datatrees")) {
                IDataTree docTree = tio.LoadDataTree(s);
                Console.WriteLine(docTree.Name + ":");
                int count = 0;
                List<string> matches = new List<string>();
                foreach (String s2 in Directory.EnumerateFiles(testpath + @"\datatrees")) {
                    if(s != s2) {
                        IDataTree tree2 = tio.LoadDataTree(s2);
                        if (docTree.CompareTo(tree2)) {
                            Console.Write(tree2.Name + " ");
                            count++;
                            matches.Add(tree2.Name);
                        }
                    }
                }

                Console.WriteLine();
                Console.WriteLine("Count: " + count);

                IEnumerable<IEnumerable<string>> containsTreeRes = resPerLine.Where(l => l.Contains(docTree.Name));
                if (containsTreeRes.Count() > 0) {
                    List<string> expectedMatches = new List<string>();
                    foreach (IEnumerable<string> list in containsTreeRes) {
                        foreach (string m in list) {
                            expectedMatches.Add(m);
                        }
                    }
                    expectedMatches = expectedMatches.Distinct().ToList();

                    IEnumerable<string> foundExpected = matches.Intersect(expectedMatches);

                    Console.WriteLine("Precision: " + foundExpected.Count() + "/" + matches.Count);
                    Console.WriteLine("Recall: " + foundExpected.Count() + "/" + expectedMatches.Count);

                    numCounted++;
                    if (matches.Count > 0) {
                        avgPrecision += ((double)foundExpected.Count()) / matches.Count;
                    }
                    avgRecall += ((double)foundExpected.Count()) / expectedMatches.Count;
                }

                Console.WriteLine();
            }

            Console.WriteLine("-------");
            Console.WriteLine("Average Precision: " + (avgPrecision / numCounted));
            Console.WriteLine("Average Recall: " + (avgRecall / numCounted));
        }


        public static void TestSuggestions() {
            IIO io = new FileIO();
            IEnumerable<string> file = io.ReadSourceIterable(testpath + "TIME.ALL");
            ITextExtractor it = new BeginMarkerExtraction(file, "*TEXT");

            WordSuggestor ws = new WordSuggestor();
            ws.AddAllStemmed(it);
            var words = ws.Suggestions(.2);
            foreach (string s in words) {
                Console.WriteLine(s);
            }
        }
    }
}
