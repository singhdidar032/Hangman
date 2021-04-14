using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Hangman.Data;
using System.IO;

namespace Hangman
{

    public partial class App : Application
    {
        static HangmanDatabase database;

        // locate Database and access the database anywhere in the app
        public static HangmanDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new HangmanDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Hangman.db3"));
                }
                return database;
            }
        }
        public App()
        {
            InitializeComponent();

            // allows navigation in the app by making the mainpage the root 
            // from where all page can navigate from
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}