using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramChatBot.CommandHandlers
{
    public class UserInfoCommandHandler : CommandHadler
    {
        public UserInfoCommandHandler() => Commands = new List<HandlerData> { HandlerData.FromString("/user_info") };

        public override Task<bool> ValidateUser(Update _Update) => CheckIfUserIsAdmin();

        protected override async Task Execute(Message _Message)
        {
            await SendMessageText(Messages.IndicateId);
            Bot.RegisterUserNextStep(ChatId, SendUserInfo);
        }

        private async Task SendUserInfo(Message _Message)
        {
            if (_Message?.Text == null)
            {
                await Bot.Client.SendTextMessageAsync(Bot.AdminChatId, Messages.InvalidId);
                Bot.RegisterUserNextStep(ChatId, SendUserInfo);
                return;
            }

            if (_Message.Text == "/cancel")
                return;

            if (!long.TryParse(_Message.Text, out long Id) || !_Database.Users.ContainsKey(Id))
            {
                await Bot.Client.SendTextMessageAsync(Bot.AdminChatId, Messages.InvalidId);
                Bot.RegisterUserNextStep(ChatId, SendUserInfo);
                return;
            }

            InlineKeyboardButton ChangeRatingButton = new InlineKeyboardButton("Изменить рейтинг");
            ChangeRatingButton.CallbackData = $"changerating_{Id}";

            await Bot.Client.SendTextMessageAsync(Bot.AdminChatId, AccountCommandHandler.CreateAcoountMessage(_Database.Users[Id]), 
                replyMarkup: new InlineKeyboardMarkup(ChangeRatingButton));
        }
    }
}
