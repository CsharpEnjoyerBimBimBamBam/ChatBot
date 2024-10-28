using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace TelegramChatBot.CryptoPay
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum InvoiceStatus
    {
        [EnumMember(Value = "active")] Active,
        [EnumMember(Value = "paid")] Paid,
        [EnumMember(Value = "expired")] Expired,
    }
}
