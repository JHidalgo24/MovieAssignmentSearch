using System.Collections.Generic;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

/*I got the converters from someone on stackoverflow to convert the genres, regions, and writers into an array 
instead of keeping it a string
https://stackoverflow.com/questions/60966194/using-csvhelper-to-read-cell-content-into-list-or-array 
*/
namespace MovieAssignmentInterfaces.Converters
{
    public class ToIntArrayConverter : TypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            string[] allElements = text.Split(',');//turns the string into an array
            int[] elementsAsInteger = allElements.Select(s => int.Parse(s)).ToArray();//parses the values from string to int array
            return new List<int>(elementsAsInteger);
        }

        public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            return string.Join('|', ((List<int>)value).ToArray());//turns the array into a string, I didn't really use it
        }

    }
}