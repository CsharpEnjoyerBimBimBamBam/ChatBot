using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Payments;

namespace TelegramChatBot
{
    public class PremiumPaymentHandler : PreCheckoutQueryHandler
    {
        public override Task<bool> ValidateUser(Update _Update) => CheckIfUserInDatabase();

        protected override async Task Execute(PreCheckoutQuery Query)
        {
            await Bot.Client.AnswerPreCheckoutQueryAsync(Query.Id);
            User.AddPremiumTime(TimeSpan.FromDays(int.Parse(Query.InvoicePayload)));
            await SendMessageText(Messages.PaymentCompleted);

            await Bot.Client.SendTextMessageAsync(Bot.AdminChatId, $"Была куплена премиум подписка на {Query.InvoicePayload} дней");
        }
    }
}
