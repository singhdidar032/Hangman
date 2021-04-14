using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Hangman
{
    public class WordCell : ViewCell
    {
        public WordCell() 
        { 
            var grid = new Grid();
            var wordLabel = new Label();

            wordLabel.SetBinding(Label.TextProperty, "Word");
            grid.Children.Add(wordLabel);

            View = grid;
        }
    }
}
