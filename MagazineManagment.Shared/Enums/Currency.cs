using System.Text.Json.Serialization;

namespace MagazineManagment.Shared.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CurrencyTypeEnum
    {
        Euro = 1,
        Dollar,
        Lek
    }
}