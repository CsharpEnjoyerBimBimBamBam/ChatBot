using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramChatBot.ButtonHandlers
{
    public class MSButtonHandler : ButtonCallDataHandler
    {
        public MSButtonHandler() => ButtonDataVariants = new List<HandlerData> { HandlerData.FromString("ms") };

        public override Task<bool> ValidateUser(Update _Update) => CheckIfUserIsAdmin();

        protected override async Task Execute(CallbackQuery Callback)
        {
            MandatorySubscription? Subscription = _Database.MandatorySubscriptions.Find(Subscription => Subscription.ChatId == ButtonDataId);

            if (Subscription == null)
            {
                await SendMessageText(Messages.MSNotFound);
                return;
            }

            await SendSubscriptionMessage(Subscription);
        }

        private async Task SendSubscriptionMessage(MandatorySubscription Subscription)
        {
            InlineKeyboardButton DeleteButton = new InlineKeyboardButton("Удалить");
            InlineKeyboardButton CalculateClicksCountButton = new InlineKeyboardButton("Посчитать кол-во подписок");
            InlineKeyboardButton ChangeLinkButton = new InlineKeyboardButton("Изменить ссылку");
            DeleteButton.CallbackData = $"remove_{Subscription.ChatId}";
            CalculateClicksCountButton.CallbackData = $"calcms_{Subscription.ChatId}";
            ChangeLinkButton.CallbackData = $"changemslink_{Subscription.ChatId}";
            InlineKeyboardButton[][] Buttons = new InlineKeyboardButton[][]
            {
                new InlineKeyboardButton[] { ChangeLinkButton },
                new InlineKeyboardButton[] { CalculateClicksCountButton },
                new InlineKeyboardButton[] { DeleteButton }
            };

            string SubscriptionMessage = $"Короткое имя - {Subscription.Username}\n" +
                                         $"Id - {Subscription.ChatId}\n" +
                                         $"Ссылка для перехода - {Subscription.Url}\n" +
                                         $"Последнее кол-во подписок - {Subscription.SubscribesCount}";

            await Bot.Client.SendTextMessageAsync(ChatId, SubscriptionMessage, replyMarkup: new InlineKeyboardMarkup(Buttons));
        }
    }
}
