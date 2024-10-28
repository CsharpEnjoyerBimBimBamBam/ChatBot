using Newtonsoft.Json;

namespace TelegramChatBot.CryptoPay
{
    public abstract class CryptoBotResponse
    {
        [JsonProperty("ok")] public bool Ok { get; protected set; }
    }
}
