using MovieCollector.Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MovieCollector.View.Controls
{
    /// <summary>
    /// Interaction logic for ActorRoleC.xaml
    /// </summary>
    public partial class ActorRoleC : UserControl
    {
        public ActorRoleC()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ActorRoleProperty =
            DependencyProperty.Register("ActorRole", typeof(ActorRole), typeof(ActorRoleC));

        public ActorRole ActorRole
        {
            get { return (ActorRole)GetValue(ActorRoleProperty); }
            set { SetValue(ActorRoleProperty, value); }
        }
    }
}
