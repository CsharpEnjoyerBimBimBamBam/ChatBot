using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Payments;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramChatBot.CryptoPay;

namespace TelegramChatBot.CommandHandlers
{
    public class PremiumCommandHandler : CommandHadler
    {
        public PremiumCommandHandler() => Commands = new List<HandlerData>
        {
            HandlerData.FromString("/premium"),
            HandlerData.FromString(UserReplyKeyboard.Premium),
            HandlerData.FromString("Премиум")
        };

        private string _PremiumMessage = "Приобретая премиум, вы получаете следующие преимущества:\n\n" +
                                         "🔍 Поиск по полу\n" +
                                         "ℹ️ Данные о собеседнике (пол и возраст)\n" +
                                         "🚫 Отсутствие рекламы\n\n" +
                                         "/referals - Получить премиум бесплатно";

        public override Task<bool> ValidateUser(Update _Update) => CheckIfUserInDatabase();

        protected override async Task Execute(Message _Message) => await SendPremiumMessage();

        private async Task SendPremiumMessage()
        {
            InlineKeyboardButton OneDayButton = new InlineKeyboardButton("На один день - 15⭐");
            InlineKeyboardButton ThreeDaysButton = new InlineKeyboardButton("На три дня - 35⭐");
            InlineKeyboardButton OneWeekButton = new InlineKeyboardButton("На неделю - 75⭐");
            InlineKeyboardButton OneMonthButton = new InlineKeyboardButton("На месяц - 250⭐");

            string BaseCallData = "premium_";

            OneDayButton.CallbackData = BaseCallData + "1";
            ThreeDaysButton.CallbackData = BaseCallData + "3";
            OneWeekButton.CallbackData = BaseCallData + "7";
            OneMonthButton.CallbackData = BaseCallData + "30";

            List<List<InlineKeyboardButton>> PremiumButtons = new List<List<InlineKeyboardButton>>
            {
                new List<InlineKeyboardButton> { OneDayButton },
                new List<InlineKeyboardButton> { ThreeDaysButton },
                new List<InlineKeyboardButton> { OneWeekButton },
                new List<InlineKeyboardButton> { OneMonthButton },
            };

            await Bot.Client.SendTextMessageAsync(ChatId, _PremiumMessage, replyMarkup: new InlineKeyboardMarkup(PremiumButtons));
        }
    }
}
