using System.Collections.Generic;
using System.IO;
using System.Linq;
using MovieAssignmentInterfaces.MediaObjects;
using Newtonsoft.Json;
using System;

namespace MovieAssignmentInterfaces.FileManagers
{
    public class JsonFileHelper : IMediaHelper
    {
        private const string MoviePath = "Files//movies.json";
        private const string ShowPath = "Files//shows.json";
        private const string VideoPath = "Files//videos.json";
        private List<Shows> ShowsList = new List<Shows>();
        private List<Movie> MovieList = new List<Movie>();
        private List<Video> VideoList = new List<Video>();

        //returns movie list without giving them access to actual list
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
    
        //constructor to read in lists
        public JsonFileHelper()
        {
            //reads the files into their list as soon as JSONFileHelper is made 
            Shows();
            Movies();
            Videos();
        }
        
        //these read the File mostly used in constructor)
        public void Shows()
        {
            try
            {
                string json = File.ReadAllText(ShowPath);
                ShowsList = JsonConvert.DeserializeObject<List<Shows>>(json);
            }
            catch (Exception)
            {
                Console.WriteLine("Could not read JSON file to List");
            }
        }
        public void Movies()
        {
            try
            {
                string json = File.ReadAllText(MoviePath);
                MovieList = JsonConvert.DeserializeObject<List<Movie>>(json);
            }
            catch (Exception)
            {
                Console.WriteLine("Could not read JSON file to List");
            }
        }
        public void Videos()
        {
            try
            {
                string json = File.ReadAllText(VideoPath);
                VideoList = JsonConvert.DeserializeObject<List<Video>>(json);
            }
            catch (Exception)
            {
                Console.WriteLine("Could not read JSON file to List");
            }
        }
        
        //these add object to it's file
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
                Console.WriteLine($"What is the name of writer #{i+1}");
                writers.Add(Console.ReadLine());
            }

            (temp as Shows).writers = writers;
            ShowsList.Add((temp as Shows));
            string json = JsonConvert.SerializeObject(ShowsList, Formatting.Indented);
            File.WriteAllText(ShowPath, json);
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
            string json = JsonConvert.SerializeObject(MovieList, Formatting.Indented);
            File.WriteAllText(MoviePath, json);
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
            string json = JsonConvert.SerializeObject(VideoList, Formatting.Indented);
            File.WriteAllText(VideoPath, json);
        }
        
        //searches media
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
        
        //duplicate checks from lists
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