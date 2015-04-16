using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace TreeApi.Tree.IO {
    public class TreeIO : ITreeIO {
        public void SaveBaseTree(IBaseTree tree, string location) {
            SaveObject(tree, location);
        }

        public IBaseTree LoadBaseTree(string location) {
            IBaseTree tree;
            try {
                tree = LoadObject(location) as IBaseTree;
            } catch(InvalidCastException) {
                throw new InvalidFileException();
            }
            if (tree == null) {
                throw new InvalidFileException();
            }
            return tree;
        }

        public void SaveDataTree(IDataTree tree, string location) {
            SaveObject(tree, location);
        }

        public IDataTree LoadDataTree(string location) {
            IDataTree tree;
            try {
                tree = LoadObject(location) as IDataTree;
            } catch (InvalidCastException) {
                throw new InvalidFileException();
            }
            if (tree == null) {
                throw new InvalidFileException();
            }
            return tree;
        }

        public void SaveObject(object o, string location) {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(location, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, o);
            stream.Close();
        }

        public object LoadObject(string location) {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(location, FileMode.Open, FileAccess.Read, FileShare.Read);
            object o = formatter.Deserialize(stream);
            stream.Close();
            return o;

        }
    }
}
