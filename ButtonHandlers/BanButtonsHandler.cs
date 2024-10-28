using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramChatBot.ButtonHandlers
{
    public class BanButtonsHandler : ButtonCallDataHandler
    {
        public BanButtonsHandler() => ButtonDataVariants = new List<HandlerData> { HandlerData.FromString("ban"), HandlerData.FromString("unban") };

        public override Task<bool> ValidateUser(Update _Update) => CheckIfUserIsAdmin();

        protected override async Task Execute(CallbackQuery Callback)
        {
            User SuspectUser = _Database.Users[ButtonDataId];
            if (SuspectUser.ChatId == Bot.AdminChatId)
                return;

            if (ButtonData == "ban" && _Database.BannedUsers.Contains(SuspectUser.ChatId))
                return;

            if (ButtonData == "unban" && !_Database.BannedUsers.Contains(SuspectUser.ChatId))
                return;

            if (ButtonData == "ban")
            {
                _Database.BannedUsers.Add(SuspectUser.ChatId);
                await Bot.Client.SendTextMessageAsync(Bot.AdminChatId, Messages.UserBanned);
                return;
            }

            _Database.BannedUsers.Remove(SuspectUser.ChatId);
            await Bot.Client.SendTextMessageAsync(Bot.AdminChatId, Messages.UserUnbanned);
        }
    }
}
