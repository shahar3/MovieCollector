using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollector.Model
{
    public class Movie
    {
        #region Properties
        private string name;

        public string MovieName
        {
            get { return name; }
            set { name = value; }
        }

        private string plot;

        public string MoviePlot
        {
            get { return plot; }
            set { plot = value; }
        }

        private int year;

        public int MovieReleaseYear
        {
            get { return year; }
            set { year = value; }
        }

        private List<string> movieGenre;

        public List<string> MovieGenre
        {
            get { return movieGenre; }
            set { movieGenre = value; }
        }

        private string movieGenreString;

        public string MovieGenreString
        {
            get { return string.Join(",",movieGenre.ToArray()); }
            set { movieGenreString = value; }
        }

        private List<ActorRole> movieCast;

        public List<ActorRole> MovieCast //the actor and his role in the movie
        {
            get { return movieCast; }
            set { movieCast = value; }
        }

        #endregion

        public Movie(string name, string plot, int year, List<string> movieGenre, List<ActorRole> movieCast)
        {
            this.name = name;
            this.plot = plot;
            this.year = year;
            this.movieGenre = movieGenre;
            this.movieCast = movieCast;
        }

        public override bool Equals(object obj)
        {
            Movie otherMovie = (Movie)obj;
            return otherMovie.ToString() == this.ToString();
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})",name,year);
        }
    }
}
