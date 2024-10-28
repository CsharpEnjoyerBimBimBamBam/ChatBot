using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace TelegramChatBot
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Sex
    {
        [EnumMember(Value = "male")] Male,
        [EnumMember(Value = "female")] Female
    }
}
