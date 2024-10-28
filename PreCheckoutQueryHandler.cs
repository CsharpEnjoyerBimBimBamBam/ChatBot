using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Payments;

namespace TelegramChatBot
{
    public abstract class PreCheckoutQueryHandler : UpdateHadler
    {
        protected abstract Task Execute(PreCheckoutQuery Query);

        public override async Task Execute(Update _Update)
        {
            if (_Update?.PreCheckoutQuery == null)
                throw new NullReferenceException(nameof(Update.PreCheckoutQuery));
            await Execute(_Update.PreCheckoutQuery);
        }

        public override void SetUpdateData(Update _Update)
        {
            ChatId = _Update.PreCheckoutQuery.From.Id;
            UserId = _Update.PreCheckoutQuery.From.Id;
        }
    }
}
