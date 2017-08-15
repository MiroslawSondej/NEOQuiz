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
    /// Logika interakcji dla klasy QuizWonDialog.xaml
    /// </summary>
    public partial class QuizWonDialog : Window
    {
        public QuizWonDialog(QuizManager manager)
        {
            InitializeComponent();

            switch (manager.GetSettings().QuizMode)
            {
                case Models.AppSettingsModel.EQuizMode.QUESTION_MAX:
                    TimeInfoLabel.Visibility = Visibility.Collapsed;
                    CorrectInfoLabel.Visibility = Visibility.Visible;
                    break;
                case Models.AppSettingsModel.EQuizMode.QUESTION_MIN:
                    TimeInfoLabel.Visibility = Visibility.Collapsed;
                    break;
                case Models.AppSettingsModel.EQuizMode.TIMEOUT:
                    break;
            }
            

        }
        private void OkButton_Clicked(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
