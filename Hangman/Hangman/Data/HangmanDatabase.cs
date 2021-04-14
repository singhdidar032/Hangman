using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using SQLiteNetExtensionsAsync.Extensions;

namespace Hangman.Data
{
    public class HangmanDatabase
    {
        // DB Connection
        readonly SQLiteAsyncConnection _database;

        // Create Tables 
        public HangmanDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<PlayerModel>().Wait();
            _database.CreateTableAsync<HangManModel>().Wait();
            _database.CreateTableAsync<WordsModel>().Wait();
        }

        // get all players in DB
        public Task<List<PlayerModel>> GetPlayersAsync()
        {
            return _database.Table<PlayerModel>().ToListAsync();
        }

        // get one player in the DB
        public Task<PlayerModel> GetPlayerAsync(int id)
        {
            return _database.Table<PlayerModel>()
                            .Where(i => i.Id == id)
                            .FirstOrDefaultAsync();
        }

        // save player in to the DB
        public Task<int> SavePlayerAsync(PlayerModel player)
        {
            if (player.Id != 0)
            {
                return _database.UpdateAsync(player);
            }
            else
            {
                return _database.InsertAsync(player);
            }
        }
        // Delete player from DB
        public Task<int> DeletePlayerAsync(PlayerModel player)
        {
            return _database.DeleteAsync(player);
            //return _database.DeleteAllAsync<PlayerModel>();
        }
        // get all the hangman game from DB
        public Task<List<HangManModel>> GetHangmansAsync()
        {
            return _database.Table<HangManModel>().ToListAsync();
        }
        // get one hangman game from DB
        public Task<HangManModel> GetHangmanAsync(int id)
        {
            return _database.Table<HangManModel>()
                            .Where(i => i.Id == id)
                            .FirstOrDefaultAsync();
        }

        // save hangman game to the DB
        public Task<int> SaveHangmanAsync(HangManModel hangman)
        {
            if (hangman.Id != 0)
            {
                _database.UpdateAsync(hangman).Wait();
                if (hangman.childPlayerModel != null)
                {
                    _database.UpdateAsync(hangman);
                    return (Task<int>)_database.UpdateWithChildrenAsync(hangman);
                }
                else
                {
                    return _database.UpdateAsync(hangman);
                }

            }
            else
            {
                if (hangman.childPlayerModel != null)
                {
                    _database.InsertAsync(hangman);
                    return (Task<int>)_database.UpdateWithChildrenAsync(hangman);
                }
                else
                {
                    return _database.InsertAsync(hangman);
                }
            }
        }

        //Delete Hangman game from DB
        public Task<int> DeleteHangmanAsync(HangManModel hangman)
        {
            return _database.DeleteAsync(hangman);
        }

        // Get all Words in DB
        public Task<List<WordsModel>> GetWordsAsync()
        {
            return _database.Table<WordsModel>().ToListAsync();
        }

        //Get one word in DB
        public Task<WordsModel> GetWordAsync(int id)
        {
            return _database.Table<WordsModel>()
                            .Where(i => i.Id == id)
                            .FirstOrDefaultAsync();
        }

        //Save Word in to DB
        public Task<int> SaveWordAsync(WordsModel word)
        {

            if (word.Id != 0)
            {
                return _database.UpdateAsync(word);
            }
            else
            {
                return _database.InsertAsync(word);
            }

        }

        // Delete word from the DB 
        public Task<int> DeleteWordAsync(WordsModel word)
        {
            return _database.DeleteAsync(word);
            //return _database.DeleteAllAsync<WordsModel>();
        }

        //Get Random Word from DB

        //Website: Microsoft Documentation
        //Title: CA1828: Do not use CountAsync/LongCountAsync when AnyAsync can be used
        // URL: https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1828

        //Website: Microsoft Documentation
        //Title: Async return types (C#)
        // URL: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/async-return-types
        public async Task<string> CheckRandomID(int id)
        => await _database.Table<WordsModel>().Where(i => i.Id == id).CountAsync() != 0 ? "Not empty" : "Empty";

        public async Task<string> CheckDuplicateWords(string Word)
       => await _database.Table<WordsModel>().Where(i => i.Word == Word).CountAsync() != 0 ? "Not empty" : "Empty";



        /*public Task<List<WordsModel>> GetRandomWordDBAsync()
          {
              // Website name: MySQLTUTORIAL
              // Title: MySQL Select Random Records
              // URL: https://www.mysqltutorial.org/select-random-records-database-table.aspx
              return _database.QueryAsync<WordsModel>("SELECT Word FROM WordsModel ORDER BY random() LIMIT 1");
          }*/

        /*
        public Task<List<WordsModel>> GetRandomWordDBAsync()
        {
            // Website name: MySQLTUTORIAL
            // Title: MySQL Select Random Records
            // URL: https://www.mysqltutorial.org/select-random-records-database-table.aspx
            //Random rnd = new Random();
            //rnd.Next();
            return _database.QueryAsync<WordsModel>("SELECT `Word` FROM `WordsModel` ORDER BY random() LIMIT 1");
        }
        */

        /*
        public Task<List<WordsModel>> GetRandomWordDBAsync()
        {
            // Website name: MySQLTUTORIAL
            // Title: MySQL Select Random Records
            // URL: https://www.mysqltutorial.org/select-random-records-database-table.aspx
            //https://www.sqlitetutorial.net/sqlite-functions/sqlite-random/
            return _database.QueryAsync<WordsModel>("SELECT Word FROM WordsModel ORDER BY random() LIMIT 1");
        }
        */
    }
}
