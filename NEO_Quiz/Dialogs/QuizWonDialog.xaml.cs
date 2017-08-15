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
        public enum EQuizWonMethod
        {
            TIME_END,
            QUESTIONS_LIMIT_END,
            MAX_QUESTION_LIMIT_REACHED,
            MIN_QUESTION_REACHED,
        };

        public QuizWonDialog(EQuizWonMethod method, QuizManager manager)
        {
            InitializeComponent();

            switch (method)
            {
                case EQuizWonMethod.TIME_END:
                    {
                        HeaderInfo.Text = Properties.Resources.QuizWonTimeEndInfoHeader;

                        string content = Properties.Resources.QuizWonTimeEndInfoContent;
                        content = content.Replace("{0}", manager.CorrectQuestionCount.ToString());
                        ContentInfo.Text = content;

                        break;
                    }
                case EQuizWonMethod.QUESTIONS_LIMIT_END:
                    {
                        HeaderInfo.Text = Properties.Resources.QuizWonTimeQuestonEndInfoHeader;
                        ContentInfo.Visibility = Visibility.Collapsed;
                        break;
                    }
                case EQuizWonMethod.MIN_QUESTION_REACHED:
                    {
                        HeaderInfo.Text = Properties.Resources.QuizWonMinModeInfoHeader;

                        string content = Properties.Resources.QuizWonMinModeInfoContent;
                        content = content.Replace("{0}", manager.CorrectQuestionCount.ToString());
                        content = content.Replace("{1}", manager.CurrentQuestionNumber.ToString());
                        ContentInfo.Text = content;

                        break;
                    }
                case EQuizWonMethod.MAX_QUESTION_LIMIT_REACHED:
                    {
                        HeaderInfo.Text = Properties.Resources.QuizWonMaxModeInfoHeader;

                        string content = Properties.Resources.QuizWonMaxModeInfoContent;
                        content = content.Replace("{0}", manager.CorrectQuestionCount.ToString());
                        ContentInfo.Text = content;
                        break;
                    }
            }
            

        }
        private void OkButton_Clicked(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
