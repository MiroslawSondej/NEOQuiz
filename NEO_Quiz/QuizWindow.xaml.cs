using NEO_Quiz.Dialogs;
using NEO_Quiz.Models;
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
    public partial class QuizWindow : Window, ITimerListener
    {
        private readonly QuizManager quizManager;
        private readonly AppSettingsModel settings;
        private QuestionModel Question;

        int checkedAnswerId = 0;

        public QuizWindow(QuizManager manager)
        {
            quizManager = manager;
            settings = manager.GetSettings();

            Question = manager.NextQuestion();
            InitializeComponent();

            PrepareHeader();

            PushData();
        }
        protected override void OnClosed(EventArgs e)
        {
            if (settings.QuizMode == AppSettingsModel.EQuizMode.TIMEOUT)
            {
                quizManager.UnregisterTickListener(this);
            }
            base.OnClosed(e);
        }
        private void OnAnyAnswer_Checked(object sender, RoutedEventArgs e)
        {
            if(!CheckAnswerButton.IsEnabled)
                CheckAnswerButton.IsEnabled = true;

            Console.WriteLine(((RadioButton)sender).Name);

            if(((RadioButton)sender).Name == Answer1.Name)
            {
                checkedAnswerId = 1;
            }
            else if (((RadioButton)sender).Name == Answer2.Name)
            {
                checkedAnswerId = 2;
            }
            else if (((RadioButton)sender).Name == Answer3.Name)
            {
                checkedAnswerId = 3;
            }
            else if (((RadioButton)sender).Name == Answer4.Name)
            {
                checkedAnswerId = 4;
            }
        }
        private void CheckAnswer_Clicked(object sender, RoutedEventArgs e)
        {
            CheckAnswer();

            CheckAnswerButton.Visibility = Visibility.Collapsed;
            NextQuestionButton.Visibility = Visibility.Visible;

            PrepareHeader();
        }
        private void NextQuestion_Clicked(object sender, RoutedEventArgs e)
        {
            if(quizManager.GetState() == QuizManager.EQuizState.ACTIVE && quizManager.HasNextQuestion())
            {
                new QuizWindow(quizManager).Show();
                Close();
            }
            else
            {
                OnEnd();
            }
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
        private void PrepareHeader()
        {
            AppSettingsModel.EQuizMode mode = quizManager.GetMode();

            if(mode == AppSettingsModel.EQuizMode.TIMEOUT)
            {
                quizManager.RegisterTickListener(this);
                UpdateTimerHeader();
            }
            else if(mode == AppSettingsModel.EQuizMode.QUESTION_MIN)
            {
                UpdateQuestionMinHeader();
            }
            else if (mode == AppSettingsModel.EQuizMode.QUESTION_MAX)
            {
                UpdateQuestionMaxHeader();
            }
        }
        private void PushData()
        {
            QuestionText.Text = Question.QuestionText;
            Answer1Text.Text = Question.Answer[0];
            Answer2Text.Text = Question.Answer[1];
            Answer3Text.Text = Question.Answer[2];
            Answer4Text.Text = Question.Answer[3];
        }
        private void UpdateQuestionMinHeader()
        {
            InfoHeaderTimeLabelStatic.Visibility = Visibility.Collapsed;
            InfoHeaderCorrectLabelStatic.Visibility = Visibility.Visible;

            InfoHeaderLabelDynamic.Content = quizManager.CorrectQuestionCount + "/" + settings.QuestionsCount;
        }
        private void UpdateQuestionMaxHeader()
        {
            InfoHeaderTimeLabelStatic.Visibility = Visibility.Collapsed;
            InfoHeaderNumberLabelStatic.Visibility = Visibility.Visible;

            InfoHeaderLabelDynamic.Content = quizManager.CurrentQuestionNumber;
        }
        public void UpdateTimerHeader()
        {
            int seconds = quizManager.GetSecondsLeft();
            int minutes = seconds / 60;
            seconds = seconds - (minutes * 60);

            string secondsText = seconds >= 10 ? seconds.ToString() : "0" + seconds;
            string minutesText = minutes >= 10 ? minutes.ToString() : "0" + minutes;

            InfoHeaderLabelDynamic.Content = minutesText + ":" + secondsText;
        }
        public void OnTimerTick(object sender, EventArgs e)
        {
            UpdateTimerHeader();
            if(quizManager.GetSecondsLeft() <= 0)
            {
                quizManager.StopTimer();
                OnEnd();
            }
        }
        private void CheckAnswer()
        {

            AccessText[] tmpRadio = { Answer1Text, Answer2Text, Answer3Text, Answer4Text };
            tmpRadio[Question.CorrectAnswer - 1].Foreground = new SolidColorBrush(Colors.Green);

            if (!quizManager.CheckAnswer(checkedAnswerId))
            {
                tmpRadio[checkedAnswerId - 1].Foreground = new SolidColorBrush(Colors.Red);
            }
            tmpRadio[Question.CorrectAnswer - 1].FontWeight = FontWeights.Heavy;
        } 
        private void OnEnd()
        {
            QuizWonDialog.EQuizWonMethod wonMethod = QuizWonDialog.EQuizWonMethod.MAX_QUESTION_LIMIT_REACHED;

            if (quizManager.GetMode() == AppSettingsModel.EQuizMode.QUESTION_MAX)
            {
                wonMethod = QuizWonDialog.EQuizWonMethod.MAX_QUESTION_LIMIT_REACHED;
            }
            else if (quizManager.GetMode() == AppSettingsModel.EQuizMode.QUESTION_MIN)
            {
                wonMethod = QuizWonDialog.EQuizWonMethod.MIN_QUESTION_REACHED;
            }
            else if (quizManager.GetMode() == AppSettingsModel.EQuizMode.TIMEOUT)
            {
                if (!quizManager.HasNextQuestion())
                {
                    quizManager.StopTimer();
                    wonMethod = QuizWonDialog.EQuizWonMethod.QUESTIONS_LIMIT_END;
                }
                else
                {
                    wonMethod = QuizWonDialog.EQuizWonMethod.TIME_END;
                }
            }

            new QuizWonDialog(wonMethod, quizManager).ShowDialog();
            Close();
        }
    }
}
