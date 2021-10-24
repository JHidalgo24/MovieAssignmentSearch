using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using MovieAssignmentInterfaces.ClassMaps;
using MovieAssignmentInterfaces.MediaObjects;

namespace MovieAssignmentInterfaces.FileManagers
{
    public class CsvFileHelper : IMediaHelper
    {

        private const string MoviePath = "Files//movies.csv";
        private const string ShowPath = "Files//shows.csv";
        private const string VideoPath = "Files//videos.csv";
        public List<Shows> ShowsList { get; set; }
        public List<Movie> MovieList { get; set; }
        public List<Video> VideoList { get; set; }
        public List<Movie> ReturnMovieList()
        {
            return MovieList;

        }
        public List<Shows> ReturnShowList()
        {
            return ShowsList;
        }
        public List<Video> ReturnVideoList()
        {
            return VideoList;
        }
        public CsvFileHelper()//reads the files into their list as soon as CsvFileHelper is made 
        {
            Shows();
            Movies();
            Videos();
        }
        public void Shows()
        {
            try
            {

                using (var streamReader =
                    new StreamReader(ShowPath))
                using (var csv = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<Shows>().ToList();
                    ShowsList = records;

                }
            }
            catch (Exception)
            {
                Console.WriteLine("Couldn't read file");
                throw;
            }
        }
        public void Videos()
        {

            try
            {

                using (var streamReader =
                    new StreamReader(VideoPath))
                using (var csv = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<Video>().ToList();
                    VideoList = records;

                }
            }
            catch (Exception)
            {
                Console.WriteLine("Couldn't read file");
                throw;
            }
        }
        public void Movies()
        {

            try
            {

                using (var streamReader =
                    new StreamReader(MoviePath))
                using (var csv = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<Movie>().ToList();
                    MovieList = records;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Couldn't read file");
                throw;
            }
        }
        public void ShowAdd()
        {
            Menu menu = new Menu();
            Media temp = new Shows();
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
            (temp as Shows).season = menu.ValueGetter();
            Console.WriteLine("How many Episodes are there?");
            (temp as Shows).episode = menu.ValueGetter();
            Console.WriteLine("How many writers are there?");
            int writerCount = menu.ValueGetter();
            List<string> writers = new List<string>();
            for (int i = 0; i < writerCount; i++)
            {
                Console.WriteLine($"What is the name of writer #{i + 1}");
                writers.Add(Console.ReadLine());
            }
            ShowsList.Add((temp as Shows));
            var records = new List<Shows> { new Shows { episode = (temp as Shows).episode, Id = temp.Id, season = (temp as Shows).season, title = temp.title, writers = (temp as Shows).writers } };
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
            Console.WriteLine("0-NA \n 1-SA \n3-Asia");
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
        public void SearchMedia(string title)
        {
           List<Media> foundMedia = new List<Media>();
            
            MovieList.Where(c => c.title.ToLower().Contains(title.ToLower())).ToList().ForEach(c => foundMedia.Add(c));
            ShowsList.Where(c => c.title.ToLower().Contains(title.ToLower())).ToList().ForEach(c => foundMedia.Add(c));
            VideoList.Where(c => c.title.ToLower().Contains(title.ToLower())).ToList().ForEach(c => foundMedia.Add(c));

            System.Console.WriteLine($"There are {foundMedia.Count} matched searches! for '{title}'");
            foreach (var x in foundMedia)
            {
                Console.WriteLine(x.Display());
            } 
        }
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