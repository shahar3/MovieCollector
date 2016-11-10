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
using System.IO;
using System.Windows;

namespace MovieCollector.Model
{
    public class MyModel:INotifyPropertyChanged
    {
        DataTable movieTable; //stores the movies details
        Collection collection;
        const string DOMAIN = "http://www.imdb.com";

        //private movie details
        private List<string> movieGenre;
        private string moviePlot;
        private string movieName;
        private int movieYear;
        private List<ActorRole> movieCast;

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
            movieCast = new List<ActorRole>();
            loadData();
        }

        private void loadData()
        {
            if (!Directory.Exists("Data"))
            {
                Directory.CreateDirectory("Data");
            }else
            {
                //load my collection from "data/my collection.bin"
                if(File.Exists("Data\\my collection.bin"))
                    readCollectionFromFile();
            }
        }

        private void readCollectionFromFile()
        {
            using(FileStream fs = new FileStream("data\\my collection.bin", FileMode.Open))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    while (br.BaseStream.Position != br.BaseStream.Length)
                    {
                        string movieName = br.ReadString();
                        string moviePlot = br.ReadString();
                        int movieReleaseYear = br.ReadInt32();
                        List<string> movieGenre = new List<string>();
                        int numOfGenres = br.ReadInt32();
                        while (numOfGenres > 0)
                        {
                            movieGenre.Add(br.ReadString());
                            numOfGenres--;
                        }
                        List<ActorRole> movieCast = new List<ActorRole>();
                        int numOfActors = br.ReadInt32();
                        while (numOfActors > 0)
                        {
                            string actorName = br.ReadString();
                            string actorUrl = br.ReadString();
                            string role = br.ReadString();
                            ActorRole actorRole = new ActorRole(new Actor(actorName, actorUrl), role);
                            movieCast.Add(actorRole);
                            numOfActors--;
                        }
                        Movie movie = new Movie(movieName, moviePlot, movieReleaseYear,movieGenre,movieCast);
                        myCollection.Add(movie);
                    }
                }
            }
        }

        /// <summary>
        /// creates binary file with the collection
        /// </summary>
        public void saveCollectionToFile()
        {
            using(FileStream fs = new FileStream("data\\my collection.bin", FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    foreach (Movie movie in myCollection)
                    {
                        bw.Write(movie.MovieName);
                        bw.Write(movie.MoviePlot);
                        bw.Write(movie.MovieReleaseYear);
                        bw.Write(movie.MovieGenre.Count); //how many genres the movie has
                        foreach (string genre in movie.MovieGenre)
                        {
                            bw.Write(genre);
                        }
                        bw.Write(movie.MovieCast.Count); //how many actors playing
                        foreach (ActorRole actorRole in movie.MovieCast)
                        {
                            bw.Write(actorRole.Actor.ActorName);
                            bw.Write(actorRole.Actor.ActorUrl);
                            bw.Write(actorRole.Role);
                        }
                    }
                }
            }
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
            //check if the movie already exist in our collection
            if (!myCollection.Contains(movieToAdd))
            {
                myCollection.Add(movieToAdd);
            }else
            {
                MessageBox.Show(String.Format("The movie {0} is already in the list", movieToAdd));
            }
        }

        private Movie getMovie(MoviePreview selectedMovie)
        {
            string link = selectedMovie.PageLink;
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(link);
            extractMovieDetails(doc);
            Movie movie = new Movie(movieName, moviePlot, movieYear,movieGenre, movieCast);
            return movie;
        }

        private void extractMovieDetails(HtmlDocument doc)
        {
            //get the plot 
            HtmlNode[] nodes = doc.DocumentNode.SelectNodes("//div[@class=\"summary_text\"]").ToArray();
            moviePlot = nodes[0].InnerText.Trim();
            //get the genres
            movieGenre = new List<string>();
            HtmlNode[] genreNodes = doc.DocumentNode.SelectNodes("//div[@id=\"title-overview-widget\"]/div[2]/div[2]/div/div[2]/div[2]/div/a//span").ToArray();
            foreach (HtmlNode genreNode in genreNodes)
            {
                movieGenre.Add(genreNode.InnerText);
            }
            //get the cast
            HtmlNode[] castTableNodes = doc.DocumentNode.SelectNodes("//*[@id=\"titleCast\"]/table//tr").ToArray();
            bool firstRow = true;
            foreach (HtmlNode row in castTableNodes)
            {
                if (firstRow)
                {
                    firstRow = false;
                    continue;
                }
                int i = 0;
                string actorName="", actorUrl="",role="",picUrl="";
                foreach (HtmlNode column in row.SelectNodes(".//td[1]|td[2]|td[4]"))
                {
                    if (i%3==1)
                    {
                        //extract actor data
                        actorName = column.InnerText.Trim();
                        actorUrl = DOMAIN + column.SelectNodes(".//a")[0].GetAttributeValue("href","");
                    }else if(i%3==2)
                    { //extract actor's role in the movie
                        role = column.InnerText.Trim();
                        ActorRole actorRole = new ActorRole(new Actor(actorName, actorUrl,picUrl), role);
                        movieCast.Add(actorRole);
                    }else
                    {
                        //get actor pic
                        HtmlNode actorPicNode = column.SelectNodes(".//a/img").First();
                        picUrl = actorPicNode.GetAttributeValue("loadlate", "");
                    }
                    i++;
                }
            }
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
