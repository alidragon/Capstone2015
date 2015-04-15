using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreeApi.Tree {
    public class Connection {
        public long Weight { get; set; }
        public DataNode EndPoint { get; set; }

        public Connection(DataNode end) {
            this.Weight = 0;
            this.EndPoint = end;
        }
    }
}
