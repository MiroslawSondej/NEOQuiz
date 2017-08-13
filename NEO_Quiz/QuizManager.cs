using NEO_Quiz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEO_Quiz
{
    public class QuizManager
    {
        AppSettingsModel settings;

        List<QuestionModel> QuestionList;
        List<QuestionModel> QuestionUsed;

        public QuizManager(AppSettingsModel settings)
        {
            this.settings = settings;
        }

        public void Begin()
        {

        }
        public void Cancel()
        {

        }
        public void NextQuestion()
        {

        }
        public void CheckAnswer()
        {

        }

        private void OnEnd()
        {

        }
        
    }
}
