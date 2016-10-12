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
        static void Main(string[] args)
        {
            GetMenuChoice();

            string textFilePath = "Data\\Data.txt";

            ObjectListReadWrite(textFilePath);

            Console.WriteLine("\nPress any key to exit.");
            Console.ReadKey();
        }

        static void ObjectListReadWrite(string dataFile)
        {
            List<HighScore> highScoresClassListWrite = new List<HighScore>();

            List<string> highScoresStringListRead = new List<string>(); ;
            List<HighScore> highScoresClassListRead = new List<HighScore>(); ;

            string highScoreString;

            // initialize a list of HighScore objects
            highScoresClassListWrite = InitializeListOfHighScores();

            Console.WriteLine("The following high scores will be added to Data.txt.\n");
            // display list of high scores objects
            DisplayHighScores(highScoresClassListWrite);

            Console.WriteLine("\nAdd high scores to text file. Press any key to continue.\n");
            Console.ReadKey();

            // build the list of strings and write to the text file line by line
            WriteHighScoresToTextFile(highScoresClassListWrite, dataFile);

            Console.WriteLine("High scores added successfully.\n");

            Console.WriteLine("Read into a string of HighScore and display the high scores from data file. Press any key to continue.\n");
            Console.ReadKey();


            // build the list of HighScore class objects from the list of strings
            highScoresClassListRead = ReadHighScoresFromTextFile(dataFile);

            // display list of high scores objects
            DisplayHighScores(highScoresClassListRead);
        }

        static List<HighScore> InitializeListOfHighScores()
        {
            List<HighScore> highScoresClassList = new List<HighScore>();

            // initialize the IList of high scores - note: no instantiation for an interface
            highScoresClassList.Add(new HighScore() { PlayerName = "John", PlayerScore = 1296 });
            highScoresClassList.Add(new HighScore() { PlayerName = "Joan", PlayerScore = 345 });
            highScoresClassList.Add(new HighScore() { PlayerName = "Jeff", PlayerScore = 867 });
            highScoresClassList.Add(new HighScore() { PlayerName = "Charlie", PlayerScore = 2309 });

            return highScoresClassList;
        }

        static void DisplayHighScores(List<HighScore> highScoreClassList)
        {
            foreach (HighScore player in highScoreClassList)
            {
                Console.WriteLine("Player: {0}\tScore: {1}", player.PlayerName, player.PlayerScore);
            }
        }

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

        static List<HighScore> ReadHighScoresFromTextFile(string dataFile)
        {
            const char delineator = ',';

            List<string> highScoresStringList = new List<string>();

            List<HighScore> highScoresClassList = new List<HighScore>();

            // read each line and put it into an array and convert the array to a list
            highScoresStringList = File.ReadAllLines(dataFile).ToList();

            foreach (string highScoreString in highScoresStringList)
            {
                // use the Split method and the delineator on the array to separate each property into an array of properties
                string[] properties = highScoreString.Split(delineator);

                highScoresClassList.Add(new HighScore() { PlayerName = properties[0], PlayerScore = Convert.ToInt32(properties[1]) });
            }

            return highScoresClassList;
        }

        static void DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine("1. Display All Records");
            Console.WriteLine("2. Add a Record");
            Console.WriteLine("3. Delete a Record");
            Console.WriteLine("4. Update a Record");
            Console.WriteLine("5. Clear All Records");
            Console.WriteLine("6. Exit");
        }

        static int GetMenuChoice()
        {
            DisplayMenu();

            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.D0:
                    break;
                case ConsoleKey.D1:
                    break;
                case ConsoleKey.D2:
                    break;
                case ConsoleKey.D3:
                    break;
                case ConsoleKey.D4:
                    break;
                case ConsoleKey.D5:
                    break;
                case ConsoleKey.D6:
                    break;
                default:
                    break;
            }
        }
    }


}
