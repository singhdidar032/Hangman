using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Hangman
{
    public class HangManModel : INotifyPropertyChanged
    {
        public HangManModel()
        {
        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; } // Id for the hangman Game
        public string NameOfPlayer { get; set; } // The Name of the Player 
        public string Difficulty { get; set; } // The Game Difficulty
        public string StateOfGame { get; set; } // Keeps track of when the Game should end
        public int Score { get; set; } // The Current Score 

        public event PropertyChangedEventHandler PropertyChanged; // helps with BindingContext with this model

        // ForeignKey for PlayerModel
        [OneToMany(CascadeOperations = CascadeOperation.CascadeRead)]
        public PlayerModel childPlayerModel { get; set; }

        [ForeignKey(typeof(PlayerModel))]
        public int PlayerModelID { get; set; }
    }
}

