using NEO_Quiz.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace NEO_Quiz
{
    public class QuestionManager
    {
        public static List<QuestionModel> LoadQuestions(string pathToFile)
        {
            try
            {
                FileStream fileStream = new FileStream(pathToFile, FileMode.Open);
                List<QuestionModel> model = LoadQuestions(fileStream);
                fileStream.Close();
                return model;
            }
            catch(Exception e)
            {
                return new List<QuestionModel>();
            }
        }
        public static List<QuestionModel> LoadQuestions(FileStream fileStream)
        {
            try
            {
                List<QuestionModel> question = new List<QuestionModel>();

                XmlReader xmlReader = XmlReader.Create(fileStream);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(QuestionModel[]));
                question.AddRange((QuestionModel[])xmlSerializer.Deserialize(xmlReader));
                return question;
            }
            catch (Exception e)
            {
                return new List<QuestionModel>();
            }
        }
        public static bool SaveQuestions(FileStream fileStream, List<QuestionModel> model)
        {
            try
            {
                XmlWriter xmlWriter = XmlWriter.Create(fileStream);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(QuestionModel[]));
                xmlSerializer.Serialize(xmlWriter, model.ToArray());
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        public static bool SaveQuestions(string pathToFile, List<QuestionModel> model)
        {
            try
            {
                FileStream fileStream = new FileStream(pathToFile, FileMode.Create);
                bool result = SaveQuestions(fileStream, model);
                fileStream.Close();
                return result;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
