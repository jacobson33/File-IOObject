using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_PersistenceFileStream
{
    class Program
    {
        //view for program
        static ConsoleMenu view = new ConsoleMenu(120, 40);

        /// <summary>
        /// Main function
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            string textFilePath = "Data\\Data.txt";

            // initialize high scores and write to text file
            InitializeHighScores(textFilePath);
            List<HighScore> scores;

            while (true)
            {
                //update list
                scores = ReadHighScoresFromTextFile(textFilePath);

                //display menu
                DisplayMenu(textFilePath, scores);
            }
        }

        /// <summary>
        /// Initialize the date file
        /// </summary>
        /// <param name="textFilePath"></param>
        static void InitializeHighScores(string textFilePath)
        {
            List<HighScore> highScoresClassList = new List<HighScore>();

            // initialize the IList of high scores - note: no instantiation for an interface
            highScoresClassList.Add(new HighScore() { PlayerName = "John", PlayerScore = 1296 });
            highScoresClassList.Add(new HighScore() { PlayerName = "Joan", PlayerScore = 345 });
            highScoresClassList.Add(new HighScore() { PlayerName = "Jeff", PlayerScore = 867 });
            highScoresClassList.Add(new HighScore() { PlayerName = "Charlie", PlayerScore = 2309 });

            WriteHighScoresToTextFile(highScoresClassList, textFilePath);
        }

        /// <summary>
        /// Writes the Data to the file
        /// </summary>
        /// <param name="highScoreClassLIst"></param>
        /// <param name="dataFile"></param>
        static void WriteHighScoresToTextFile(List<HighScore> highScoreClassLIst, string dataFile)
        {
            string highScoreString;

            List<string> highScoresStringListWrite = new List<string>();

            // build the list to write to the text file line by line
            foreach (var player in highScoreClassLIst)
            {
                highScoreString = player.PlayerName + "," + player.PlayerScore;
                highScoresStringListWrite.Add(highScoreString);
            }

            File.WriteAllLines(dataFile, highScoresStringListWrite);
        }

        /// <summary>
        /// Reads the data from the file
        /// </summary>
        /// <param name="dataFile"></param>
        /// <returns></returns>
        static List<HighScore> ReadHighScoresFromTextFile(string dataFile)
        {
            const char delineator = ',';

            List<string> highScoresStringList = new List<string>();

            List<HighScore> highScoresClassList = new List<HighScore>();

            // read each line and put it into an array and convert the array to a list
            try
            {
                highScoresStringList = File.ReadAllLines(dataFile).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            foreach (string highScoreString in highScoresStringList)
            {
                // use the Split method and the delineator on the array to separate each property into an array of properties
                string[] properties = highScoreString.Split(delineator);

                highScoresClassList.Add(new HighScore() { PlayerName = properties[0], PlayerScore = Convert.ToInt32(properties[1]) });
            }

            return highScoresClassList;
        }

        /// <summary>
        /// Displays the main menu
        /// </summary>
        /// <param name="path"></param>
        static void DisplayMenu(string path, List<HighScore> scores)
        {
            //display menu
            view.DrawMenu(28, 15, new List<string>() { "1. Display All Records", "2. Add a Record", "3. Delete a Record", "4. Update a Record", "5. Clear all Records", "6. Exit" });

            //get user choice
            switch (view.PromptKey())
            {
                case ConsoleKey.D1:
                    DisplayAllRecords(path, scores);
                    break;
                case ConsoleKey.D2:
                    AddRecord(path, scores);
                    break;
                case ConsoleKey.D3:
                    DeleteRecord(path, scores);
                    break;
                case ConsoleKey.D4:
                    UpdateRecord(path, scores);
                    break;
                case ConsoleKey.D5:
                    DeleteAllRecords(path);
                    break;
                case ConsoleKey.D6:
                    Environment.Exit(1);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Displays the data
        /// </summary>
        /// <param name="path"></param>
        static void DisplayAllRecords(string path, List<HighScore> scores)
        {
            Console.Clear();

            if (scores.Count >= 18) Console.BufferHeight *= 2;

            //-------------------
            int gridRowNum = scores.Count;
            int gridColNum = 2;
            int gridCellWidth = 20;
            int gridCellHeight = 1;
            int gridX = 120 / 2 - ((gridColNum * gridCellWidth) + gridColNum + 1) / 2;
            int gridY = 3;

            view.DrawGrid(gridX, gridY, gridRowNum, gridColNum, gridCellWidth, gridCellHeight);

            //-------------------
            int i = 1;
            foreach (HighScore player in scores)
            {
                view.WriteAt(gridX + 1, gridY + 1 * i, player.PlayerName);
                view.WriteAt(gridX + gridCellWidth + 2, gridY + 1 * i, $"{player.PlayerScore}");
                i += 2;
            }

            Console.ReadKey(true);
            Console.BufferHeight = 40;
        }

        /// <summary>
        /// Adds record the list
        /// </summary>
        /// <param name="path"></param>
        static void AddRecord(string path, List<HighScore> scores)
        {
            int addScore;

            //prompt user to add a player name
            view.DrawPromptBox("Enter Player's Name: ");
            string addPlayer = Console.ReadLine();

            //do update instead of add for existing player
            foreach (HighScore score in scores)
            {
                if (score.PlayerName.ToLower() == addPlayer.ToLower())
                {
                    UpdateRecord(path, scores, true, score.PlayerName);
                    return;
                }
            }

            //prompt user to add a score
            view.DrawPromptBox("Enter Player's Score: ");
            string response = Console.ReadLine();
            if (int.TryParse(response, out addScore)) // Try to parse the string as an integer
            {
                scores.Add(new HighScore(addPlayer, addScore));
            }
            else
            {
                Console.WriteLine("Not an integer!");
            }

            //write to file
            WriteHighScoresToTextFile(scores, path);

        }

        /// <summary>
        /// Deletes data from the file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="view"></param>
        static void DeleteRecord(string path, List<HighScore> scores)
        {

            //prompt user to delete a score
            view.DrawPromptBox("Who's score to delete?");
            string response = Console.ReadLine();

            //remove player score
            scores.RemoveAll(score => score.PlayerName == response);

            //update file
            WriteHighScoresToTextFile(scores, path);
        }

        /// <summary>
        /// Updates a player
        /// </summary>
        /// <param name="path">file path</param>
        /// <param name="view">view</param>
        /// <param name="scores">list of highscores</param>
        /// <param name="alreadyFound">is this coming from add highscore?</param>
        /// <param name="name">name of player already found</param>
        static void UpdateRecord(string path, List<HighScore> scores, bool alreadyFound = false, string name = "")
        {
            int newScore;
            bool playerFound = false;
            string response;

            //handle if player was found in AddHighScore
            if (alreadyFound)
            {
                playerFound = true;
                response = name;
            }
            else
            {
                //prompt user to update a score
                view.DrawPromptBox("Who's score to update?");
                response = Console.ReadLine();

                foreach (HighScore score in scores)
                {
                    if (score.PlayerName == response)
                        playerFound = true;
                }
            }

            //player name found
            if (playerFound)
            {
                string score;

                view.DrawPromptBox("Enter new score:");
                score = Console.ReadLine();

                while (!Int32.TryParse(score, out newScore))
                {
                    view.DrawPromptBox("Enter a valid score:");
                    score = Console.ReadLine();
                }

            }
            else
            {
                Console.Clear();
                Console.CursorVisible = false;
                view.DrawTextBox("No player found with that name. Press any key to continue.");
                Console.ReadKey(true);
                return;
            }

            //update score
            foreach (HighScore score in scores)
            {
                if (score.PlayerName == response)
                {
                    score.PlayerScore = newScore;
                }
            }

            //update file
            WriteHighScoresToTextFile(scores, path);
        }

        /// <summary>
        /// Delete all records in file
        /// </summary>
        static void DeleteAllRecords(string path)
        {
            view.DrawPromptBox("Delete all records? (yes / no): ");
            string response = Console.ReadLine().ToUpper();

            if (response == "YES")
                WriteHighScoresToTextFile(new List<HighScore>(), path);
        }
    }
}
