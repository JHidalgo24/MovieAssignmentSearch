using System;
using System.Collections.Generic;
using System.Linq;
using MovieAssignmentInterfaces.FileManagers;
using MovieAssignmentInterfaces.MediaObjects;
using NLog;

namespace MovieAssignmentInterfaces
{
    internal class Program
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();

        private static JsonFileHelper FileHelper = new(); //Can change JSONFileHelper to CSVFileHelper and should still work  

        private static readonly Menu Menu = new(); //helps with menu options and getting ints

        private static void Main(string[] args)
        {
            //ask the user what they want to do
            var option = 0;
            while (option != 4)
            {
                Menu.Display();
                option = Menu.ValueGetter();
                switch (option)
                {
                    case 1: //Add
                        logger.Debug("User chose to add to files");
                        Console.WriteLine("What do you want to add to?\n1)Movie\n2)Show\n3)Video");
                        var mediaChoice = Menu.ValueGetter();
                        switch (mediaChoice)
                        {
                            case 1:
                                FileHelper.MovieAdd();
                                break;
                            case 2:
                                FileHelper.ShowAdd();
                                break;
                            case 3:
                                FileHelper.VideoAdd();
                                break;
                            default:
                                Console.WriteLine("Sorry not a choice");
                                break;
                        }
                        break;
                    case 2: //Search
                        logger.Debug("User chose to search files");
                        Console.WriteLine("What do you want to search for?");
                        FileHelper.SearchMedia(Console.ReadLine());
                        break;
                    case 3: //Display
                        logger.Debug("User chose to display from files");
                        Console.WriteLine("What do you want to display from?\n1)Movie\n2)Show\n3)Video");
                        var searchChoice = Menu.ValueGetter();
                        switch (searchChoice)
                        {
                            case 1:
                            {
                                var cont = ""; //variable to let user decide whether they want to exit or not
                                var temp = FileHelper.MovieList;
                                while (cont.ToLower() != "exit")
                                {
                                    for (var i = 0; i < 10; i++) Console.WriteLine(temp[i].Display());
                                    temp = temp.Skip(10).ToList();
                                    Console.WriteLine("Show +10 more? enter exit to leave");
                                    cont = Console.ReadLine();
                                }
                            }
                                break;
                            case 2:
                            {
                                var cont = "";
                                var temp = FileHelper.ShowsList;
                                while (cont.ToLower() != "exit")
                                {
                                    for (var i = 0; i < 1; i++) Console.WriteLine(temp[i].Display());
                                    temp = temp.Skip(1).ToList();
                                    Console.WriteLine("Show +1 more? enter exit to leave");
                                    cont = Console.ReadLine();
                                    cont = temp.Count == 0
                                        ? "exit"
                                        : cont; //exits them if there is no more shows so they don't keep
                                    //getting blank spaces in console
                                }
                            }
                                break;

                            case 3:
                            {
                                var cont = "";
                                var temp = FileHelper.VideoList;
                                while (cont.ToLower() != "exit")
                                {
                                    for (var i = 0; i < 1; i++) Console.WriteLine(temp[i].Display());
                                    temp = temp.Skip(1).ToList();
                                    Console.WriteLine("Show +1 more? enter exit to leave");
                                    cont = Console.ReadLine();
                                    cont = temp.Count == 0 ? "exit" : cont;
                                }
                            }
                                break;
                            default:
                                Console.WriteLine("Sorry not a choice");
                                break;
                        }
                        break;
                    case 4: //Exit
                        logger.Debug("User exited the program");
                        Console.WriteLine("Goodbye!");
                        break;
                    default: //wrong input
                        logger.Debug($"User made invalid choice of (1-4) Chose: {option}");
                        Console.WriteLine("Sorry not a choice");
                        break;
                }
            }
        }
    }
}