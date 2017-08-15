using NEO_Quiz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace NEO_Quiz
{
    public class QuizManager : ITimerListener
    {

        public enum EQuizState
        {
            ACTIVE,
            WON,
            LOST,
        };

        AppSettingsManager settingsManager;
        AppSettingsModel settings;

        DispatcherTimer timer;
        int leftTimeInSeconds;

        List<QuestionModel> QuestionList;
        List<QuestionModel> QuestionUsed;
        QuestionModel currentQuestion;

        public int CurrentQuestionNumber { get; private set; }
        public int CorrectQuestionCount { get; private set; }

        private EQuizState QuizState;

        public QuizManager(AppSettingsManager settingsManager)
        {
            this.settingsManager = settingsManager;
            this.settings = settingsManager.GetSettings();

            QuestionList = new List<QuestionModel>();
            QuestionUsed = new List<QuestionModel>();
        }

        public void Begin()
        {
            CurrentQuestionNumber = 0;
            CorrectQuestionCount = 0;

            string[] files = settingsManager.GetQuestionsFileList();
            foreach(string file in files)
            {
                QuestionList.AddRange(QuestionManager.LoadQuestions(file));
            }

            if(settings.QuizMode == AppSettingsModel.EQuizMode.TIMEOUT)
            {
                leftTimeInSeconds = (int)(settings.MaxTime * 60f);
                InitializeTimer();
            }
        }
        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Start();
            timer.Tick += this.OnTimerTick;
        }
        public void RegisterTickListener(ITimerListener listener)
        {
            timer.Tick += listener.OnTimerTick;
        }
        public void UnregisterTickListener(ITimerListener listener)
        {
            timer.Tick -= listener.OnTimerTick;
        }
        public void Cancel()
        {
            if (settings.QuizMode == AppSettingsModel.EQuizMode.TIMEOUT)
            {
                timer.Tick -= this.OnTimerTick;
                timer.Stop();
            }
        }
        public bool HasNextQuestion()
        {
            if(QuestionList.Count > 0)
            {
                return true;
            }
            return false;
        }
        public QuestionModel NextQuestion()
        {
            if(QuestionList.Count == 0)
            {
                if(QuestionUsed.Count > 0)
                {
                    QuestionList.Clear();
                    QuestionList.AddRange(QuestionUsed);
                    QuestionUsed.Clear();
                }
                return null;
            }

            int id = new Random(DateTime.Now.Second).Next(0, QuestionList.Count);

            currentQuestion = QuestionList[id];
            QuestionList.RemoveAt(id);

            CurrentQuestionNumber++;
            return currentQuestion;
        }
        public AppSettingsModel.EQuizMode GetMode()
        {
            return settings.QuizMode;
        }
        public void OnTimerTick(object sender, EventArgs e)
        {
            leftTimeInSeconds -= 1;
        }
        public AppSettingsModel GetSettings()
        {
            return settings;
        }
        public bool CheckAnswer(int answer)
        {
            bool result = false;

            if (currentQuestion.CorrectAnswer == answer)
            {
                CorrectQuestionCount++;
                result = true;
            }
            else
            {
                QuestionUsed.Add(currentQuestion);
                result = false;
            }

            UpdateState();
            return result;
        }
        private void UpdateState()
        {
            if(settings.QuizMode == AppSettingsModel.EQuizMode.QUESTION_MAX)
            {
               if(CurrentQuestionNumber + 1 >= settings.QuestionsCount)
                {
                    QuizState = EQuizState.WON;
                }
            }
            else if (settings.QuizMode == AppSettingsModel.EQuizMode.QUESTION_MIN)
            {
                if(CorrectQuestionCount >= settings.QuestionsCount)
                {
                    QuizState = EQuizState.WON;
                }
            }
            else if (settings.QuizMode == AppSettingsModel.EQuizMode.TIMEOUT)
            {
                if(leftTimeInSeconds <= 0)
                {
                    QuizState = EQuizState.WON;
                }
            }
        }
        public EQuizState GetState()
        {
            return QuizState;
        }
        public int GetSecondsLeft()
        {
            if(leftTimeInSeconds >= 0)
                return leftTimeInSeconds;

            return 0;
        }
        public void StopTimer()
        {
            timer.Stop();
        }
    }
}
