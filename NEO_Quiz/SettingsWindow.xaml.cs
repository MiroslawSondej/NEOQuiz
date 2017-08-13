using NEO_Quiz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Logika interakcji dla klasy SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        AppSettingsManager appSettingsManager;
        AppSettingsModel appSettingsModel;


        public SettingsWindow()
        {
            appSettingsManager = DependencyContainer.Instance.GetDependencyInstance<AppSettingsManager>("SettingsManager");
            appSettingsModel = appSettingsManager.GetSettingsCopy();

            InitializeComponent();
            this.DataContext = appSettingsModel;
            QuizModeCombo.ItemsSource =  Enum.GetValues(typeof(AppSettingsModel.EQuizMode)).Cast<AppSettingsModel.EQuizMode>();
        }

        private void SaveButton_Clicked(object sender, RoutedEventArgs e)
        {
            appSettingsManager.ModifySettings(appSettingsModel);
            Close();
        }
        private void CloseButton_Clicked(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void IntNumberValidationTextBox(object sender, RoutedEventArgs e)
        {
            int value;
            if (!int.TryParse(((TextBox)sender).Text, out value))
            {
                ((TextBox)sender).Text = "0";
            }
        }
        private void FloatNumberValidationTextBox(object sender, RoutedEventArgs e)
        {
            float value;
            if (!float.TryParse(((TextBox)sender).Text, out value))
            {
                ((TextBox)sender).Text = "0,0";
            }
        }

    }
}
