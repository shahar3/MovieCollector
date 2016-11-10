using MovieCollector.View.Controls;
using MovieCollector.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MovieCollector.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MyViewModel vm;

        public MainWindow()
        {
            InitializeComponent();
            vm = new MyViewModel(new Model.MyModel());
            Controls.ToolBar tb = new Controls.ToolBar(vm);
            contentPanel.Children.Add(tb);
            CollectionScreen cs = new CollectionScreen(vm);
            collectionPanel.Children.Add(cs);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            vm.onClose();
        }
    }
}
