using TreeApi.Tree;
using System;
using System.Collections.Generic;
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
using Microsoft.VisualBasic;
using Microsoft.Win32;
using TreeApi.Tree.IO;
using TreeApi.Tree.ContentTree;

namespace TreeBuilder {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private ViewableTree tree;
        private IBaseTree baseTree;

        public MainWindow() {
            InitializeComponent();

            baseTree = new DuplicatesAllowedBaseTree();
            baseTree.AddWord((string)null, "Root");

            tree = new ViewableTree(baseTree);
            nodeList.ItemsSource = tree.Root;

        }

        private void Add_Click(object sender, RoutedEventArgs e) {
            ViewableTreeNode selected = nodeList.SelectedItem as ViewableTreeNode;
            string word = wordBox.Text;
            if (selected == null) {
                MessageBox.Show("You must select a parent before adding a word");
            } else if(string.IsNullOrEmpty(word)) {
                MessageBox.Show("You must enter a word to add to the tree");
            } else {
                tree.Add(selected, word);
                wordBox.Text = "";
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e) {
            ViewableTreeNode selected = nodeList.SelectedItem as ViewableTreeNode;
            if (selected == null) {
                MessageBox.Show("You must select a word to delete");
            } else {
                try {
                    tree.Remove(selected);
                } catch (InvalidOperationException) {
                    MessageBox.Show("Cannot remove root of tree");
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e) {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "Tree"; // Default file name
            sfd.DefaultExt = ".tree"; // Default file extension
            sfd.Filter = "Tree Files (.tree)|*.tree"; // Filter files by extension 

            // Show open file dialog box
            Nullable<bool> result = sfd.ShowDialog();

            // Process open file dialog box results 
            if (result == true) {
                // Open document 
                string filename = sfd.FileName;
                TreeIO io = new TreeIO();
                io.SaveBaseTree(baseTree, filename);
            }
        }
        private void Load_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "Tree"; 
            ofd.DefaultExt = ".tree";
            ofd.Filter = "Tree Files (.tree)|*.tree";

            Nullable<bool> result = ofd.ShowDialog();

            if (result == true) {
                string filename = ofd.FileName;
                TreeIO io = new TreeIO();
                baseTree = io.LoadBaseTree(filename);

                tree = new ViewableTree(baseTree);
                nodeList.ItemsSource = tree.Root;
            }
        }

        private void New_Click(object sender, RoutedEventArgs e) {
            baseTree = new DuplicatesAllowedBaseTree();
            baseTree.AddWord((string)null, "Root");

            tree = new ViewableTree(baseTree);
            nodeList.ItemsSource = tree.Root;
        }

        private void Node_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            Label nodeLabel = sender as Label;
            string newName = Interaction.InputBox("Please enter the new keyword:", "Rename", nodeLabel.Content.ToString());
            if (!string.IsNullOrEmpty(newName)) {
                ViewableTreeNode selected = nodeList.SelectedItem as ViewableTreeNode;
                tree.Rename(selected, newName);
            }
        }
    }
}
