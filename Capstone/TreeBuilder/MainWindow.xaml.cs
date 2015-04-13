﻿using TreeApi.Tree;
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

namespace TreeBuilder {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private IBaseTree tree;

        public MainWindow() {
            InitializeComponent();

            tree = new BaseTree();
            tree.AddWord((string)null, "Root");

            //nodeList.ItemsSource = tree;
        }

        private void Load_Click(object sender, RoutedEventArgs e) {
            //open new popup window to find tree save file, open file
        }

        private void Add_Click(object sender, RoutedEventArgs e) {

        }

        private void Delete_Click(object sender, RoutedEventArgs e) {

        }

        private void Save_Click(object sender, RoutedEventArgs e) {

        }
    }
}
