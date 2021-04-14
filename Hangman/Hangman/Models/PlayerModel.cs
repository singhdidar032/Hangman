using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Hangman
{
    public class PlayerModel : INotifyPropertyChanged
    {
        public PlayerModel()
        {

        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; } // id for Player Profile 
        public string UserName { get; set; } // player Username
        public string NameOfPlayer { get; set; } // player Name
        public string AvatarOfPlayer { get; set; } // Player Avatar
        public int Gems { get; set; } // Player gems to reveal characters in the word
        public int BestScore { get; set; } // the best score the player received form one of the game the player played

        public event PropertyChangedEventHandler PropertyChanged; // helps with BindingContext with this model

        // ForeignKey for HangmanModel
        [OneToMany(CascadeOperations = CascadeOperation.CascadeRead)]
        public HangManModel hangmanModel { get; set; }

        [ForeignKey(typeof(HangManModel))]
        public int HangmanModelID { get; set; }
    }
}
