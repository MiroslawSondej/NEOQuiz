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

namespace NEO_Quiz.Dialogs
{
    /// <summary>
    /// Logika interakcji dla klasy NoResourcesDialog.xaml
    /// </summary>
    public partial class NoResourcesDialog : Window
    {
        public NoResourcesDialog()
        {
            InitializeComponent();
        }

        private void OkButton_Clicked(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
