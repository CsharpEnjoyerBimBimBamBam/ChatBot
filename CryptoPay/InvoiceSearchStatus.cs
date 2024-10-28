using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace TelegramChatBot.CryptoPay
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum InvoiceSearchStatus
    {
        [EnumMember(Value = "active")] Active,
        [EnumMember(Value = "paid")] Paid,
    }
}
