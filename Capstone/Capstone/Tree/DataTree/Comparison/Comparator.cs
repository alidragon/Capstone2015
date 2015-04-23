using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeApi.Tree;

namespace Capstone.Tree.DataTree.Comparison {
    public static class Comparator {
        private const double COMPARE_VALUE = .9;
        private const double BRANCH_WEIGHT_VALUE = 2.7;

        public static bool CompareTo(this IDataTree one, IDataTree two) {
            double dif = Dif(one.Root, two.Root, one.MappedWords, two.MappedWords);
            return dif < COMPARE_VALUE;
        }

        private static double Dif(DataNode rootOrig, DataNode rootComp, double totalOne, double totalTwo) {
            double dif = 0;
            
            if (rootOrig.Children.Count > 0) {
                List<Connection> twoCopy = rootComp.Children.ToList();
                foreach (Connection child in rootOrig.Children) {
                    double weight = child.Weight / totalOne;
                    var temp = rootComp.Children.Where(c => c.EndPoint.Equals(child.EndPoint)).FirstOrDefault();
                    if (temp != null) {
                        dif += Math.Abs(weight - (temp.Weight / totalTwo));
                        twoCopy.Remove(temp);
                    } else {
                        dif += weight;
                    }
                }

                foreach (Connection c in twoCopy) {
                    dif += c.Weight / totalTwo;
                }

                if (dif < COMPARE_VALUE) {
                    foreach (Connection child in rootOrig.Children) {
                        var temp = rootComp.Children.Where(c => c.EndPoint.Equals(child.EndPoint)).FirstOrDefault();
                        if (temp != null) {
                            dif += Dif(child.EndPoint, temp.EndPoint, totalOne * BRANCH_WEIGHT_VALUE, totalTwo * BRANCH_WEIGHT_VALUE);
                        }
                    }
                }

            } else {
                if (rootComp.Children.Count > 0) {
                    foreach (Connection c in rootComp.Children) {
                        dif += c.Weight / totalTwo;
                    }
                }
            }

            return dif;
        }
    }
}
