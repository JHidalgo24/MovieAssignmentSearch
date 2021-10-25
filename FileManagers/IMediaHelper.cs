using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieAssignmentInterfaces.MediaObjects;

namespace MovieAssignmentInterfaces.FileManagers
{
    public interface IMediaHelper
    {
        public abstract void ReadFiles();
        public abstract void ShowAdd();
        public abstract void MovieAdd();
        public abstract void VideoAdd();
        public abstract void SearchMedia(string title);
        public abstract bool DuplicateChecker(string chosenMedia, string type);
    }

}