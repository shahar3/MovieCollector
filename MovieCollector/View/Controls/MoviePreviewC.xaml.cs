﻿using MovieCollector.Model;
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
    /// Interaction logic for MoviePreviewC.xaml
    /// </summary>
    public partial class MoviePreviewC : UserControl
    {
        public MoviePreviewC()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty MoviePreviewProperty = 
            DependencyProperty.Register("MoviePreview", typeof(MoviePreview), typeof(MoviePreviewC));

        public MoviePreview MoviePreview
        {
            get { return (MoviePreview)GetValue(MoviePreviewProperty); }
            set { SetValue(MoviePreviewProperty, value); }
        }

    }
}
