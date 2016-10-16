using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Xml;

namespace MovieCollector.Model
{
    class MyModel:INotifyPropertyChanged
    {
        public MyModel()
        {

        }

        public async void searchMovie(string movieName)
        {
            //translate the name to fit the search query of IMDB
            string movieSearchName = translateMovieName(movieName);
            string searchFormula = "http://www.imdb.com/find?ref_=nv_sr_fn&q=" + movieSearchName + "&s=all";
            //download the page
            HtmlWeb searchResultPage = new HtmlWeb();
            HtmlDocument doc = await Task.Factory.StartNew(()=>searchResultPage.Load(searchFormula));
            extractMovies(doc);
        }

        private void extractMovies(HtmlDocument searchDoc)
        {
            HtmlNode[] nodes = searchDoc.DocumentNode.SelectNodes("//*[@id=\"main\"]/div/div[2]/table//tr").ToArray();
            foreach (HtmlNode node in nodes)
            {
                
            }
        }

        /// <summary>
        /// returns the correct form for the search query
        /// </summary>
        /// <param name="movieName"></param>
        /// <returns></returns>
        private string translateMovieName(string movieName)
        {
            string translated = string.Empty;
            foreach (char c in movieName)
            {
                if (c == ' ')
                {
                    translated += '+';
                }
                else
                {
                    translated += c;
                }
            }
            return translated;
        }

        #region event triggered
        public event PropertyChangedEventHandler PropertyChanged;
        private void notifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        #endregion
    }
}
