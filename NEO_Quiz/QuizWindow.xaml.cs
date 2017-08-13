using NEO_Quiz.Dialogs;
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

namespace NEO_Quiz
{
    /// <summary>
    /// Logika interakcji dla klasy QuizWindow.xaml
    /// </summary>
    public partial class QuizWindow : Window
    {
        private readonly QuizManager quizManager;

        public QuizWindow(QuizManager manager)
        {
            quizManager = manager;
            InitializeComponent();
        }

        public void CancelQuiz_Clicked(object sender, RoutedEventArgs e)
        {
            bool? result = new CancelQuizDialog().ShowDialog();

            if (result == true)
            {
                quizManager.Cancel();
                Close();
            }
        }
    }
}
