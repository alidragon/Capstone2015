using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeApi.Tree {
    public interface IDataTree {
        DataNode Root { get; }
        void AddConnection(string word);
        void SetBaseTree(IBaseTree tree);
        IBaseTree GetBaseTree();
    }
}
