using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Converter
{
    public class DateConverter : JsonConverter<DateTime>
    {
        private const string DateFormat = "dd MM yyyy";

        public override void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString(DateFormat));
        }

        public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (DateTime.TryParseExact(reader.Value.ToString(), DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                return result;
            }
            throw new JsonSerializationException($"Invalid datetime format. Expected format: {DateFormat}");
        }
    }
}
