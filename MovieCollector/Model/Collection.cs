using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollector.Model
{
    /// <summary>
    /// This class keeps all the data about the movies in our collection
    /// load it and save it in a database
    /// </summary>
    class Collection
    {
        ObservableCollection<Movie> collectionMovies;

        public Collection()
        {
            initCollection();
        }

        private void initCollection()
        {
            collectionMovies = new ObservableCollection<Movie>();   
        }
    }
}
