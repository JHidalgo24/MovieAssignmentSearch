using System;
using System.Collections.Generic;
using CsvHelper.Configuration.Attributes;
using MovieAssignmentInterfaces.Converters;

namespace MovieAssignmentInterfaces.MediaObjects
{
    public class Video : Media
    {

        [Name("format")]
        [TypeConverter(typeof(ToStringArrayConverter))]
        public List<string> Format { get; set; }

        [Name("length")]
        public int Length { get; set; }

        [Name("regions")]
        [TypeConverter(typeof(ToIntArrayConverter))]
        public List<int> Regions { get; set; }


        public override string Display()
        {
            return $"Type:Video VideoId:{Id} Title:{title} Format:{String.Join('|', Format)} Length:{Length} Region(s):{String.Join(',', Regions)}";
        }
    }
}