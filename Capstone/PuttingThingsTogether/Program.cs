using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextExtraction.Extract;
using TextExtraction.IO;
using TreeApi.Tree;
using TreeApi.Tree.IO;

namespace PuttingThingsTogether {
    public class Program {
        static string testpath = @"C:\Users\Sean Palmer\Documents\Capstone\Capstone2015\5114.male.25.indUnk.Scorpio.xml";

        static void Main(string[] args) {
            IIO io = new FileIO();
            string file = io.ReadSource(testpath);
            ITextExtractor it = new TextExtractor(file, "post");
            //while (it.HasNextContent()) {
            //    Console.WriteLine("-------------------NEW POST-----------------------");
            //    Console.WriteLine(it.FindNextContent());
            //}

            ITreeIO tio = new TreeIO();
            IBaseTree tree = tio.LoadBaseTree(testpath + ".tree");
            while (it.HasNextContent()) {
                string content = it.FindNextContent();
                if (content.Length > 200) {
                    Console.WriteLine();
                    Console.WriteLine(content);

                    IDataTree datatree = DataTreeBuilder.CreateDocumentMappedTree(tree);
                    DataTreeBuilder.AddToDataTree(datatree, content);

                    Console.WriteLine(datatree.Words);
                }
            }

            
        }
    }
}
