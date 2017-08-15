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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NEO_Quiz
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AppSettingsManager settingsManager;
        Uri LanguageImageUri;

        bool onLanguageChange = false;

        public MainWindow()
        {
            settingsManager = DependencyContainer.Instance.GetDependencyInstance<AppSettingsManager>("SettingsManager");

            InitializeComponent();

            string language = settingsManager.GetSettings().Language;
            ChangeLanguageIcon(language);
        }

        public void BeginButton_Clicked(object sender, RoutedEventArgs e)
        {
            QuizManager manager = new QuizManager(settingsManager);
            manager.Begin();

            new QuizWindow(manager).Show();
        }
        private void SettingsButton_Clicked(object sender, RoutedEventArgs e)
        {
            if(Application.Current.Windows.OfType<SettingsWindow>().Any())
            {
                Application.Current.Windows.OfType<SettingsWindow>().First().Activate();
            }
            else
            {
                new SettingsWindow().Show();
            }
        }
        private void LanguageButton_Clicked(object sender, RoutedEventArgs e)
        {
            AppSettingsModel settings = settingsManager.GetSettings();
            string language = settings.Language;

            if (string.Equals(language, "English"))
            {
                language = "Polish";
            }
            else if(string.Equals(language, "Polish"))
            {
                language = "English";
            }
            else
            {
                language = "Polish";
            }

            settingsManager.GetSettings().Language = language;
            settingsManager.UpdateLanguage();
            settingsManager.ModifySettings(settings);
            ChangeLanguageIcon(language);

            onLanguageChange = true;
            new MainWindow().Show();
            Close();
        }
        private void CloseButton_Clicked(object sender, RoutedEventArgs e)
        {
            Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            if(!onLanguageChange)
            {
                onLanguageChange = false;
                Application.Current.Shutdown();
            }
            else
            {
                onLanguageChange = false;
                base.OnClosed(e);
            }
        }

        private void ChangeLanguageIcon(string language)
        {
            switch (language)
            {
                case "English":
                    {
                        LanguageImageUri = new Uri(@"Resources\LangIcons\English.png", UriKind.Relative);
                        break;
                    }
                default:
                    {
                        LanguageImageUri = new Uri(@"Resources\LangIcons\Polish.png", UriKind.Relative);
                        break;
                    }
            }
            LanguageButtonImage.Source = new BitmapImage(LanguageImageUri);
        }
    }
}
