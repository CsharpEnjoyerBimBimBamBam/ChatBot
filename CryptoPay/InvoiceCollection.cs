using Newtonsoft.Json;

namespace TelegramChatBot.CryptoPay
{
    public class InvoiceCollection : CryptoBotResponse
    {
        [JsonProperty("result")] public GetInvoiceResult Result { get; private set; } = new GetInvoiceResult();

        public List<Invoice> Items => Result.Items;

        public class GetInvoiceResult
        {
            [JsonProperty("items")] public List<Invoice> Items { get; private set; } = new List<Invoice>();
        }
    }
}
