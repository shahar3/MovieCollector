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

        public int MovieRealeaseYear
        {
            get { return year; }
            set { year = value; }
        }
        #endregion

        public Movie(string name, string plot, int year)
        {
            this.name = name;
            this.plot = plot;
            this.year = year;
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})",name,year);
        }
    }
}
