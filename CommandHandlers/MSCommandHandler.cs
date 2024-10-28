using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramChatBot.CommandHandlers
{
    public class MSCommandHandler : CommandHadler
    {
        public MSCommandHandler() => Commands = new List<HandlerData> { HandlerData.FromString("/ms") };

        public override Task<bool> ValidateUser(Update _Update) => CheckIfUserIsAdmin();

        protected override async Task Execute(Message _Message)
        {
            List<List<InlineKeyboardButton>> Buttons = new List<List<InlineKeyboardButton>>();

            foreach (MandatorySubscription Subscription in _Database.MandatorySubscriptions)
            {
                InlineKeyboardButton Button = new InlineKeyboardButton(Subscription.Username);
                Button.CallbackData = $"ms_{Subscription.ChatId}";
                Buttons.Add(new List<InlineKeyboardButton> { Button });
            }

            InlineKeyboardButton AddButton = new InlineKeyboardButton("Добавить");
            AddButton.CallbackData = "add_ms";
            Buttons.Add(new List<InlineKeyboardButton> { AddButton });

            InlineKeyboardMarkup Markup = new InlineKeyboardMarkup(Buttons);

            if (Buttons.Count > 1)
            {
                await Bot.Client.SendTextMessageAsync(ChatId, Messages.ActiveMS, replyMarkup: Markup);
                return;
            }

            await Bot.Client.SendTextMessageAsync(ChatId, Messages.NoActiveMS, replyMarkup: Markup);
        }
    }
}
