using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramChatBot.CommandHandlers;

namespace TelegramChatBot.ButtonHandlers
{
    public class ComplaintButtonHandler : ButtonCallDataHandler
    {
        public ComplaintButtonHandler() => ButtonDataVariants = new List<HandlerData> { HandlerData.FromString("complaint") };

        public override async Task<bool> ValidateUser(Update _Update)
        {
            if (!await CheckIfUserInDatabase())
                return false;

            if (await CheckIfUserIsBanned())
                return false;

            return true;
        }

        protected override async Task Execute(CallbackQuery Callback)
        {
            int CallbackMessageId = Callback.Message.MessageId;

            await Bot.Client.EditMessageTextAsync(ChatId, Callback.Message.MessageId, Messages.ComplaintSent);

            User SuspectUser = _Database.Users[ButtonDataId];
            if (SuspectUser.ChatId == Bot.AdminChatId)
                return;

            List<string>? History = _Database.ComplaintMessagesHistory.Find(History => History.Messageid == CallbackMessageId)?.History;
            if (History == null)
                return;

            string SuspectMessageHistory = "История сообщений: \n";
            History.ForEach(MessageText => SuspectMessageHistory += MessageText + "\n");
            _Database.RemoveComplaintFromHistory(CallbackMessageId);

            InlineKeyboardButton BanButton = new InlineKeyboardButton("Забанить");
            InlineKeyboardButton UnbanButton = new InlineKeyboardButton("Разбанить");
            BanButton.CallbackData = $"ban_{SuspectUser.ChatId}";
            UnbanButton.CallbackData = $"unban_{SuspectUser.ChatId}";

            InlineKeyboardButton[][] Buttons = new InlineKeyboardButton[][]
            {
                new InlineKeyboardButton[] { BanButton },
                new InlineKeyboardButton[] { UnbanButton },
            };

            InlineKeyboardMarkup Markup = new InlineKeyboardMarkup(Buttons);

            if (NotificationCommandsHandler.SendComplaints)
                await Bot.Client.SendTextMessageAsync(Bot.AdminChatId, SuspectMessageHistory, replyMarkup: Markup);
        }
    }
}
