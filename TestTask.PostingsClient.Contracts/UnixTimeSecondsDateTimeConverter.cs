using System.Text.Json;
using System.Text.Json.Serialization;

namespace TestTask.PostingsClient.Contracts
{
    public class UnixTimeSecondsDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number && reader.TryGetInt64(out long seconds))
            {
                // Convert Unix timestamp to UTC DateTime
                return DateTimeOffset.FromUnixTimeSeconds(seconds).UtcDateTime;
            }

            throw new JsonException("Invalid Unix timestamp");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            // Convert DateTime to Unix timestamp in seconds
            long seconds = ((DateTimeOffset)value.ToUniversalTime()).ToUnixTimeSeconds();
            writer.WriteNumberValue(seconds);
        }
    }

}
