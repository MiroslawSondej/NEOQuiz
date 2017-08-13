using NEO_Quiz.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEO_Quiz.Models
{
    public class AppSettingsModel
    {
        /// <summary>
        /// <para>TIMEOUT = user have to answer as much questions as he can in predefined time</para>
        /// <para>QUESTION_MIN = user have to answer predefined count of questions</para>
        /// <para>QUESTION_MAX = user have at most X question (where X is predefined)</para>
        /// </summary>
        [TypeConverter(typeof(EnumDescriptionTypeConverter))]
        public enum EQuizMode
        {
            [LocalizedDescription("TIMEOUT", typeof(Resources))]
            TIMEOUT,
            [LocalizedDescription("QUESTION_MIN", typeof(Resources))]
            QUESTION_MIN,
            [LocalizedDescription("QUESTION_MAX", typeof(Resources))]
            QUESTION_MAX,
        };

        /// <summary>
        /// Language of app interface and questions
        /// <para>e.g.: "English" or "Polish"</para>
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// List of files containing questions
        /// </summary>
        public string[] ResourcesFile { get; set; }

        /// <summary>
        /// Path to questions resources
        /// </summary>
        public string ResourcesPath { get; set; }


        /// <summary>
        /// See <see cref="EQuizMode"/>
        /// </summary>
        public EQuizMode QuizMode { get; set; }

        /// <summary>
        /// Defined time in timeout mode
        /// </summary>
        public float MaxTime { get; set; }

        /// <summary>
        /// Max questions count in QUESTION_MAX mode or min count in QUESTION_MIN 
        /// </summary>
        public float QuestionsCount { get; set; }


        public AppSettingsModel()
        {
            ResourcesFile = new string[] { };
        }
    }
}
