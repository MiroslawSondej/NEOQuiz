using NEO_Quiz.Models;

using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NEO_Quiz.QuestionEditor
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FileStream currentQuestionFile;

        public List<QuestionModel> Question { get; set; }
        public MainWindow()
        {
            currentQuestionFile = null;
            Question = new List<QuestionModel>();

            InitializeComponent();

            QuestionDataGrid.ItemsSource = Question;
        }

        private void NewFileMenuItem_Clicked(object sender, RoutedEventArgs e)
        {
            if (currentQuestionFile != null)
            {
                if(!OnCurrentClose())
                {
                    return;
                }
            }

            Question = new List<QuestionModel>();
            QuestionDataGrid.ItemsSource = Question;
            QuestionDataGrid.Items.Refresh();
        }
        private void SaveFileMenuItem_Clicked(object sender, RoutedEventArgs e)
        {
            string filename = "";
            if (currentQuestionFile == null)
            {
                Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
                dialog.DefaultExt = ".xml";
                dialog.Filter = "Pliki quizu | *.xml";
                bool? result = dialog.ShowDialog();

                if (result == true)
                {
                    filename = dialog.FileName;
                }
                else
                {
                    MessageBox.Show("Nie można otworzyć pliku do zapisu", "Błąd", MessageBoxButton.OK);
                    return;
                }
            }
            else
            {
                filename = currentQuestionFile.Name;
                currentQuestionFile.Close();
            }


            try
            {
                currentQuestionFile = new FileStream(filename, FileMode.Create);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nie można otworzyć pliku do zapisu" + ex, "Błąd", MessageBoxButton.OK);
                return;
            }


            SaveQuestions();
        }
        private void OpenFileMenuItem_Clicked(object sender, RoutedEventArgs e)
        {
            if (currentQuestionFile != null)
            {
                if (!OnCurrentClose())
                {
                    return;
                }
            }

            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.DefaultExt = ".xml";
            dialog.Filter = "Pliki quizu | *.xml";
            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                // Open document 
                string filename = dialog.FileName;
                LoadQuestions(filename);
            }

        }
        private void OpenOldFileMenuItem_Clicked(object sender, RoutedEventArgs e)
        {
            if (currentQuestionFile != null)
            {
                if (!OnCurrentClose())
                {
                    return;
                }
            }

            Question.Clear();
            QuestionDataGrid.Items.Refresh();


            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.DefaultExt = ".txt";
            dialog.Filter = "Stary format | *.txt";
            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                // Open document 
                string filename = dialog.FileName;
                Question.AddRange(LoadOldFormat(filename));
            }
            QuestionDataGrid.Items.Refresh();
        }

        private void ExitFileMenuItem_Clicked(object sender, RoutedEventArgs e)
        {
            SaveFileMenuItem_Clicked(sender, e);
            Close();
        }
        private void DeleteButton_Clicked(object sender, RoutedEventArgs e)
        {
            if (QuestionDataGrid.SelectedIndex >= 0 && QuestionDataGrid.SelectedIndex < Question.Count)
            {
                Question.RemoveAt(QuestionDataGrid.SelectedIndex);
                QuestionDataGrid.Items.Refresh();
            }
        }
        protected override void OnClosed(EventArgs e)
        {
            if (currentQuestionFile != null)
            {
                currentQuestionFile.Close();
            }
            base.OnClosed(e);
        }
        private bool OnCurrentClose()
        {
            MessageBoxResult result = MessageBox.Show("Czy zapisać zmiany w " + currentQuestionFile.Name + "?", "Ostrzeżenie", MessageBoxButton.YesNoCancel);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    try
                    {
                        string filename = currentQuestionFile.Name;
                        currentQuestionFile.Close();
                        currentQuestionFile = new FileStream(filename, FileMode.Create);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Nie można otworzyć pliku do zapisu" + ex, "Błąd", MessageBoxButton.OK);
                        return true;
                    }
                    SaveQuestions();
                    currentQuestionFile.Close();
                    currentQuestionFile = null;
                    break;
                case MessageBoxResult.No:
                    currentQuestionFile.Close();
                    currentQuestionFile = null;
                    return true;
                default:
                    return false;
            }
            return false;
        }
        //==============================
        private void LoadQuestions(string filename)
        {
            try
            {
                currentQuestionFile = new FileStream(filename, FileMode.Open);
                Question = QuestionManager.LoadQuestions(currentQuestionFile);
            }
            catch (Exception e)
            {
                Question = new List<QuestionModel>();
            }
            QuestionDataGrid.ItemsSource = Question;
            QuestionDataGrid.Items.Refresh();
        }
        private void SaveQuestions()
        {
            QuestionManager.SaveQuestions(currentQuestionFile, Question);
            currentQuestionFile.Flush();
        }

        private List<QuestionModel> LoadOldFormat(string filename)
        {
            List<QuestionModel> oldQuestions = new List<QuestionModel>();

            try
            {
                StreamReader reader = new StreamReader(filename);

                int.Parse(reader.ReadLine()); // trash data
                int questionCount = int.Parse(reader.ReadLine());

                for (int i = 0; i < questionCount; i++)
                {
                    QuestionModel tmpQuestion = new QuestionModel();
                    tmpQuestion.QuestionText = reader.ReadLine();

                    for (int j = 0; j < 4; j++)
                        tmpQuestion.Answer[j] = reader.ReadLine();

                    tmpQuestion.CorrectAnswer = int.Parse(reader.ReadLine());
                    tmpQuestion.HasOptionalQuestionImage = false;
                    tmpQuestion.OptionalQuestionImageName = "";

                    oldQuestions.Add(tmpQuestion);
                }
                return oldQuestions;
            }
            catch (Exception e)
            {
                return new List<QuestionModel>();
            }
        }
    }
}
