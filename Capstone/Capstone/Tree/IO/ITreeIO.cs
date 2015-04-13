using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeApi.Tree.IO {
    public interface ITreeIO {
        void SaveBaseTree(IBaseTree tree, string location);
        IBaseTree LoadBaseTree(string location);
    }
}
