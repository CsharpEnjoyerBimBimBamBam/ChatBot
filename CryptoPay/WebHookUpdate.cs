using Newtonsoft.Json;

namespace TelegramChatBot.CryptoPay
{
    public class WebHookUpdate
    {
        [JsonProperty("update_id")] public long UpdateId { get; private set; }
        [JsonProperty("request_date")] public string RequestDate { get; private set; } = "";
        [JsonProperty("payload")] public Invoice Payload { get; private set; } = new Invoice();
    }
}
