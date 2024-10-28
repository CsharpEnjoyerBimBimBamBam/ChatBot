using Newtonsoft.Json;

namespace TelegramChatBot.CryptoPay
{
    public class ExchangeRate
    {
        [JsonProperty("is_valid")] public bool IsValid { get; private set; }
        [JsonProperty("is_crypto")] public bool IsCrypto { get; private set; }
        [JsonProperty("is_fiat")] public bool IsFiat { get; private set; }
        [JsonProperty("source")] public CryptoCurrencyAsset Source { get; private set; }
        [JsonProperty("target")] public FiatCurrencyAsset Target { get; private set; }
        [JsonProperty("rate")] public double Rate { get; private set; }
    }
}
