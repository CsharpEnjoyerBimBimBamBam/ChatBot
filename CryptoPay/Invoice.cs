using Newtonsoft.Json;

namespace TelegramChatBot.CryptoPay
{
    public class Invoice
    {
        [JsonProperty("invoice_id")] public int InvoiceId { get; private set; }
        [JsonProperty("hash")] public string Hash { get; private set; } = "";
        [JsonProperty("currency_type")] public CurrencyType CurrencyType { get; private set; }
        [JsonProperty("amount")] public double Amount { get; private set; }
        [JsonProperty("status")] public InvoiceStatus Status { get; private set; }
        [JsonProperty("payload")] public string PayLoad { get; private set; } = "";
        [JsonProperty("pay_url")] public string PayUrl { get; private set; } = "";
        [JsonProperty("bot_invoice_url")] public string BotInvoiceUrl { get; private set; } = "";
        [JsonProperty("asset")] public CryptoCurrencyAsset Asset { get; private set; }
    }
}
