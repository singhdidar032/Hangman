using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Hangman
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GameOverPage : ContentPage
    {

        Logic LogicClass;

        public GameOverPage(int UserID, string Diff, int TotalScore)
        {
            InitializeComponent();

            LogicClass = new Logic();

            //Prevents Players Back Btn
            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetHasNavigationBar(this, false);

            //Message, Entry as GOName, two Lables of GOLevel, GOScore and two Buttons as btnAgain, btnExit.

            BoxView BoxLine = new BoxView
            {

                Color = Color.DarkKhaki

            };

            Label Welcome = new Label
            {
                Text = "GAME OVER!",
                TextColor = Color.Red,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                FontSize = 25
            };
            BoxView BoxLine2 = new BoxView
            {

                Color = Color.DarkKhaki

            };
            Label GOName = new Label
            {
                Text = "",
                FontSize = 25,
                TextColor = Color.Blue,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            Label GOLevel = new Label
            {
                Text = Diff,
                FontSize = 25,
                TextColor = Color.Green,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            Label GOscore = new Label
            {
                Text = "Score: " + Convert.ToString(TotalScore),
                FontSize = 25,
                TextColor = Color.DarkCyan,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            Label GOHighScore = new Label
            {
                Text = "",
                FontSize = 25,
                TextColor = Color.DarkCyan,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            Button btnAgain = new Button
            {
                Text = "Play again!",
                FontSize = 25,
                TextColor = Color.Black
            };
            btnAgain.Clicked += (object sender, EventArgs e) =>
             {
                 Again_Clicked(sender, e, UserID);
             };

            Button btnExit = new Button
            {
                Text = "Exit",
                FontSize = 25,
                TextColor = Color.Black
            };
            btnExit.Clicked += Exit_Clicked;

            Content = new StackLayout
            {
                Children =
                {
                    BoxLine,
                    Welcome,
                    GOName,
                    GOLevel,
                    GOscore,
                    GOHighScore,
                    BoxLine2,
                    new StackLayout
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        Children =
                        {
                            btnAgain,
                            btnExit,
                        }
                    }
                }
            };

            //Adds In User Name on Page Load
            LogicClass.GetPlayerName(UserID, GOName);
            LogicClass.CheckHiScores(UserID, TotalScore, GOHighScore);
        }
        private void Exit_Clicked(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        public async void Again_Clicked(object sender, EventArgs e, int UserID)
        {
            await Navigation.PushAsync(new LevelPage(UserID));
        }
    }
}