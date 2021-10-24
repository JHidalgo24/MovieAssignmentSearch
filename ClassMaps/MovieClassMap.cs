using CsvHelper.Configuration;
using MovieAssignmentInterfaces.MediaObjects;

namespace MovieAssignmentInterfaces.ClassMaps
{
    public class MovieClassMap : ClassMap<Movie>
    {

        public MovieClassMap()
        {
            Map(m => m.Id).Index(0).Name("movieId");
            Map(m => m.title).Index(1).Name("title");
            Map(m => m.Genres).Index(2).Name("genres");
        }
    }
}