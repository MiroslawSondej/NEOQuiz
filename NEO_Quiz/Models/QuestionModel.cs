using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEO_Quiz.Models
{
    public class QuestionModel
    {
        public string QuestionText { get; set; }
        public string[] Answer { get; set; }
        public int CorrectAnswer { get; set; }
        public bool HasOptionalQuestionImage { get; set; }
        public string OptionalQuestionImageName { get; set; }

        public QuestionModel()
        {
            Answer = new string[4] { "", "", "", "" };
        }
    }
}
