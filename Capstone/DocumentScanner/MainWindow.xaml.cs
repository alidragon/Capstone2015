using Capstone.Tree.DataTree.Comparison;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TextExtraction;
using TextExtraction.Extract;
using TextExtraction.IO;
using TreeApi.Tree;
using TreeApi.Tree.IO;

namespace DocumentScanner {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        IBaseTree baseTree;
        ObservableCollection<string> documents;
        
        public MainWindow() {
            InitializeComponent();
            documentList.ItemsSource = documents;
        }

        private void loadContentTreeButton_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "Tree";
            ofd.DefaultExt = ".tree";
            ofd.Filter = "Tree Files (.tree)|*.tree";

            Nullable<bool> result = ofd.ShowDialog();

            if (result == true) {
                string filename = ofd.FileName;
                TreeIO io = new TreeIO();
                baseTree = io.LoadBaseTree(filename);
                contentTreeLabel.Content = ofd.FileName;
            }

        }

        private void buildDataTreeButton_Click(object sender, RoutedEventArgs e) {
            buildDataTreePopup.IsOpen = true;
        }

        private void loadDataTreeButton_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "Tree";
            ofd.DefaultExt = ".dtree";
            ofd.Filter = "Data Tree Files (.dtree)|*.dtree";

            Nullable<bool> result = ofd.ShowDialog();

            if (result == true) {
                string filename = ofd.FileName;
                TreeIO io = new TreeIO();
                IDataTree tree = io.LoadDataTree(filename);

                DocumentTreeWrapper wrapper = new DocumentTreeWrapper(tree);

                dataTree.ItemsSource = wrapper.Tree;
            }
        }

        private void formatOkay_Click(object sender, RoutedEventArgs e) {
            if (baseTree == null) {
                MessageBox.Show("Please select a content tree for the data tree.");
                return;
            }

            if(formatBox.SelectedIndex == -1) {
                formatBox.BorderBrush = Brushes.Red;
                return;
            }
            if(string.IsNullOrEmpty(documentFormatBox.Text)) {
                documentFormatBox.BorderBrush = Brushes.Red;
                return;
            }

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "Tree";
            ofd.DefaultExt = ".txt";

            Nullable<bool> result = ofd.ShowDialog();

            if (result == true) {
                string filename = ofd.FileName;
                documentLabel.Content = filename + "datatrees";
                using (Ookii.Dialogs.Wpf.ProgressDialog dial = new ProgressDialog()) {
                    dial.ProgressBarStyle = ProgressBarStyle.MarqueeProgressBar;
                    dial.Show();
                    dial.Description = "Analyzing text...";
                    IIO io = new FileIO();
                    ITextExtractor it = null;
                    switch (formatBox.SelectedIndex) {
                        case 0:
                            string text = io.ReadSource(filename);
                            it = new XMLTextExtractor(text, documentFormatBox.Text);
                            break;
                        case 1:
                            var texts = io.ReadSourceIterable(filename);
                            it = new BeginMarkerExtraction(texts, documentFormatBox.Text);
                            break;
                        default:
                            throw new InvalidOperationException();
                    }
                    documents = new ObservableCollection<string>();
                    while (it.HasNextContent()) {
                        string content = it.FindNextContent();
                        string name = Helpers.GetNameWhenFirst(content);
                        documents.Add(name);

                        IDataTree tree = DataTreeBuilder.CreateDocumentMappedTree(baseTree);
                        DataTreeBuilder.AddToDataTree(tree, content);

                        ITreeIO tio = new TreeIO();
                        tio.SaveDataTree(tree, filename + @"datatrees\" + name + ".dtree");
                    }
                    documentList.ItemsSource = documents;
                }
            }


            buildDataTreePopup.IsOpen = false;
        }

        private void documentList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            object selectedVal = documentList.SelectedValue;
            string name = selectedVal.ToString();
            string filename = documentLabel.Content.ToString() + @"\" + name + ".dtree";

            ITreeIO tio = new TreeIO();
            IDataTree tree = tio.LoadDataTree(filename);

            DocumentTreeWrapper wrapper = new DocumentTreeWrapper(tree);

            dataTree.ItemsSource = wrapper.Tree;
        }

        private void loadDirectory_Click(object sender, RoutedEventArgs e) {
            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();
            var result = dialog.ShowDialog();
            if (result == true) {
                documentLabel.Content = dialog.SelectedPath;
                IEnumerable<string> files = Directory.EnumerateFiles(dialog.SelectedPath);
                documents = new ObservableCollection<string>();
                foreach(string s in files) {
                    string s2 = System.IO.Path.GetFileName(s);
                    s2 = new string(s2.Take(s2.LastIndexOf('.')).ToArray());
                    documents.Add(s2);
                }
                documentList.ItemsSource = documents;
            }
        }

        private void queryButton_Click(object sender, RoutedEventArgs e) {
            if (documents == null || documents.Count < 1) {
                MessageBox.Show("Please load a directory with 1 or more data trees before querying");
            }
            if (baseTree == null) {
                MessageBox.Show("Please load a content tree before querying.");
            }
            queryPopup.IsOpen = true;
        }

        private void queryOkay_Click(object sender, RoutedEventArgs e) {
            string query = queryBox.Text;
            List<string> matches = new List<string>();
            using (Ookii.Dialogs.Wpf.ProgressDialog dial = new ProgressDialog()) {
                IDataTree queryTree = DataTreeBuilder.CreateDocumentMappedTree(baseTree);
                DataTreeBuilder.AddToDataTreeBoyerMoore(queryTree, query);

                ITreeIO tio = new TreeIO();
                foreach (String s in Directory.EnumerateFiles(documentLabel.Content.ToString())) {
                    IDataTree docTree = tio.LoadDataTree(s);
                    if (queryTree.CompareTo(docTree)) {
                        matches.Add(s);
                    }
                }
            }

            if (matches.Count > 0) {
                StringBuilder sb = new StringBuilder("Documents which match \"" + query + "\":");
                foreach (string s in matches) {
                    string s2 = System.IO.Path.GetFileName(s);
                    s2 = new string(s2.Take(s2.LastIndexOf('.')).ToArray());
                    documents.Add(s2);
                    sb.Append(" " + s2);
                }
                MessageBox.Show(sb.ToString());
            } else {
                MessageBox.Show("No documents match the query \"" + query + "\"");
            }
            queryPopup.IsOpen = false;
        }

        private void compareButton_Click(object sender, RoutedEventArgs e) {
            MessageBox.Show("Please select first tree");
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "Tree";
            ofd.DefaultExt = ".dtree";
            ofd.Filter = "Data Tree Files (.dtree)|*.dtree";

            Nullable<bool> result = ofd.ShowDialog();

            if (result == true) {
                string filename = ofd.FileName;
                TreeIO io = new TreeIO();
                IDataTree tree = io.LoadDataTree(filename);


                MessageBox.Show("Please select a second tree");

                result = ofd.ShowDialog();

                if (result == true) {
                    string filename2 = ofd.FileName;
                    IDataTree tree2 = io.LoadDataTree(filename2);

                    if (tree.CompareTo(tree2)) {
                        MessageBox.Show("Your treees match");
                    } else {
                        MessageBox.Show("These trees do not match");
                    }
                }
            }
        }


    }
}
