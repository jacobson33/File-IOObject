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
        /// <summary>
        /// Main function
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string textFilePath = "Data\\Data.txt";

            // initialize high scores and write to text file
            InitializeHighScores(textFilePath);

            while (true)
            {
                DisplayMenu(textFilePath);
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
        /// Display the Data
        /// </summary>
        /// <param name="highScoreClassList"></param>
        static void DisplayHighScores(List<HighScore> highScoreClassList)
        {
            foreach (HighScore player in highScoreClassList)
            {
                Console.WriteLine("Player: {0}\tScore: {1}", player.PlayerName, player.PlayerScore);
            }

            Console.WriteLine("Press any key to continue.");
            Console.ReadKey(true);
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
        static void DisplayMenu(string path)
        {
            ConsoleMenu view = new ConsoleMenu(120, 40);

            //display menu
            view.DrawMenu(28, 15, new List<string>() { "1. Display All Records", "2. Add a Record", "3. Delete a Record", "4. Update a Record", "5. Clear all Records", "6. Exit" });

            //get user choice
            switch (view.PromptKey())
            {
                case ConsoleKey.D1:
                    DisplayAllRecords(path);
                    break;
                case ConsoleKey.D2:
                    AddRecord(path, view);
                    break;
                case ConsoleKey.D3:
                    DeleteRecord(path, view);
                    break;
                case ConsoleKey.D4:
                    UpdateRecord(path, view);
                    break;
                case ConsoleKey.D5:
                    WriteHighScoresToTextFile(new List<HighScore>(), path);
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
        static void DisplayAllRecords(string path)
        {
            //open file
            List<HighScore> scores = ReadHighScoresFromTextFile(path);

            Console.Clear();

            DisplayHighScores(scores);
        }

        /// <summary>
        /// Adds record the list
        /// </summary>
        /// <param name="path"></param>
        static void AddRecord(string path, ConsoleMenu view)
        {
            int addScore;
            bool playerFound = false;

            //open file
            List<HighScore> scores = ReadHighScoresFromTextFile(path);

            //prompt user to add a player name
            view.DrawPromptBox("Enter Player's Name: ");
            string addPlayer = Console.ReadLine();

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

        }

        /// <summary>
        /// Deletes data from the file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="view"></param>
        static void DeleteRecord(string path, ConsoleMenu view)
        {
            //read file
            List<HighScore> scores = ReadHighScoresFromTextFile(path);

            //prompt user to delete a score
            view.DrawPromptBox("Who's score to delete?");
            string response = Console.ReadLine();

            //remove player score
            scores.RemoveAll(score => score.PlayerName == response);

            //update file
            WriteHighScoresToTextFile(scores, path);
        }

        /// <summary>
        /// Updates data from the file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="view"></param>
        static void UpdateRecord(string path, ConsoleMenu view)
        {
            int newScore;
            bool playerFound = false;

            //read file
            List<HighScore> scores = ReadHighScoresFromTextFile(path);

            //prompt user to delete a score
            view.DrawPromptBox("Who's score to update?");
            string response = Console.ReadLine();

            foreach (HighScore score in scores)
            {
                if (score.PlayerName == response)
                    playerFound = true;
            }

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
    }
}
