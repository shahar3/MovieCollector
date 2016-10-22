using MovieCollector.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollector.ViewModel
{
    public class MyViewModel : INotifyPropertyChanged
    {
        MyModel model;

        /// <summary>
        /// The default constructor
        /// when an event is triggered in the model to correspondent event is activated in the view model
        /// </summary>
        /// <param name="model"></param>
        public MyViewModel(MyModel model)
        {
            this.model = model;
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
             {
                 notifyPropertyChanged("VM_" + e.PropertyName);
             };
        }

        /// <summary>
        /// Call the search movie function in the model
        /// </summary>
        /// <param name="movieName"></param>
        public void searchMovie(string movieName)
        {
            model.searchMovie(movieName);
        }

        private ObservableCollection<MoviePreview> moviesFound;

        public ObservableCollection<MoviePreview> VM_MoviesFound
        {
            get { return model.MoviesFound; }
            set {
                moviesFound = value;
            }
        }

        private ObservableCollection<Movie> myCollection;

        public ObservableCollection<Movie> VM_MyCollection
        {
            get { return model.MyCollection; }
            set { myCollection = value; }
        }


        /// <summary>
        /// call the addMovieToCollection function in the model
        /// </summary>
        /// <param name="selectedMovie"></param>
        public void addMovieToCollection(MoviePreview selectedMovie)
        {
            model.addMovieToCollection(selectedMovie);
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
