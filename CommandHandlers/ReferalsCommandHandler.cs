using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramChatBot.CommandHandlers
{
    public class ReferalsCommandHandler : CommandHadler
    {
        public ReferalsCommandHandler() => Commands = new List<HandlerData> { HandlerData.FromString("/referals") };

        public override Task<bool> ValidateUser(Update _Update) => CheckIfUserInDatabase();

        protected override async Task Execute(Message _Message)
        {
            string? BotUsername = (await Bot.Client.GetMeAsync()).Username;
            if (string.IsNullOrEmpty(BotUsername))
            {
                await SendMessageText(Messages.ReferalsNotAvailable);
                return;
            }

            string ReferalLink = $"https://t.me/{BotUsername}?start={ChatId}";
            await SendMessageText(CreateReferalsMessage(ReferalLink));
        }

        private string CreateReferalsMessage(string ReferalLink)
        {
            return $"Приглашайте пользователей по своей реферальной ссылке и получайте премиум статус на 1 час за каждого\n\n" +
                   $"Ваша реферальная ссылка:\n" +
                   $"{ReferalLink}";
        }
    }
}
