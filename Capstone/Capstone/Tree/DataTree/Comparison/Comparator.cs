using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeApi.Tree;

namespace Capstone.Tree.DataTree.Comparison {
    public static class Comparator {
        private const double COMPARE_VALUE = .33;
        private const double BRANCH_WEIGHT_VALUE = 1.2;
        private const double ERROR_LEVEL = .14;
        //private const double IRREL_WEIGHT_VALUE = 1.5;

        public static bool CompareTo(this IDataTree query, IDataTree document) {
            //double dif = Dif(query.Root, document.Root, query.MappedWords, document.MappedWords);
            //double dif = DifUsingCutoffs(query.Root, document.Root, query.MappedWords, document.MappedWords);
            double dif = DifUsingCutoffs(query.Root, document.Root, query.MappedWords, document.MappedWords);
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
                    dif += c.Weight / (totalTwo);
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


        public static double DifUsingCutoffs(DataNode rootQuery, DataNode rootDoc, double totalQuery, double totalDoc) {
            double dif = 0;

            if (rootQuery.Children.Count > 0) {
                List<Connection> docCopy = rootDoc.Children.ToList();
                foreach (Connection child in rootQuery.Children) {
                    double weight = child.Weight / totalQuery;
                    var temp = rootDoc.Children.Where(c => c.EndPoint.Equals(child.EndPoint)).FirstOrDefault();
                    if (temp != null) {
                        //CHANGE THIS TO BE A 15% acceptable error range
                        //dif += Math.Abs(weight - (temp.Weight / totalDoc));
                        double tempWeight = temp.Weight / totalDoc;
                        if (Math.Abs(weight - tempWeight) > ERROR_LEVEL) {
                            dif += Math.Abs(weight - tempWeight) - ERROR_LEVEL;
                            //if (tempWeight < .03) {
                            //    dif += weight;
                            //} else {
                            //    double tempdif = Math.Abs(weight - tempWeight) - ERROR_LEVEL;
                            //    dif += tempdif;//something
                            //}
                        }
                        docCopy.Remove(temp);
                    } else {
                        //this is probably okay
                        //dif += weight * Math.Log(totalQuery / 3.5);
                        dif += weight * 1.1;
                    }
                }
                    
                foreach (Connection c in docCopy) {
                    //make this a 15% error range, too
                    double temp = c.Weight / (totalDoc);
                    if (temp > ERROR_LEVEL) {
                        dif += temp/*- ERROR_LEVEL*/;
                    }
                }

                if (dif < COMPARE_VALUE) {
                    foreach (Connection child in rootQuery.Children) {
                        var temp = rootDoc.Children.Where(c => c.EndPoint.Equals(child.EndPoint)).FirstOrDefault();
                        if (temp != null) {
                            dif += DifUsingCutoffs(child.EndPoint, temp.EndPoint, totalQuery * BRANCH_WEIGHT_VALUE, totalDoc * BRANCH_WEIGHT_VALUE);
                        }
                    }
                }
            } else {
                //if (rootDoc.Children.Count > 0) {
                //    foreach (Connection c in rootDoc.Children) {
                //        dif += c.Weight / totalDoc;
                //    }
                //}
            }

            return dif;
        }

        public static double DifUsingErrorMarginsAndDifferentBranching(DataNode rootQuery, DataNode rootDoc, double totalQuery, double totalDoc, double error) {
            double dif = 0;

            if (rootQuery.Children.Count > 0) {
                List<Connection> docCopy = rootDoc.Children.ToList();
                foreach (Connection child in rootQuery.Children) {
                    double weight = child.Weight / totalQuery;
                    var temp = rootDoc.Children.Where(c => c.EndPoint.Equals(child.EndPoint)).FirstOrDefault();
                    if (temp != null) {
                        //acceptable error range
                        //dif += Math.Abs(weight - (temp.Weight / totalDoc));
                        double tempWeight = temp.Weight / totalDoc;
                        if (Math.Abs(weight - tempWeight) > error) {
                            dif += Math.Abs(weight - tempWeight) - error;
                        }
                        docCopy.Remove(temp);
                    } else {
                        //this is probably okay
                        dif += weight;
                    }
                }

                foreach (Connection c in docCopy) {
                    //make this a 15% error range, too
                    double temp = c.Weight / (totalDoc);
                    if (temp > error) {
                        dif += temp /*- ERROR_LEVEL*/;
                    }
                }

                if (dif < COMPARE_VALUE) {
                    foreach (Connection child in rootQuery.Children) {
                        var temp = rootDoc.Children.Where(c => c.EndPoint.Equals(child.EndPoint)).FirstOrDefault();
                        if (temp != null) {
                            dif += DifUsingErrorMarginsAndDifferentBranching(child.EndPoint, temp.EndPoint, child.Weight, temp.Weight, error * BRANCH_WEIGHT_VALUE);
                        }
                    }
                }
            } else {
                //if (rootDoc.Children.Count > 0) {
                //    foreach (Connection c in rootDoc.Children) {
                //        dif += c.Weight / totalDoc;
                //    }
                //}
            }

            return dif;
        }
    }
}
