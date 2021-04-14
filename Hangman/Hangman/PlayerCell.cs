using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Hangman
{
    public class PlayerCell : ViewCell
    {
        public PlayerCell()
        {
            ////instantiate each of our views
            //StackLayout cellWrapper = new StackLayout();
            //StackLayout horizontalLayout = new StackLayout();
            //Label userNameLBL = new Label();
            //Label nameOfPlayerLBL = new Label();
            //Label bestScoreLBL = new Label();
            //Label gemsLBL = new Label();

            ////set bindings
            //userNameLBL.SetBinding(Label.TextProperty, "UserName");
            //nameOfPlayerLBL.SetBinding(Label.TextProperty, "NameOfPlayer");
            //bestScoreLBL.SetBinding(Label.TextProperty, "Gems");
            //gemsLBL.SetBinding(Label.TextProperty, "BestScore");


            ////Set properties for desired design
            //cellWrapper.BackgroundColor = Color.FromHex("#eee");
            //horizontalLayout.Orientation = StackOrientation.Horizontal;
            //nameOfPlayerLBL.HorizontalOptions = LayoutOptions.EndAndExpand;
            //userNameLBL.TextColor = Color.FromHex("#f35e20");
            //nameOfPlayerLBL.TextColor = Color.FromHex("503026");
            //bestScoreLBL.TextColor = Color.FromHex("#f35e20");
            //gemsLBL.TextColor = Color.FromHex("#f35e20");

            ////add views to the view hierarchy
            //horizontalLayout.Children.Add(userNameLBL);
            //horizontalLayout.Children.Add(nameOfPlayerLBL);
            //horizontalLayout.Children.Add(bestScoreLBL);
            //horizontalLayout.Children.Add(gemsLBL);
            //cellWrapper.Children.Add(horizontalLayout);
            //View = cellWrapper;


            // Website: microsoft Documentation
            // Title: Creating a Xamarin.Forms DataTemplate
            // URL: https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/templates/data-templates/creating

            //instantiate each of our views
            var grid = new Grid();
            var userNameLabel = new Label();
            var nameOfPlayerLabel = new Label { HorizontalTextAlignment = TextAlignment.Center };
            var bestScoreLabel = new Label { HorizontalTextAlignment = TextAlignment.Center };
            var gemsLabel = new Label { HorizontalTextAlignment = TextAlignment.Center };
            var Avatar = new Image();

            //set bindings
            userNameLabel.SetBinding(Label.TextProperty, "UserName");
            nameOfPlayerLabel.SetBinding(Label.TextProperty, "NameOfPlayer");
            bestScoreLabel.SetBinding(Label.TextProperty, "BestScore");
            gemsLabel.SetBinding(Label.TextProperty, "Gems");
            Avatar.SetBinding(Image.SourceProperty, "AvatarOfPlayer");

            //add views to the view hierarchy
            grid.Children.Add(userNameLabel);
            grid.Children.Add(nameOfPlayerLabel, 1, 0);
            grid.Children.Add(bestScoreLabel, 2, 0);
            grid.Children.Add(gemsLabel, 3, 0);
            grid.Children.Add(Avatar, 4, 0);
            View = grid;
        }
    }
}
