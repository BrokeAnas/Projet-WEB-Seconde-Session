using System.Text.Json;
using System.Text.Json.Serialization;

namespace MediCareManager.Core.Common;

/// <summary>
/// Sérialise les identifiants <see cref="long"/> (numéros de Registre National) en chaîne JSON,
/// afin de préserver les 11 chiffres côté Angular (où id_nat est typé <c>string</c>).
/// Gère également la lecture depuis une chaîne ou un nombre.
/// </summary>
public sealed class LongToStringJsonConverter : JsonConverter<long>
{
    public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var s = reader.GetString();
            return long.TryParse(s, out var value) ? value : 0L;
        }
        return reader.GetInt64();
    }

    public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString());
}
