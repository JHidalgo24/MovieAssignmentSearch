using System.Collections.Generic;
using CsvHelper.Configuration.Attributes;
using MovieAssignmentInterfaces.Converters;

namespace MovieAssignmentInterfaces.MediaObjects
{
    public class Shows : Media
    {
        [Name("season")] public int season { get; set; }
        [Name("episode")] public int episode { get; set; }

        [Name("writers")]
        [TypeConverter(typeof(ToStringArrayConverter))]
        public List<string> writers { get; set; }

        public override string Display()
        {
            return $"Type:Show ShowId:{Id} Title:{title} Seasons:{season} Episodes:{episode} Writers:{string.Join('|', writers)} ";
        }
    }
}