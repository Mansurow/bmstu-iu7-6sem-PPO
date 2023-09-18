using System.Globalization;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace Portal.Common.JsonConverter;

public class CustomDateOnlyConverter: JsonConverter<DateOnly>
{
    private readonly string _format;
    
    public CustomDateOnlyConverter()
    {
        _format = "dd.MM.yyyy";
    }
    
    public CustomDateOnlyConverter(string format)
    {
        _format = format;
    }
    
    public override DateOnly ReadJson(JsonReader reader,
        Type objectType,
        DateOnly existingValue,
        bool hasExistingValue,
        JsonSerializer serializer) =>
        DateOnly.ParseExact((string)reader.Value!, _format, CultureInfo.InvariantCulture);

    public override void WriteJson(JsonWriter writer, DateOnly value, JsonSerializer serializer) => 
        writer.WriteValue(value.ToString(_format, CultureInfo.InvariantCulture));
}