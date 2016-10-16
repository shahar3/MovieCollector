using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieCollector.Model
{
    class MyModel:INotifyPropertyChanged
    {
        public MyModel()
        {

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
