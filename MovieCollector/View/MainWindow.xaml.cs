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

namespace MovieCollector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            browser.Navigate("http://www.imdb.com");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //get the movie name that we want to search
            string movieName = MovieBox.Text;
            //get the IMDB main page
            mshtml.HTMLDocument document = (mshtml.HTMLDocument)browser.Document;
            //get the search bar element
            mshtml.IHTMLElement searchBar = document.getElementById("navbar-query");
            //insert in the search bar our movie name that we are looking for and click on the button
            searchBar.setAttribute("value", movieName);
            mshtml.IHTMLElement button = document.getElementById("navbar-submit-button");
            if (button != null)
                button.click();
            //download the result page and send to the model the examine

        }
    }
}
