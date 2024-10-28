using Telegram.Bot.Types;

namespace TelegramChatBot.CommandHandlers
{
    public class BanCommandsHandler : CommandHadler
    {
        public BanCommandsHandler() => Commands = new List<HandlerData> { HandlerData.FromString("/ban"), HandlerData.FromString("/unban") };

        public override Task<bool> ValidateUser(Update _Update) => CheckIfUserIsAdmin();

        protected override async Task Execute(Message _Message)
        {
            await SendMessageText(Messages.IndicateId);
            bool Ban = true;
            if (MessageText == "/unban")
                Ban = false;
            Bot.RegisterUserNextStep(ChatId, (Message) => BanUser(Message.Text, Ban));
        }

        private async Task BanUser(string? MessageText, bool Ban)
        {
            if (MessageText == "/cancel")
                return;

            if (MessageText == null || !long.TryParse(MessageText, out long UserChatId) || !_Database.Users.ContainsKey(UserChatId))
            {
                await SendMessageText(Messages.InvalidId);
                Bot.RegisterUserNextStep(ChatId, (Message) => BanUser(Message.Text, Ban));
                return;
            }

            if (_Database.BannedUsers.Contains(UserChatId) && Ban)
            {
                await SendMessageText(Messages.UserAlreadyBanned);
                return;
            }

            if (!_Database.BannedUsers.Contains(UserChatId) && !Ban)
            {
                await SendMessageText(Messages.UserNotBanned);
                return;
            }

            if (Ban)
            {
                _Database.BannedUsers.Add(UserChatId);
                await SendMessageText(Messages.UserBanned);
                return;
            }
            _Database.BannedUsers.Remove(UserChatId);
            await SendMessageText(Messages.UserUnbanned);
        }
    }
}
