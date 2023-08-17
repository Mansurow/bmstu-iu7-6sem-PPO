using System.Globalization;
using Newtonsoft.Json;

namespace Portal.Json;

public class CustomTimeOnlyConverter: JsonConverter<TimeOnly>
{
    private readonly string _format;
    public CustomTimeOnlyConverter(string format)
    {
        _format = format;
    }
    
    public override TimeOnly ReadJson(JsonReader reader,
        Type objectType,
        TimeOnly existingValue,
        bool hasExistingValue,
        JsonSerializer serializer) =>
        TimeOnly.ParseExact((string)reader.Value, _format, CultureInfo.InvariantCulture);

    public override void WriteJson(JsonWriter writer, TimeOnly value, JsonSerializer serializer) => 
        writer.WriteValue(value.ToString(_format, CultureInfo.InvariantCulture));
}