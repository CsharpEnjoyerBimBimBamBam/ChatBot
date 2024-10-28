using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramChatBot.CryptoPay
{
    public class CreateInvoiceResponse : CryptoBotResponse
    {
        [JsonProperty("result")] public Invoice Result { get; private set; } = new Invoice();
    }
}
