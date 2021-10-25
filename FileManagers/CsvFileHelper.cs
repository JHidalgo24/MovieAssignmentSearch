using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using MovieAssignmentInterfaces.ClassMaps;
using MovieAssignmentInterfaces.Converters;
using MovieAssignmentInterfaces.MediaObjects;

namespace MovieAssignmentInterfaces.FileManagers
{
    public class CsvFileHelper : IMediaHelper
    {
        //if for some reason path changes I can directly change here rather than look for it everywhere
        private const string MoviePath = "Files//movies.csv";
        private const string ShowPath = "Files//shows.csv";
        private const string VideoPath = "Files//videos.csv";
        
        //holds the media objects
        public List<Shows> ShowsList = new List<Shows>();
        public List<Movie> MovieList = new List<Movie>();
        public List<Video> VideoList = new List<Video>();
        
        //constructor to read in lists
        public CsvFileHelper() 
        {
            ReadFiles();
        }
        
        //reads all the objects into lists but didn't want to add code directly in constructor
        //to keep constructor clean
        public void ReadFiles()
        {
            try
            {
                //reads shows to list from csv
                using (var streamReader =
                    new StreamReader(ShowPath))
                using (var csv = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<Shows>().ToList();
                    ShowsList = records;

                }

                //reads videos to list from csv
                using (var streamReader =
                    new StreamReader(VideoPath))
                using (var csv = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<Video>().ToList();
                    VideoList = records;

                }

                //reads movies to list from csv
                using (var streamReader =
                    new StreamReader(MoviePath))
                using (var csv = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<Movie>().ToList();
                    MovieList = records;
                }
            }
            catch (Exception )
            {
                Console.WriteLine("Could not read files");
                Program.logger.Debug("Could not read files");
            }
        }
        
        //these add object to it's file and updates its list
        public void ShowAdd()
        {
            Menu menu = new Menu();
            Shows temp = new Shows();
            temp.Id = ShowsList[^1].Id+1;
            Console.WriteLine("What is the title of the Show?");
            string tempTitle = Console.ReadLine();
            while (DuplicateChecker(tempTitle,"Show"))
            {
                Console.WriteLine("That show already exists choose a new title");
                tempTitle = Console.ReadLine();
            }
            temp.title = tempTitle;
            Console.WriteLine("How many seasons did the show have?");
            temp.season = menu.ValueGetter();
            Console.WriteLine("How many Episodes are there?");
            temp.episode = menu.ValueGetter();
            Console.WriteLine("How many writers are there?");
            int writerCount = menu.ValueGetter();
            var writersList = new List<string>();
            for (int i = 0; i < writerCount; i++)
            {
                Console.WriteLine($"What is the name of writer #{i + 1}");
                writersList.Add(Console.ReadLine());
            }
            temp.Writers = writersList;
            ShowsList.Add(temp);
            var records = new List<Shows> { new Shows { episode = temp.episode, Id = temp.Id, season = temp.season, title = temp.title, Writers = temp.Writers } };
            var config = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false, };
            using (var stream = File.Open(ShowPath,
                FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.Context.RegisterClassMap<ShowClassMap>();
                csv.WriteRecords(records);
            }

        }
        public void VideoAdd()
        {
            Media temp = new Video();
            Menu menu = new Menu();
            temp.Id = VideoList[^1].Id+1;
            Console.WriteLine("What is the title of the video?");
            string videoTitle = Console.ReadLine();
            while (DuplicateChecker(videoTitle,"Video"))
            {
                Console.WriteLine("Sorry that video already exists enter a new title");
                videoTitle = Console.ReadLine();
            }
            temp.title = videoTitle;
            Console.WriteLine("How many formats is the video in DVD, VHS, Etc.? [Enter as number] ");
            List<string> format = new List<string>();
            int formatAmount = menu.ValueGetter();
            for (int i = 0; i < formatAmount; i++)
            {
                Console.WriteLine($"What is the #{i+1} format?");
                format.Add(Console.ReadLine());

            }

            (temp as Video).Format = format;
            Console.WriteLine("How long is the video in minutes?");
            (temp as Video).Length = menu.ValueGetter();
            Console.WriteLine("How many regions is the video in?");
            Console.WriteLine("0-NA \n1-SA \n2-Asia");
            int regionsTotal = menu.ValueGetter();
            List<int> regions = new List<int>();
            for (int i = 0; i < regionsTotal; i++)
            {
                Console.WriteLine($"Enter the #{i+1} Region");
                int region = menu.ValueGetter();
                while (region is > 3 or < 0)
                {
                    region = menu.ValueGetter();
                }
                regions.Add(region);
            }
            (temp as Video).Regions = regions;
            VideoList.Add((temp as Video));
            var records = new List<Video> { new Video { Id = temp.Id, title = temp.title, Format = (temp as Video).Format, Length = (temp as Video).Length, Regions = (temp as Video).Regions } };
            var config = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false, };
            using (var stream = File.Open(VideoPath,
                FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.Context.RegisterClassMap<VideosClassMap>();
                csv.WriteRecords(records);
            }
        }
        public void MovieAdd()
        {
            Menu menu = new Menu();
            Media temp = new Movie();

            (temp as Movie).Id = MovieList[^1].Id + 1;
            Console.WriteLine("What is the title of the Movie?");
            string movieTitle = Console.ReadLine();
            Console.WriteLine("What year was the movie made in?");
            movieTitle = movieTitle + " (" + menu.ValueGetter() + ")";
            while (DuplicateChecker(movieTitle,"Movie"))
            {
                Console.WriteLine("Sorry that movie already exists! Enter a new one");
                movieTitle = Console.ReadLine();
                Console.WriteLine("What year was the movie made in?");
                movieTitle = movieTitle + " (" + menu.ValueGetter() + ")";
            }
            temp.title = movieTitle;
            Console.WriteLine("How many genres are there?");
            List<string> genres = new List<string>();
            int genresTotal = menu.ValueGetter();
            for (int i = 0; i < genresTotal; i++)
            {
                Console.WriteLine("What is the first genre?");
                genres.Add(Console.ReadLine());
            }

            (temp as Movie).Genres = genres;
            MovieList.Add((temp as Movie));
            var records = new List<Movie> { new Movie { Id = temp.Id, title = temp.title, Genres = (temp as Movie).Genres } };
            var config = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false, };
            using (var stream = File.Open(MoviePath,
                FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.Context.RegisterClassMap<MovieClassMap>();//tells the CSVwriter the order to use
                csv.WriteRecords(records);
            }
        }
        
        //searches media(from all of the lists)
        public void SearchMedia(string title)
        {
           List<Media> foundMedia = new List<Media>();
            /* where item in list title to lower case contains user input title to lower case then for each media
            matched add to the foundMedia list. I could make it output then and there but would make it
            messy with 3 Console.WriteLines on 3 separate lines*/
            MovieList.Where(c => c.title.ToLower().Contains(title.ToLower())).ToList().ForEach(c => foundMedia.Add(c));
            ShowsList.Where(c => c.title.ToLower().Contains(title.ToLower())).ToList().ForEach(c => foundMedia.Add(c));
            VideoList.Where(c => c.title.ToLower().Contains(title.ToLower())).ToList().ForEach(c => foundMedia.Add(c));

            System.Console.WriteLine($"There are {foundMedia.Count} matched searches! for '{title}'");
            foreach (var x in foundMedia)
            {
                Console.WriteLine(x.Display());
            } 
        }
        
        //duplicate checks from lists (finds exact match and not just a substring)
        public bool DuplicateChecker(string chosenMedia, string type)
        {
            var contained = false;
            switch (type)
            {
                case "Movie":
                    contained = MovieList.Any(c => c.title == chosenMedia);

                    break;
                case "Show":
                    contained = ShowsList.Any(c => c.title == chosenMedia);

                    break;
                case "Video":
                    contained = VideoList.Any(c => c.title == chosenMedia);
                    break;
            }

            return contained;

        }
    }
}