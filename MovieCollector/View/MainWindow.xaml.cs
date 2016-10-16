﻿using System;
using HtmlAgilityPack;
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
using MovieCollector.ViewModel;
using MovieCollector.Model;

namespace MovieCollector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MyViewModel vm;

        public MainWindow()
        {
            InitializeComponent();
            vm = new MyViewModel(new MyModel());
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            browser.Navigate("http://www.imdb.com");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //get the movie name that we want to search
            string movieName = MovieBox.Text;
            vm.searchMovie(movieName);
        }
    }
}
