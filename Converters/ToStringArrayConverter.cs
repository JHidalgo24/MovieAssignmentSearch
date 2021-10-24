using System.Collections.Generic;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace MovieAssignmentInterfaces.Converters
{
    public class ToStringArrayConverter : TypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (text == "")
            { return new List<string>(); }
            string[] allElements = text.Split('|');
            return new List<string>(allElements);


        }

        public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            return string.Join("|", ((List<string>)value).ToArray());
        }
    }
}