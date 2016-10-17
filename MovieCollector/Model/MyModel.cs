using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Xml;
using System.Data;
using System.Collections.ObjectModel;

namespace MovieCollector.Model
{
    public class MyModel:INotifyPropertyChanged
    {
        DataTable movieTable;

        #region Properties
        private ObservableCollection<MoviePreview> moviesFound;

        public ObservableCollection<MoviePreview> MoviesFound
        {
            get { return moviesFound; }
            set {
                moviesFound = value;
            }
        }
        #endregion

        public MyModel()
        {
            initMovieTable();
            moviesFound = new ObservableCollection<MoviePreview>();
        }

        private void initMovieTable()
        {
            movieTable = new DataTable();
            movieTable.Columns.Add("movieImage",typeof(string));
            movieTable.Columns.Add("movieName", typeof(string));
            movieTable.Columns.Add("movieYear", typeof(int));
        }

        public async void searchMovie(string movieName)
        {
            //clear the lists
            movieTable.Clear();
            moviesFound.Clear();
            //translate the name to fit the search query of IMDB
            string movieSearchName = translateMovieName(movieName);
            string searchFormula = "http://www.imdb.com/find?ref_=nv_sr_fn&q=" + movieSearchName + "&s=all";
            //download the page
            HtmlWeb searchResultPage = new HtmlWeb();
            HtmlDocument doc = await Task.Factory.StartNew(()=>searchResultPage.Load(searchFormula));
            extractMovies(doc);
            addMoviesToList();
        }

        private void addMoviesToList()
        {
            foreach (DataRow row in movieTable.Rows)
            {
                string icon = (string)row[0];
                string name = (string)row[1];
                int year = (int)(row[2]);
                MoviePreview movie = new MoviePreview(name, icon, year);
                moviesFound.Add(movie);
            }
        }

        private void extractMovies(HtmlDocument searchDoc)
        {
            HtmlNode[] moviesIconsNodes = searchDoc.DocumentNode.SelectNodes("//*[@id=\"main\"]/div/div[2]/table//tr//td[1]").ToArray();
            HtmlNode[] moviesTitlesNodes = searchDoc.DocumentNode.SelectNodes("//*[@id=\"main\"]/div/div[2]/table//tr//td[2]").ToArray();
            extractImageUrl(moviesIconsNodes);
            extractNameAndYear(moviesTitlesNodes);
        }

        private void extractNameAndYear(HtmlNode[] moviesTitlesNodes)
        {
            int rowNumber = 0;
            foreach (HtmlNode node in moviesTitlesNodes)
            {
                int movieYear = 0;
                string fullText = node.InnerText;
                string movieName;
                if (!fullText.Contains('('))
                {
                    movieName = fullText;
                }
                else
                {
                    movieName = fullText.Substring(1, fullText.IndexOf('(') - 1);
                    int idxOpenParent = fullText.IndexOf('('); //index of '('
                    int idxCloseParent = fullText.IndexOf(')');
                    string yearStr = fullText.Substring(idxOpenParent + 1, idxCloseParent - idxOpenParent - 1);
                    movieYear = Int32.Parse(yearStr);
                }
                //now add the movie's name and year to the table
                movieTable.Rows[rowNumber][1] = movieName;
                movieTable.Rows[rowNumber++][2] = movieYear;
            }
        }

        private void extractImageUrl(HtmlNode[] moviesIconsNodes)
        {
            foreach (HtmlNode node in moviesIconsNodes)
            {
                DataRow newRow = movieTable.NewRow();
                newRow[0] = node.SelectNodes(".//img")[0].GetAttributeValue("src","");
                movieTable.Rows.Add(newRow);
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
