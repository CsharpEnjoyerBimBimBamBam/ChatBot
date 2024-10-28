using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramChatBot.CryptoPay
{
    public class GetExchangeRatesResponse : CryptoBotResponse
    {
        [JsonProperty("result")] public List<ExchangeRate> Result { get; private set; } = new List<ExchangeRate>();
    }
}
