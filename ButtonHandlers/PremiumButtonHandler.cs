using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Payments;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramChatBot.CryptoPay;

namespace TelegramChatBot.ButtonHandlers
{
    public class PremiumButtonHandler : ButtonCallDataHandler
    {
        public PremiumButtonHandler() => ButtonDataVariants = new List<HandlerData>{ HandlerData.FromString("premium") };

        public override Task<bool> ValidateUser(Update _Update) => CheckIfUserInDatabase();

        protected override async Task Execute(CallbackQuery Callback)
        {
            int Sum = 15;
            string Days = "день";
            if (ButtonDataId == 3)
            {
                Sum = 35;
                Days = "дня";
            }

            if (ButtonDataId == 7)
            {
                Sum = 75;
                Days = "дней";
            }

            if (ButtonDataId == 30)
            {
                Sum = 250;
                Days = "дней";
            }

            List<LabeledPrice> Labels = new List<LabeledPrice> { new LabeledPrice("1", Sum) };
            await Bot.Client.SendInvoiceAsync(ChatId, "Премиум подписка", $"Оплатить подписку на {ButtonDataId} {Days}", ButtonDataId.ToString(), "", "XTR", Labels);
        }
    }
}
