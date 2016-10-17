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

        public MyViewModel(MyModel model)
        {
            this.model = model;
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
             {
                 notifyPropertyChanged("VM_" + e.PropertyName);
             };
        }

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
