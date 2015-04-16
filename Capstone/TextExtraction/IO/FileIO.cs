using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextExtraction.IO {
    public class FileIO : IIO {

        public string ReadSource(object source) {
            if (source != null && source is String) {
                return ReadFile((string)source);
            }
            throw new InvalidOperationException("Source must be a file path");
        }

        public string ReadFile(string path) {
            if (!File.Exists(path)) {
                throw new InvalidOperationException("Path must lead to an existing file.");
            }
            return File.ReadAllText(path);
        }

        public IEnumerable<string> ReadSourceIterable(object source) {
            if (source != null && source is String) {
                return ReadFileIterable((string)source);
            }
            throw new InvalidOperationException("Source must be a file path");
        }

        private IEnumerable<string> ReadFileIterable(string path) {
            if (!File.Exists(path)) {
                throw new InvalidOperationException("Path must lead to an existing file.");
            }
            return File.ReadAllLines(path);
        }
    }
}
