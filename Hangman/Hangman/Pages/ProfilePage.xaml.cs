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
    public partial class ProfilePage : ContentPage
    {
        // Elements For CRUD operations
        Entry entryPlayerName;
        Entry entryUserName;
        Entry entryPlayerGender;
        Label bestScoreLbL;
        Label gemsLbL;
        Button saveBTN;
        Button deleteBTN;

        // Select Player Profile for your Hangman game
        Button selectPlayerBTN;

        //PlayerListViews Variables (For Logic)
        ListView playerListView;
        int SelectedPlayerIndex = 0; // Get DB ID of the Selected Player form ListView
        Boolean isSelectedPlayer = false; // Keep Track When the Player is selected (in order to Deselect/ select)
        List<int> players = new List<int>(); // store ids from DB

        // Headers For the PlayerListView 
        Label hdNameLabel = new Label { Text = "UserName", FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Subtitle, typeof(Label)), VerticalTextAlignment = TextAlignment.Center, HeightRequest = 20 };
        Label hdLastNameLabel = new Label { Text = "Player", FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Subtitle, typeof(Label)), HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center, HeightRequest = 20 };
        Label hdBestScoreLabel = new Label { Text = "BestScore", FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Subtitle, typeof(Label)), HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center, HeightRequest = 20 };
        Label hdGemsLabel = new Label { Text = "Gems", FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Subtitle, typeof(Label)), HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center, HeightRequest = 20 };
        Label hdAvatarLabel = new Label { Text = "Avatar", FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Subtitle, typeof(Label)), HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center, HeightRequest = 20 };

        //Layout 
        StackLayout MainstackLayout;
        Grid ListViewHeader;
        Grid ListViewGrid;

        // Place Holders and Default Values
        string DefaultPlayerName = "Enter in player name"; // Defualt Value for Entry: player name
        string DefaultUserName = "Enter in User Name"; // Defualt Value for Entry: User Name
        string DefaultPlayerGender = "Player Gender: Female or Male"; // Defualt Value for Entry: Player Gender

        bool resetEntry = false; // Track When Reseting Entry to void clashing with validation
        string ImageName; // keep track of the Images (Avatar Images)

        int countValidInput = 0;


        public ProfilePage()
        {
            InitializeComponent();

            MainstackLayout = new StackLayout();

            ListViewHeader = new Grid();

            ListViewGrid = new Grid();
            Grid.SetRow(ListViewGrid, 1);

            entryPlayerName = new Entry
            {
                Text = DefaultPlayerName
            };
            entryPlayerName.TextChanged += TextChanged;
            entryPlayerName.Focused += OnFocus;
            entryPlayerName.Unfocused += UnFocusEntry;

            entryUserName = new Entry
            {
                Text = DefaultUserName
            };
            entryUserName.TextChanged += TextChanged;
            entryUserName.Focused += OnFocus;
            entryUserName.Unfocused += UnFocusEntry;

            entryPlayerGender = new Entry
            {
                Text = DefaultPlayerGender
            };
            entryPlayerGender.TextChanged += TextChanged;
            entryPlayerGender.Focused += OnFocus;
            entryPlayerGender.Unfocused += UnFocusEntry;

            bestScoreLbL = new Label
            {
                Text = "Score: 0",
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
            };

            gemsLbL = new Label
            {
                Text = "Gems: 0",
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
            };

            saveBTN = new Button
            {
                Text = "Create or Edit Player"
            };
            saveBTN.Clicked += SavePlayerDB;

            deleteBTN = new Button
            {
                Text = "Delete Player"
            };
            deleteBTN.Clicked += DeletePlayerDB;
            deleteBTN.IsEnabled = false;

            selectPlayerBTN = new Button
            {
                Text = "Select Player"
            };
            selectPlayerBTN.IsEnabled = false;
            selectPlayerBTN.Clicked += SelectProfile;

            MainstackLayout.Children.Add(selectPlayerBTN);

            ListViewHeader.Children.Add(hdNameLabel);
            ListViewHeader.Children.Add(hdLastNameLabel, 1, 0);
            ListViewHeader.Children.Add(hdBestScoreLabel, 2, 0);
            ListViewHeader.Children.Add(hdGemsLabel, 3, 0);
            ListViewHeader.Children.Add(hdAvatarLabel, 4, 0);

            MainstackLayout.Children.Add(ListViewHeader);

            CreateListView();

            ListViewGrid.Children.Add(playerListView);
            MainstackLayout.Children.Add(ListViewGrid);

            playerListView.ItemSelected += GetPlayerFromListView;

            playerListView.ItemTapped += DeselectPlayer;

            MainstackLayout.Children.Add(entryPlayerName);

            MainstackLayout.Children.Add(entryUserName);

            MainstackLayout.Children.Add(entryPlayerGender);

            MainstackLayout.Children.Add(bestScoreLbL);

            MainstackLayout.Children.Add(gemsLbL);

            MainstackLayout.Children.Add(saveBTN);

            MainstackLayout.Children.Add(deleteBTN);

            ScrollView scrollView = new ScrollView { Content = MainstackLayout };

            Content = scrollView;
        }

        #region Create the ListView
        public void CreateListView()
        {
            players = App.Database.GetPlayersAsync().Result.Select(itm => itm.Id).ToList();

            playerListView = new ListView
            {
                ItemsSource = App.Database.GetPlayersAsync().Result,
                Margin = new Thickness(0, 20, 0, 0),
                HeightRequest = Application.Current.MainPage.Width * 0.5,
                SelectionMode = (ListViewSelectionMode)SelectionMode.Single
            };
            
            playerListView.ItemTemplate = new DataTemplate(typeof(PlayerCell));
        }
        #endregion

        #region Save Player To DataBase
        async void SavePlayerDB(object sender, EventArgs e)
        {
            if (validation())
            {
                await App.Database.SavePlayerAsync(new PlayerModel
                {
                   Id = SelectedPlayerIndex,
                   UserName = entryUserName.Text,
                   NameOfPlayer = entryPlayerName.Text,
                   AvatarOfPlayer = ImageName
                });
                RefreshThePage();
            }
            else
            {
                await DisplayAlert("Invalid Entry", "Enter in a Username, Player Name and Choose a Gender: Male or Female", "Ok");
            }
        }
        #endregion

        #region Delete Player From DataBase 
        async void DeletePlayerDB(object sender, EventArgs e)
        {
            PlayerModel player = new PlayerModel();
            player = await App.Database.GetPlayerAsync(SelectedPlayerIndex);

            if (player != null)
            {
               deleteBTN.IsEnabled = false;
               await App.Database.DeletePlayerAsync(player);
                RefreshThePage();
            }
        }
        #endregion

        #region Refresh The Page
        public void RefreshThePage()
        {
            Navigation.PushAsync(new ProfilePage());
        }
        #endregion

        #region Get the selected player from the ListView
        async void GetPlayerFromListView(object sender, SelectedItemChangedEventArgs e)
        {
            if (isSelectedPlayer == false)
            {
                //var lvw = (ListView)sender;
                SelectedPlayerIndex = players[e.SelectedItemIndex];
                PlayerModel player = new PlayerModel();
                player = await App.Database.GetPlayerAsync(SelectedPlayerIndex);

                entryPlayerName.Text = player.NameOfPlayer;
                entryUserName.Text = player.UserName;
                bestScoreLbL.FormattedText = "Best Score: " + player.BestScore.ToString();
                gemsLbL.FormattedText = "Gems: " + player.Gems.ToString();
                if (player.AvatarOfPlayer == "Avatar2.jpg")
                {
                    entryPlayerGender.Text = "female";
                }
                else if (player.AvatarOfPlayer == "Avatar1.jpg")
                {
                    entryPlayerGender.Text = "male";
                }

                Console.WriteLine("DB Id: " + SelectedPlayerIndex);

                deleteBTN.IsEnabled = true;
                isSelectedPlayer = true;
                selectPlayerBTN.IsEnabled = true;
            }
        }
        #endregion

        #region Deselect the Player from ListView
        public void DeselectPlayer(object sender, ItemTappedEventArgs e)
        {
            if (isSelectedPlayer == true)
            {
                ((ListView)sender).SelectedItem = null;
                SelectedPlayerIndex = 0;
                RestedALLEntrys();
                isSelectedPlayer = false;
                deleteBTN.IsEnabled = false;
                selectPlayerBTN.IsEnabled = false;
            }
        }
        #endregion

        #region When the Text Change, Start Validating
        int _limit = 10;     //Enter text limit
        public void TextChanged(object sender, TextChangedEventArgs e)
        {
            //Website: Xamarin Forums
            //Title: Max length on Entry
            //URL: https://forums.xamarin.com/discussion/19285/max-length-on-entry
            var _entry = (Entry)sender;

            string _text = _entry.Text; //Get Current Text
            if (resetEntry == false)
            {
                if (_entry.Text.Any(ch => !Char.IsLetterOrDigit(ch)))
                {
                    DisplayAlert("Invalid Input", "No Special Characters", "OK");
                    RemoveCharacter(_text, sender);
                }

                if (_text.Length > _limit) // If it is more than your character restriction
                {
                    DisplayAlert("Max Characters is " + _limit, "You can't have more than " + _limit + " Characters", "OK");
                    RemoveCharacter(_text, sender);
                }
            }
            if (sender == entryPlayerGender)
            {
                if (_text.ToLower() == "female")
                {
                    ImageName = "Avatar2.jpg";
                }
                else if (_text.ToLower() == "male")
                {
                    ImageName = "Avatar1.jpg";
                }
            }
        }
        #endregion

        #region Remove Character (Invalid Characters)
        public void RemoveCharacter(string letter, object sender)
        {
            var _entry = (Entry)sender;
            letter = letter.Remove(letter.Length - 1); // Remove Last character
            _entry.Text = letter;        //Set the Old value
        }
        #endregion

        #region When the Entry is OnFocus (Remove Default Text)
        public void OnFocus(object sender, FocusEventArgs e)
        {
            var _entry = (Entry)sender;
            if (_entry.Text == DefaultPlayerName || _entry.Text == DefaultUserName || _entry.Text == DefaultPlayerGender)
            {
                _entry.Text = "";
            }
        }
        #endregion

        #region When the Entry is UnFocus (Give Entry its Default Text if Entry is Empty)
        public void UnFocusEntry(object sender, EventArgs e)
        {
            var _entry = (Entry)sender;
            if (_entry.Text == "")
            {
                resetEntry = true;
                if (sender == entryPlayerName)
                {
                    _entry.Text = DefaultPlayerName;
                    resetEntry = false;
                }
                else if (sender == entryUserName)
                {
                    _entry.Text = DefaultUserName;
                    resetEntry = false;
                }
                else if (sender == entryPlayerGender)
                {
                    _entry.Text = DefaultPlayerGender;
                    resetEntry = false;
                }
            }
        }
        #endregion

        #region Check Validation Before Saving Player To Database
        public bool validation()
        {
            if (entryPlayerGender.Text.ToLower() == "female" || entryPlayerGender.Text.ToLower() == "male")
            {
                countValidInput += 1;
            }

            if (entryUserName.Text != DefaultPlayerName && entryUserName.Text != "")
            {
                countValidInput += 1;
            }

            if (entryPlayerName.Text != DefaultUserName && entryPlayerName.Text != "")
            {
                countValidInput += 1;
            }


            if (countValidInput == 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Rest ALL Entrys
        public void RestedALLEntrys()
        {
            resetEntry = true;
            entryPlayerName.Text = DefaultPlayerName;
            entryUserName.Text = DefaultUserName;
            entryPlayerGender.Text = DefaultPlayerGender;
            resetEntry = false;
        }
        #endregion

        #region Select Profile to use in New Game of Hangman
        public async void SelectProfile(object sender, EventArgs e)
        {
            int UserID = SelectedPlayerIndex;

            await Navigation.PushAsync(new LevelPage(UserID));
        }
        #endregion
    }
}