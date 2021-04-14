using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Diagnostics;

namespace Hangman
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            //Message and five buttons as btnGame, btnInst, btnProf, btnDB, btnExit.

            // Color line.
            BoxView BoxLine = new BoxView
            {

                Color = Color.DarkGreen

            };

            // Welcome message
            Label Welcome = new Label
            {
                Text = "Welcome to Hangman game!",
                TextColor = Color.Blue,
                FontSize = 25
            };

            // Color line.
            BoxView BoxLine2 = new BoxView
            {

                Color = Color.DarkGreen

            };

            // Game button as btnGame
            Button btnGame = new Button
            {
                Text = "Start the Game!",
                FontSize = 25,
                TextColor = Color.Red
            };
            btnGame.Clicked += StartGame_Clicked;

            // Instruction button as btnInst
            Button btnInst = new Button
            {
                Text = "Instructions",
                FontSize = 25,
                TextColor = Color.Green
            };
            btnInst.Clicked += Instruction_Clicked;

            // Profile button as btnProf
            Button btnProf = new Button
            {
                Text = "Profile",
                FontSize = 25,
                TextColor = Color.Yellow
            };
            btnProf.Clicked += Profile_Clicked;

            // DB button as btnDB
            Button btnDB = new Button
            {
                Text = "DB / Extra",
                FontSize = 25,
                TextColor = Color.Gray
            };
            btnDB.Clicked += DB_Clicked;

            // Exit button as btnExit
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
                    BoxLine2,
                    new StackLayout
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        Children =
                        {
                            btnGame,
                            btnInst,
                            btnProf,
                            btnDB,
                            btnExit
                        }
                    }
                }
            };
        }

        // Button navigations
        public void StartGame_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ProfilePage());
        }

        private void Instruction_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new InstructionPage());
        }

        private void Profile_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ProfilePage());
        }

        private void DB_Clicked(object sender, EventArgs e)
        {

            Navigation.PushAsync(new WordsCRUDPage());
        }

        private void Exit_Clicked(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
