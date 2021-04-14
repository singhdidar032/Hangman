using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using SQLite;

namespace Hangman
{
    public class WordsModel : INotifyPropertyChanged
    {
        public WordsModel()
        {

        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; } // Id of the Word
        public string Word { get; set; } // The Word to used in Hangman

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
