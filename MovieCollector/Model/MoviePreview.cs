using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollector.Model
{
    public class MoviePreview
    {
        public MoviePreview(string movieName,string movieImg, int yearReleased)
        {
            this.movieName = movieName;
            this.movieImg = movieImg;
            this.yearReleased = yearReleased;
        }

        #region Properties
        private string movieName;

        public string MovieName
        {
            get { return movieName; }
            set { movieName = value; }
        }

        private string movieImg;

        public string MovieImg
        {
            get { return movieImg; }
            set { movieImg = value; }
        }

        private int yearReleased;

        public int YearReleased
        {
            get { return yearReleased; }
            set { yearReleased = value; }
        }
        #endregion

        public override string ToString()
        {
            return movieName + " - " + yearReleased;
        }
    }
}
