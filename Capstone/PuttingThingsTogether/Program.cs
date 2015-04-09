using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextExtraction.Extract;
using TextExtraction.IO;

namespace PuttingThingsTogether {
    public class Program {
        static void Main(string[] args) {
            IIO io = new FileIO();
            string file = io.ReadSource("G:\\Blogs\\blogs\\5114.male.25.indUnk.Scorpio.xml");
            ITextExtractor it = new TextExtractor(file, "post");
            while (it.HasNextContent()) {
                Console.WriteLine("-------------------NEW POST-----------------------");
                Console.WriteLine(it.FindNextContent());
            }
        }
    }
}
