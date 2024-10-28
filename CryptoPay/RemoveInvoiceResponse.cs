using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramChatBot.CryptoPay
{
    public class RemoveInvoiceResponse : CryptoBotResponse
    {
        [JsonProperty("result")] public bool Result { get; private set; }
    }
}
