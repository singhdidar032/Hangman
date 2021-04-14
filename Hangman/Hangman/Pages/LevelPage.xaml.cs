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
    public partial class LevelPage : ContentPage
    {
        public LevelPage(int UserID)
        {
            InitializeComponent();

            Label Level = new Label
            {
                Text = "Here are levels to choose!",
                TextColor = Color.Blue,
                FontSize = 25
            };
            Button btnEasy = new Button
            {
                Text = "Easy",
                FontSize = 25,
                TextColor = Color.Green
            };
            btnEasy.Clicked += (object sender, EventArgs e) =>
            {
                btnLevelPick_Clicked(sender, e, UserID);
            };

            Button btnMed = new Button
            {
                Text = "Medium",
                FontSize = 25,
                TextColor = Color.Yellow
            };
            btnMed.Clicked += (object sender, EventArgs e) =>
            {
                btnLevelPick_Clicked(sender, e, UserID);
            };

            Button btnHard = new Button
            {
                Text = "Hard",
                FontSize = 25,
                TextColor = Color.Red
            };
            btnHard.Clicked += (object sender, EventArgs e) =>
            {
                btnLevelPick_Clicked(sender, e, UserID);
            };

            Content = new StackLayout
            {
                Children =
                {
                    Level,
                    new StackLayout
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        Children =
                        {
                            btnEasy,
                            btnMed,
                            btnHard,
                        }
                    }
                }
            };
        }

        //Sending Data to HM page
        private async void btnLevelPick_Clicked(object sender, EventArgs e, int UserID)
        {
            if (sender is Button btn)
            {
                //Pushing GameMode to HMGame
                string Diff = btn.Text;

                await Navigation.PushAsync(new HangManPage(UserID, Diff));
            }
        }

        protected override void OnDisappearing()
        {
            
        }
    }
}