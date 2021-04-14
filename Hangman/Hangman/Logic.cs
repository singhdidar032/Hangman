using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Hangman
{
    //MW's Space
    public class Logic
    {
        //The Hidden Word
        string HidWord;

        //Score & GameCount
        public int ScoreCount;
        int GameCount;

        //Gem
        int GemCount;

        //Total Wrong Guesses
        string ThisGameMode;
        int badGuess = 0;
        int deadNum = 12;
        int PointsWorth = 1;

        //HM Image Prefix
        string HMpics;

        int GameState;
        public int GameOnOff = 1;

        //Getting Game Difficulty
        public void SetDiff(string GameMode)
        {
            ScoreCount = 0;
            GameCount = -1;

            ThisGameMode = GameMode;

            if (GameMode == "Easy")
            {
                HMpics = "HME";
                deadNum = 12;
                PointsWorth = 1;
            }
            else if (GameMode == "Medium")
            {
                HMpics = "HMN";
                deadNum = 9;
                PointsWorth = 3;
            }
            else
            {
                HMpics = "HMH";
                deadNum = 6;
                PointsWorth = 7;
            }
        }

        //Makes the Correct Num of '_' for VisWord
        public string MakeBlankChars(string HidWord)
        {
            string VisWord = "";

            for (int i = 0; i < HidWord.Length; i++)
            {
                //If it isn't a letter
                if (!Char.IsLetter(Convert.ToChar(HidWord.Substring(i, 1))))
                {
                    VisWord += HidWord.Substring(i, 1);
                }
                else
                {
                    VisWord += "_";
                }
            }

            return VisWord;
        } //Make Blank Chars ENDS

        public string SpaceString(string MyString)
        {
            string spacedString = "";

            for (int i = 0; i < MyString.Length; i++)
            {
                spacedString += " " + MyString.Substring(i, 1) + " ";
            }

            return spacedString;
        } //Space String ENDS

        public string RemoveSpaces(string CurrVisWord)
        {
            string shortWord1 = "";
            string shortWord2 = "";

            //Finding Spaces
            for (int i = 0; i < CurrVisWord.Length; i++)
            {
                if (CurrVisWord.Substring(i, 1) == " ")
                {
                    shortWord1 += "0";
                }
                else
                {
                    shortWord1 += CurrVisWord.Substring(i, 1);
                }
            }

            //Conserving a Word Space
            shortWord1 = shortWord1.Replace("000", " ");

            //Places Word Space Bck
            for (int i = 0; i < shortWord1.Length; i++)
            {
                if (shortWord1.Substring(i, 1) == "0")
                {
                    shortWord2 += "";
                }
                else
                {
                    shortWord2 += shortWord1.Substring(i, 1);
                }
            }

            return shortWord2;
        } //Remove Spaces ENDS

        public async Task NewHMGame(int UserID, Label ScoreTxt, Label GameTxt, Image HMimg, Label VisWordTxt, Button[] AlphaBtns, Button GemBtn) //Sets Up Hangman Game
        {
            //Adds to Game Count
            GameCount++;
            GameTxt.Text = "Games Won: " + GameCount;

            //Showing the Total Score & Gems
            ScoreTxt.Text = Convert.ToString(ScoreCount);

            //Getting User's Gems and Adding them
            await GetUserGems(UserID, 0);

            GemBtn.Text = "X " + GemCount;

            //Getting New Word
            HidWord = await NewWord();

            //Ensures Letters are ALL Capitals
            HidWord = HidWord.ToUpper();

            //Showing the Visable Word Letter Count
            VisWordTxt.Text = SpaceString(MakeBlankChars(HidWord));

            //Enabling Gems 1++
            if (GemCount > 0)
            {
                GemBtn.IsEnabled = true;
                GemBtn.BackgroundColor = Color.White;
            }
            else //Disabling Gems 0
            {
                GemBtn.IsEnabled = false;
                GemBtn.BackgroundColor = Color.LightGray;
            }

            //Re-Enabling ALL Alphabet Btns
            for (int i = 0; i < AlphaBtns.Length; i++)
            {
                AlphaBtns[i].IsEnabled = true;
                AlphaBtns[i].BackgroundColor = Color.White;
            }

            //Showing Img, Score & set bad Guesses to Zero
            // + HMpics
            HMimg.Source = HMpics + 1 + ".png";
            ScoreTxt.Text = "Score: " + ScoreCount;
            badGuess = 0;

            //New Game Starts
            GameState = 1;
        } //NewHMGame ENDS

        //Guessing Char
        public async Task GuessChar(object sender, EventArgs e, int UserID, Label ScoreTxt, Label GameTxt, Image HMimg, Label VisWordTxt, Button[] AlphaBtns, Button GemBtn)
        {
            //Only While Game is Active
            if (sender is Button btn && GameState == 1)
            {
                btn.IsEnabled = false;

                //The Char Exists in HidWord
                if (HidWord.Contains(btn.Text) == true)
                {
                    //Background is greenish for correct input
                    btn.BackgroundColor = Color.FromRgb(223, 236, 223);

                    string NewVisWord = "";
                    string VisWord = RemoveSpaces(VisWordTxt.Text);

                    for (int i = 0; i < HidWord.Length; i++)
                    {
                        if (HidWord.Substring(i, 1) == btn.Text)
                        {
                            NewVisWord += btn.Text;
                        }
                        else
                        {
                            NewVisWord += VisWord.Substring(i, 1);
                        }
                    }

                    VisWordTxt.Text = SpaceString(NewVisWord);

                    //If all Letters are Found
                    if (NewVisWord.Contains("_") == false)
                    {
                        //Game Over
                        GameState = 0;
                        await GameEnd(UserID, 1, ScoreTxt, GameTxt, HMimg, VisWordTxt, AlphaBtns, GemBtn);
                    }

                } //The Char Exists in HidWordENDS
                else
                {
                    badGuess++;

                    //No Lives are left
                    if ((badGuess + 1) == deadNum)
                    {
                        HMimg.Source = "HMDead.png";
                        VisWordTxt.Text = SpaceString(HidWord);

                        //Background is redish for incorrect input
                        btn.BackgroundColor = Color.FromRgb(255, 102, 102);

                        //Dead Char Changes to badGuess Color
                        await Task.Delay(1000);
                        btn.BackgroundColor = Color.FromRgb(236, 223, 223);

                        //Game Over
                        GameState = 0;
                        await GameEnd(UserID, 0, ScoreTxt, GameTxt, HMimg, VisWordTxt, AlphaBtns, GemBtn);
                    }
                    else
                    {
                        HMimg.Source = HMpics + (badGuess + 1) + ".png";

                        //Background is redish for incorrect input
                        btn.BackgroundColor = Color.FromRgb(236, 223, 223);
                    }
                }
            }
        } //GuessChar ENDS

        public async Task GetUserGems(int UserID, int ChngeGem)
        {
            //Getting Users Gems
            PlayerModel player = new PlayerModel();
            player = await App.Database.GetPlayerAsync(UserID);

            GemCount = player.Gems;

            //Update Users Gems
            if (ChngeGem != 0)
            {
                GemCount = (player.Gems + ChngeGem);

                await App.Database.SavePlayerAsync(new PlayerModel
                {
                    Id = UserID,
                    UserName = player.UserName,
                    NameOfPlayer = player.NameOfPlayer,
                    AvatarOfPlayer = player.AvatarOfPlayer,
                    Gems = GemCount,
                    BestScore = player.BestScore
                });
            }
        } //Get User's Gems ENDS

        //Using Gem for Hint
        public async Task UseGem(object sender, EventArgs e, int UserID, Label ScoreTxt, Label GameTxt, Image HMimg, Label VisWordTxt, Button[] AlphaBtns)
        {
            //Only While Game is Active
            if (sender is Button GemBtn && GameState == 1 && GemCount > 0)
            {
                //Uses A Gem
                await GetUserGems(UserID, -1);
                GemBtn.Text = "X " + GemCount;

                //Disables Btn if No Gems Left
                if (GemCount <= 0)
                {
                    GemBtn.IsEnabled = false;
                    GemBtn.BackgroundColor = Color.LightGray;
                }

                //Giving Hint
                string PossHint = "";
                string NewVisWord = "";
                Random luck = new Random();

                //What User has Currently
                string VisWord = RemoveSpaces(VisWordTxt.Text);

                //Getting Not Used Letters
                for (int i = 0; i < HidWord.Length; i++)
                {
                    //If Characters is still Unknown and not a current PossHint
                    if (VisWord.Substring(i, 1) == "_" && PossHint.Contains(VisWord.Substring(i, 1)) == false)
                    {
                        PossHint += HidWord.Substring(i, 1);
                    }
                }
                //Getting Not Used Letters ENDS

                //Selecting Hint Character
                PossHint = PossHint.Substring(luck.Next(PossHint.Length), 1);

                //Revealing ALL of Character
                for (int i = 0; i < HidWord.Length; i++)
                {
                    if (HidWord.Substring(i, 1) == PossHint)
                    {
                        NewVisWord += PossHint;
                    }
                    else
                    {
                        NewVisWord += VisWord.Substring(i, 1);
                    }
                }
                //Revealing ALL of Character ENDS

                VisWord = NewVisWord;
                VisWordTxt.Text = SpaceString(VisWord);

                //Disabling Character's Btn
                int CharValue = Convert.ToInt32(Convert.ToChar(PossHint)) - 65;
                AlphaBtns[CharValue].IsEnabled = false;
                AlphaBtns[CharValue].BackgroundColor = Color.FromRgb(223, 236, 223);
                //Giving Hint ENDS

                //If all Letters are Found
                if (VisWord.Contains("_") == false)
                {
                    //Game Over
                    GameState = 0;
                    await GameEnd(UserID, 1, ScoreTxt, GameTxt, HMimg, VisWordTxt, AlphaBtns, GemBtn);
                }
            }
        } //UseGem ENDS

        public async Task GameEnd(int UserID, int gameResult, Label ScoreTxt, Label GameTxt, Image HMimg, Label VisWordTxt, Button[] AlphaBtns, Button GemBtn)
        {
            //Shows Game Result
            await GameResult(UserID, gameResult, ScoreTxt, HMimg, VisWordTxt, AlphaBtns, GemBtn);

            //New Hangman Game
            if (gameResult == 1)
            {
                GameOnOff = 1;

                await NewHMGame(UserID, ScoreTxt, GameTxt, HMimg, VisWordTxt, AlphaBtns, GemBtn);
            }
            else //GAME OVER
            {
                GameOnOff = 0;
            }
        }

        //Little Fun Result 'Animation'
        public async Task GameResult(int UserID, int gameResult, Label ScoreTxt, Image HMimg, Label VisWordTxt, Button[] AlphaBtns, Button GemBtn)
        {
            int GemsEarned = 0;

            if (gameResult == 1)
            {
                HidWord = "You Win!";

                //Getting this Rounds Total Points
                int RoundPoints = ((deadNum - 1) - badGuess) * PointsWorth;

                //Adding to the Total Score
                ScoreCount = ScoreCount + RoundPoints;

                //Adding to Users Gems
                GemsEarned = RoundPoints / 10;
                await GetUserGems(UserID, GemsEarned);
            }
            else
            {
                HidWord = "Game Over";
            }
            //Ensures Capitals are Used
            HidWord = HidWord.ToUpper();


            //Gives User Time to See the HM Word
            await Task.Delay(1000);

            //Showing Gems Earned
            if (GemsEarned > 0)
            {
                switch (GemsEarned)
                {
                    case 1: HMimg.Source = "HMGem.png"; break;
                    case 2: HMimg.Source = "HMGem2.png"; break;
                    case 3: HMimg.Source = "HMGem3.png"; break;
                }

                GemBtn.Text = "+ " + GemsEarned;
            }

            //Reveals New Word
            string VisWord = SpaceString(MakeBlankChars(HidWord));
            VisWordTxt.Text = VisWord;

            //Reveal Results Character by Character
            for (int i = 1; i < HidWord.Length; i++)
            {
                //Only Wait to Reveal Characters
                if (Char.IsLetter(Convert.ToChar(HidWord.Substring((i - 1), 1))))
                {
                    await Task.Delay(200);
                }

                VisWordTxt.Text = SpaceString(HidWord.Substring(0, i) + VisWord.Substring(i, (HidWord.Length - i)));
            }

            await Task.Delay(200);
            VisWordTxt.Text = SpaceString(HidWord);

            //Gives User Time to See Result
            await Task.Delay(1000);
        } //Little Fun Result 'Animation' ENDS

        //Selects a Random Word from DB
        public async Task<string> NewWord()
        {
            int RandID = 0;
            Random randomWord = new Random();
            WordsModel word = new WordsModel();

            var condition = "Empty";
            while (condition == "Empty")
            {
                RandID = randomWord.Next(1, App.Database.GetWordsAsync().Result.Max(x => x.Id) + 1);
                condition = $"{await App.Database.CheckRandomID(RandID)}";
            }
            word = App.Database.GetWordAsync(RandID).Result;
            return word.Word;
        } //Random Word ENDS

        //Get User's Name from DB
        public async void GetPlayerName(int UserID, Label UserNameTxt)
        {
            PlayerModel player = new PlayerModel();
            player = await App.Database.GetPlayerAsync(UserID);

            UserNameTxt.Text = player.UserName;
        }
        //Get Player Name ENDS

        //Check/Updates Users High Score
        public async void CheckHiScores(int UserID, int GameScore, Label HighScore)
        {
            //Getting User Info
            PlayerModel player = new PlayerModel();
            player = await App.Database.GetPlayerAsync(UserID);

            int CurrHighScore = player.BestScore;

            //If User made new High Score
            if (CurrHighScore < GameScore) {

                await App.Database.SavePlayerAsync(new PlayerModel
                {
                    Id = UserID,
                    UserName = player.UserName,
                    NameOfPlayer = player.NameOfPlayer,
                    AvatarOfPlayer = player.AvatarOfPlayer,
                    Gems = player.Gems,
                    BestScore = GameScore
                });

                HighScore.Text = "New High Score!";
            }
        } //Check Hi Scores ENDS
    }
}