using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Tree.IO {
    public class TreeIO : ITreeIO {
        public void SaveBaseTree(IBaseTree tree, string location) {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(location, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, tree);
            stream.Close();
        }

        public IBaseTree LoadBaseTree(string location) {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(location, FileMode.Open, FileAccess.Read, FileShare.Read);
            IBaseTree tree;
            try {
                tree = formatter.Deserialize(stream) as IBaseTree;
            } catch(InvalidCastException) {
                throw new InvalidFileException();
            } finally {
                stream.Close();
            }
            return tree;
        }
    }
}
