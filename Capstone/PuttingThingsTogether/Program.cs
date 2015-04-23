using Capstone.Tree.DataTree.Comparison;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextExtraction;
using TextExtraction.Extract;
using TextExtraction.IO;
using TreeApi.Tree;
using TreeApi.Tree.IO;

namespace PuttingThingsTogether {
    public class Program {
        static string testpath = @"G:\Data\time\";

        static void Main(string[] args) {
            Compare();
        }

        public static void Compare() {
            Console.SetBufferSize(100, 1000);
            IIO io = new FileIO();
            IEnumerable<string> file = io.ReadSourceIterable(testpath + "TIME.QUE");
            IEnumerable<string> expectedResults = io.ReadSourceIterable(testpath + "TIME.REL");
            var resultsEnum = expectedResults.GetEnumerator();
            ITextExtractor it = new BeginMarkerExtraction(file, "*FIND");

            ITreeIO tio = new TreeIO();
            IBaseTree tree = tio.LoadBaseTree(testpath + "TIME.tree");
            while (it.HasNextContent()) {
                string query = it.FindNextContent();
                Console.WriteLine("---------------------------------");
                Console.WriteLine("Query: " + Helpers.GetNameWhenFirst(query));
                query = Helpers.ConsumeName(query);

                Console.WriteLine(query);

                IDataTree queryTree = DataTreeBuilder.CreateDocumentMappedTree(tree);
                DataTreeBuilder.AddToDataTree(queryTree, query);

                Console.WriteLine("Expected Results: ");
                while(string.IsNullOrEmpty(resultsEnum.Current))
                    resultsEnum.MoveNext();
                Console.WriteLine(Helpers.ConsumeName(resultsEnum.Current));
                resultsEnum.MoveNext();

                Console.WriteLine("Actual Results: ");
                foreach(String s in Directory.EnumerateFiles(testpath + @"\datatrees")) {
                    IDataTree docTree = tio.LoadDataTree(s);
                    if (queryTree.CompareTo(docTree)) {
                        Console.Write(" " + docTree.Name);
                    }
                }
                Console.WriteLine();
            }
        }

        public static void MakeTrees() {
            IIO io = new FileIO();
            IEnumerable<string> file = io.ReadSourceIterable(testpath + "TIME.ALL");
            ITextExtractor it = new BeginMarkerExtraction(file, "*TEXT");

            ITreeIO tio = new TreeIO();
            IBaseTree tree = tio.LoadBaseTree(testpath + "TIME.tree");
            int count = 1;
            while (it.HasNextContent()) {
                string content = it.FindNextContent();
                Console.WriteLine("-----");
                string name = "" + count;
                Console.WriteLine(name);
                content = Helpers.ConsumeName(content);
                Console.WriteLine(content);

                IDataTree datatree = DataTreeBuilder.CreateDocumentMappedTree(tree);
                DataTreeBuilder.AddToDataTree(datatree, content);
                datatree.Name = name;

                tio.SaveDataTree(datatree, testpath + @"\datatrees\" + name + ".dtree");

                Console.WriteLine(datatree.MappedWords);
                count++;
            }
        }
    }
}
