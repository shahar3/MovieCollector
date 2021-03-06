﻿using MovieCollector.Model;
using MovieCollector.ViewModel;
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

namespace MovieCollector.View.Controls
{
    /// <summary>
    /// Interaction logic for searchResultC.xaml
    /// </summary>
    public partial class searchResultC : UserControl
    {
        MyViewModel vm;

        public searchResultC(MyViewModel vm)
        {
            InitializeComponent();
            this.DataContext = vm;
            this.vm = vm;
        }

        public searchResultC()
        {
            InitializeComponent();
        }

        /// <summary>
        /// add the selected movie to the collection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MoviePreview selectedMovie = (MoviePreview)moviesList.SelectedItem;
            vm.addMovieToCollection(selectedMovie);
        }
    }
}
