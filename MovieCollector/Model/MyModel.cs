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
        DataTable movieTable; //stores the movies details
        Collection collection;
        const string DOMAIN = "http://www.imdb.com";

        //private movie details
        private string moviePlot;
        private string movieName;
        private int movieYear;

        #region Properties
        private ObservableCollection<Movie> myCollection;

        public ObservableCollection<Movie> MyCollection
        {
            get { return myCollection; }
            set { myCollection = value; }
        }


        //Property for the list of movies we found after the IMDB search
        private ObservableCollection<MoviePreview> moviesFound;

        public ObservableCollection<MoviePreview> MoviesFound
        {
            get { return moviesFound; }
            set {
                moviesFound = value;
            }
        }
        #endregion

        /// <summary>
        /// the default constructor. 
        /// </summary>
        public MyModel()
        {
            initMovieTable();
            moviesFound = new ObservableCollection<MoviePreview>();
            myCollection = new ObservableCollection<Movie>();
            collection = new Collection();
        }

        /// <summary>
        /// Initialize the movie table to contain columns for the movie image, movie name, movie year and page link
        /// </summary>
        private void initMovieTable()
        {
            movieTable = new DataTable();
            movieTable.Columns.Add("movieImage",typeof(string));
            movieTable.Columns.Add("movieName", typeof(string));
            movieTable.Columns.Add("movieYear", typeof(int));
            movieTable.Columns.Add("pageLink", typeof(string));
        }

        public void addMovieToCollection(MoviePreview selectedMovie)
        {
            movieName = selectedMovie.MovieName;
            movieYear = selectedMovie.YearReleased;
            Movie movieToAdd = getMovie(selectedMovie);
            myCollection.Add(movieToAdd);
        }

        private Movie getMovie(MoviePreview selectedMovie)
        {
            string link = selectedMovie.PageLink;
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(link);
            extractMovieDetails(doc);
            Movie movie = new Movie(movieName, moviePlot, movieYear);
            return movie;
        }

        private void extractMovieDetails(HtmlDocument doc)
        {
            //get the plot 
            HtmlNode[] nodes = doc.DocumentNode.SelectNodes("//div[@class=\"summary_text\"]").ToArray();
            moviePlot = nodes[0].InnerText.Trim();
        }

        /// <summary>
        /// we call this function after we search a movie in the search bar in the GUI
        /// this function search the movie on IMDB and scrape the data that relevant to us
        /// </summary>
        /// <param name="movieName"></param>
        public async void searchMovie(string movieName)
        {
            //clear the lists
            movieTable.Clear();
            moviesFound.Clear();
            //translate the name to fit the search query of IMDB
            string movieSearchName = translateMovieName(movieName);
            string searchFormula = "http://www.imdb.com/find?ref_=nv_sr_fn&q=" + movieSearchName + "&s=all";
            //download the page
            HtmlDocument doc = await downloadPage(searchFormula); //stores the html document in doc
            //extract the relevant data
            extractMovies(doc);
            //gather the details from the movie table and add them to the moviesFound list
            addMoviesToList();
        }

        private static async Task<HtmlDocument> downloadPage(string searchFormula)
        {
            HtmlWeb searchResultPage = new HtmlWeb();
            HtmlDocument doc = await Task.Factory.StartNew(() => searchResultPage.Load(searchFormula));
            return doc;
        }

        /// <summary>
        /// Gather all the data from the movie table, build a MoviePreview object and add it to moviesFound list
        /// </summary>
        private void addMoviesToList()
        {
            foreach (DataRow row in movieTable.Rows)
            {
                string icon = (string)row[0];
                string name = (string)row[1];
                int year = (int)(row[2]);
                string pageLink = (string)row[3];
                //create the MoviePreview object
                MoviePreview movie = new MoviePreview(name, icon, year,pageLink);
                moviesFound.Add(movie);
            }
        }

        /// <summary>
        /// Extract the relevant data. steps:
        /// 1.Select all the HTML tags in the table in IMDB
        /// 2.send the ones of the image column to a function that will extract the images
        /// 3.send the ones of the movie details column to a function that will extract the other relevant data
        /// </summary>
        /// <param name="searchDoc"></param>
        private void extractMovies(HtmlDocument searchDoc)
        {
            HtmlNode[] moviesIconsNodes = searchDoc.DocumentNode.SelectNodes("//*[@id=\"main\"]/div/div[2]/table//tr//td[1]").ToArray();
            HtmlNode[] moviesTitlesNodes = searchDoc.DocumentNode.SelectNodes("//*[@id=\"main\"]/div/div[2]/table//tr//td[2]").ToArray();
            extractImageUrl(moviesIconsNodes);
            extractNameAndYear(moviesTitlesNodes);
        }

        /// <summary>
        /// This function extracts the name and the year of each movie record in our search
        /// </summary>
        /// <param name="moviesTitlesNodes"></param>
        private void extractNameAndYear(HtmlNode[] moviesTitlesNodes)
        {
            int rowNumber = 0;
            foreach (HtmlNode node in moviesTitlesNodes) //for each record (row)
            {
                //extract the link to the movie page
                string pageLink = DOMAIN+node.SelectNodes(".//a")[0].GetAttributeValue("href", "");
                int movieYear = 0; //just in case this detail is blank
                string fullText = node.InnerText; //get the full text line (eg: "Batman (1989)")
                string movieName;
                if (!fullText.Contains('(')) //if there is a year it will be inside parenthesis
                {
                    //there is no detail about the movie release year
                    movieName = fullText;
                }
                else
                {
                    movieName = fullText.Substring(1, fullText.IndexOf('(') - 1); //stores the movie name
                    int idxOpenParent = fullText.IndexOf('('); //index of '('
                    int idxCloseParent = fullText.IndexOf(')');
                    string yearStr = fullText.Substring(idxOpenParent + 1, idxCloseParent - idxOpenParent - 1); //stores the year
                    try
                    {
                        movieYear = Int32.Parse(yearStr); //convert it to integer
                    }
                    catch
                    {
                        movieYear = 0;
                    }
                }
                //now add the movie's name and year to the table
                movieTable.Rows[rowNumber][1] = movieName;
                movieTable.Rows[rowNumber][2] = movieYear;
                movieTable.Rows[rowNumber++][3] = pageLink;
            }
        }

        /// <summary>
        /// This function looks for the img tag and extract the link to the images
        /// </summary>
        /// <param name="moviesIconsNodes"></param>
        private void extractImageUrl(HtmlNode[] moviesIconsNodes)
        {
            foreach (HtmlNode node in moviesIconsNodes) //for each row extract the image
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
