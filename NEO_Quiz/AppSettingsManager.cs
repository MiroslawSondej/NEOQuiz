using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

using NEO_Quiz.Models;
using System.Text.RegularExpressions;

namespace NEO_Quiz
{
    public class AppSettingsManager
    {
        // Default settings
        private const string DEFAULT_LANGUAGE = "Polish";
        private const AppSettingsModel.EQuizMode DEFAULT_MODE = AppSettingsModel.EQuizMode.QUESTION_MAX;
        private const int DEFAULT_QUESTIONS_COUNT = 10;
        private const float DEFAULT_MAX_TIME = 30f;
        private const string DEFAULT_RESOURCE_PATH = "Questions/";
        private const string DEFAULT_SETTINGS_FILE_NAME = "settings.xml";
        //------------------
        private AppSettingsModel settings;

        public AppSettingsManager()
        {
            if(!LoadSettings(DEFAULT_SETTINGS_FILE_NAME))
            {
                CreateDefaultSettings(DEFAULT_SETTINGS_FILE_NAME);
            }
            settings.ResourcesFile = GetResourceFilesList();
            UpdateLanguage();
        }
        public bool CreateDefaultSettings(string settingsFileName)
        {
            settings = new AppSettingsModel()
            {
                Language = DEFAULT_LANGUAGE,
                MaxTime = DEFAULT_MAX_TIME,
                QuestionsCount = DEFAULT_QUESTIONS_COUNT,
                QuizMode = DEFAULT_MODE,
                ResourcesPath = DEFAULT_RESOURCE_PATH,
            };

            return SaveSettings(settingsFileName);
        }
        public bool LoadSettings(string settingsFileName)
        {
            string tmpLanguage = DEFAULT_LANGUAGE;
            AppSettingsModel.EQuizMode tmpMode = DEFAULT_MODE;
            int tmpQuestionCount = DEFAULT_QUESTIONS_COUNT;
            float tmpMaxTime = DEFAULT_MAX_TIME;
            string tmpResourcesPath = DEFAULT_RESOURCE_PATH;

            try
            {
                FileStream fileStream = new FileStream(settingsFileName, FileMode.Open);
                using (XmlReader xmlReader = XmlReader.Create(fileStream))
                {
                    while (xmlReader.Read())
                    {
                        if (xmlReader.NodeType == XmlNodeType.Element)
                        {
                            switch(xmlReader.Name)
                            {
                                case "ResourcesPath":
                                    {
                                        xmlReader.Read();
                                        tmpResourcesPath = xmlReader.Value;
                                        break;
                                    }

                                case "Language":
                                    {
                                        xmlReader.Read();
                                        tmpLanguage = xmlReader.Value;
                                        break;
                                    }

                                case "MaxTime":
                                    {
                                        xmlReader.Read();
                                        float value = 0.0f;
                                        if (float.TryParse(xmlReader.Value, out value))
                                        {
                                            tmpMaxTime = value;
                                        }
                                        break;
                                    }
                                case "QuestionsCount":
                                    {
                                        xmlReader.Read();
                                        int value = 0;
                                        if (int.TryParse(xmlReader.Value, out value))
                                        {
                                            tmpQuestionCount = value;
                                        }
                                        break;
                                    }
                                case "QuizMode":
                                    {
                                        xmlReader.Read();
                                        AppSettingsModel.EQuizMode value;
                                        if (Enum.TryParse<AppSettingsModel.EQuizMode>(xmlReader.Value, out value))
                                        {
                                            tmpMode = value;
                                        }
                                        break;
                                    }
                            }
                        }
                    }
                }     
            }
            catch (Exception e)
            {
                return false;
            }

            settings = new AppSettingsModel()
            {
                Language = tmpLanguage,
                MaxTime = tmpMaxTime,
                QuestionsCount = tmpQuestionCount,
                QuizMode = tmpMode,
                ResourcesPath = tmpResourcesPath,
            };

            return true;
        }
        private string[] GetResourceFilesList()
        {
            string[] files = Directory.GetFiles(DEFAULT_RESOURCE_PATH);
            return files;
        }
        public AppSettingsModel GetSettings()
        {
            if(settings == null)
            {
                LoadSettings(DEFAULT_SETTINGS_FILE_NAME);
            }
            return settings;
        }
        public AppSettingsModel GetSettingsCopy()
        {
            AppSettingsModel settingsCopy = new AppSettingsModel()
            {
                Language = settings.Language,
                MaxTime = settings.MaxTime,
                QuestionsCount = settings.QuestionsCount,
                QuizMode = settings.QuizMode,
                ResourcesFile = settings.ResourcesFile,
                ResourcesPath = settings.ResourcesPath,
            };
            return settingsCopy;
        }
        public void ModifySettings(AppSettingsModel newSettings)
        {
            settings.Language = newSettings.Language;
            settings.MaxTime = newSettings.MaxTime;
            settings.QuestionsCount = newSettings.QuestionsCount;
            settings.QuizMode = newSettings.QuizMode;
            settings.ResourcesFile = newSettings.ResourcesFile;
            settings.ResourcesPath = newSettings.ResourcesPath;

            SaveSettings(DEFAULT_SETTINGS_FILE_NAME);
        }
        public bool SaveSettings(string settingsFileName)
        {
            try
            {
                FileStream fileStream = new FileStream(settingsFileName, FileMode.Create);
                using (XmlWriter xmlWriter = XmlWriter.Create(fileStream))
                {
                    xmlWriter.WriteStartDocument();
                    xmlWriter.WriteStartElement("Settings");

                    xmlWriter.WriteStartElement("ResourcesPath");
                    xmlWriter.WriteString(DEFAULT_RESOURCE_PATH);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("Language");
                    xmlWriter.WriteString(settings.Language);
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("MaxTime");
                    xmlWriter.WriteString(settings.MaxTime.ToString());
                    xmlWriter.WriteEndElement();


                    xmlWriter.WriteStartElement("QuestionsCount");
                    xmlWriter.WriteString(settings.QuestionsCount.ToString());
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("QuizMode");
                    xmlWriter.WriteString(settings.QuizMode.ToString());
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndDocument();

                    xmlWriter.Close();
                }
                fileStream.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }
        public void UpdateLanguage()
        {
            ChangeLanguage(settings.Language);
        }
        public void ChangeLanguage(string language)
        {
            string lang = "pl";
            switch(language)
            {
                case "English":
                    lang = "en";
                    break;
                default:
                    lang = "pl";
                    break;
            }

            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(lang);
        }
        public string[] GetQuestionsFileList()
        {
            string currentLanguage = "pl";
            if (settings.Language == "English") currentLanguage = "eng";

            List<string> fileList = new List<string>();
            
            foreach(string file in settings.ResourcesFile)
            {
                if(Regex.IsMatch(file, ".+(." + currentLanguage + ".xml)"))
                {
                    fileList.Add(file);
                }
            }
            return fileList.ToArray();
        }
    }
}
