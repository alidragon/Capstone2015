using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Tree {
    public interface IDataTree {
        DataNode Root { get; set; }
        void AddWord();
    }
}
