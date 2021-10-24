using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieAssignmentInterfaces.MediaObjects;

namespace MovieAssignmentInterfaces.FileManagers
{
    public interface IMediaHelper
    {
        public List<Movie> ReturnMovieList();
        public List<Shows> ReturnShowList();
        public List<Video> ReturnVideoList();
        public abstract void Shows();//reads shows to list
        public abstract void Movies(); //reads movies to list
        public abstract void Videos();//reads videos to list
        public abstract void ShowAdd();
        public abstract void MovieAdd();
        public abstract void VideoAdd();
        public abstract void SearchMedia(string title);
        public abstract bool DuplicateChecker(string chosenMedia, string type);
    }

}