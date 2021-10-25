using CsvHelper.Configuration;
using MovieAssignmentInterfaces.MediaObjects;

namespace MovieAssignmentInterfaces.ClassMaps
{
    public class VideosClassMap : ClassMap<Video>
    {
        public VideosClassMap()
        {
            Map(m => m.Id).Index(0).Name("videoId");
            Map(m => m.title).Index(1).Name("title");
            Map(m => m.Format).Index(2).Name("format").Convert(c => string.Join('|',c.Value.Format));
            Map(m => m.Length).Index(3).Name("length");
            Map(m => m.Regions).Index(4).Name("regions").Convert(c => string.Join('|',c.Value.Regions));;
        }
    }
}