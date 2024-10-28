using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TelegramChatBot.CryptoPay
{
    public enum CurrencyType
    {
        [EnumMember(Value = "crypto")] Crypto,
        [EnumMember(Value = "fiat")] Fiat
    }
}
