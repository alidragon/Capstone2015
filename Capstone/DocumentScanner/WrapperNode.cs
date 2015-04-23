using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentScanner {
    public class WrapperNode {
        public ObservableCollection<WrapperNode> Children { get; set; }
        public string Keyword { get; set; }
        public long Weight { get; set; }

        public WrapperNode() {
            Children = new ObservableCollection<WrapperNode>();
        }
    }
}
