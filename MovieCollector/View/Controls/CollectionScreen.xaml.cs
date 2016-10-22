using MovieCollector.Model;
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
    /// Interaction logic for CollectionScreen.xaml
    /// </summary>
    public partial class CollectionScreen : UserControl
    {
        public CollectionScreen()
        {
            InitializeComponent();
        }

        public CollectionScreen(MyViewModel vm)
        {
            InitializeComponent();
            this.DataContext = vm;
        }

        //show the selected movie in the movie info screen
        private void moviesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            movieInfoPanel.Children.Clear();
            Movie movieToShow = (Movie)moviesList.SelectedItem;
            MovieInfoC mic = new MovieInfoC(movieToShow);
            movieInfoPanel.Children.Add(mic);
        }
    }
}
