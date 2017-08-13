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
    /// Logika interakcji dla klasy CancelQuizDialog.xaml
    /// </summary>
    public partial class CancelQuizDialog : Window
    {
        public CancelQuizDialog()
        {
            InitializeComponent();
        }
        private void YesButton_Clicked(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
        private void NoButton_Clicked(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
