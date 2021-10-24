using System.Collections.Generic;
using CsvHelper.Configuration.Attributes;
using MovieAssignmentInterfaces.Converters;

namespace MovieAssignmentInterfaces.MediaObjects
{
    public class Movie : Media
    {
        [Name("genres")]
        [TypeConverter(typeof(ToStringArrayConverter))]
        public List<string> Genres { get; set; }

        public override string Display()
        {
            return $"Type:Movie MovieId:{Id} Title:{title} Genres:{string.Join(',', Genres)}";


        }

    }
}