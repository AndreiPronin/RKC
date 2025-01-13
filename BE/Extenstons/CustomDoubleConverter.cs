using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using System;
using System.Globalization;

namespace BE.Extenstons
{
    public class CustomDoubleConverter : JsonConverter<double>
    {
        public override double ReadJson(JsonReader reader, Type objectType, double existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return 0;
            }

            var s = reader.Value.ToString().Replace('.', ',');
            double result;

            if (double.TryParse(s, out result))
            {
                return result;
            }

            return 0;
        }

        public override void WriteJson(JsonWriter writer, double value, JsonSerializer serializer)
        {
            writer.WriteValue(((decimal)value));
        }
    }
}
