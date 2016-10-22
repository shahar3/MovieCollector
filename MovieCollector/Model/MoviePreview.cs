using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollector.Model
{
    public class MoviePreview
    {
        /// <summary>
        /// The MoviePreview constructor. here we keep the relevant information that we need
        /// with all the data that we want to show to the user.
        /// </summary>
        /// <param name="movieName">the movie name</param>
        /// <param name="movieImg">a small poster image</param>
        /// <param name="yearReleased">the year the movie was released</param>
        public MoviePreview(string movieName,string movieImg, int yearReleased, string link)
        {
            this.movieName = movieName;
            this.movieImg = movieImg;
            this.yearReleased = yearReleased;
            this.pageLink = link;
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

        private string pageLink;

        public string PageLink
        {
            get { return pageLink; }
            set { pageLink = value; }
        }

        #endregion

        /// <summary>
        /// This function is just for debugging purpose
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return movieName + " - " + yearReleased;
        }
    }
}
