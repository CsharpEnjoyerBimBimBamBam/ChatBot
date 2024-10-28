using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramChatBot
{
    public class EditedMessageHandler : UpdateHadler
    {
        private Message? _EditedMessage;

        public override Task<bool> ValidateUser(Update _Update)
        {
            if (_Database.BannedUsers.Contains(ChatId))
                return Task.FromResult(false);

            if (User == null)
                return Task.FromResult(false);

            if (!User.InDialogue || User.Companion == null)
                return Task.FromResult(false);

            if (string.IsNullOrEmpty(_EditedMessage?.Text))
                return Task.FromResult(false);
            return Task.FromResult(true);
        }

        public override void SetUpdateData(Update _Update)
        {
            if (_Update.EditedMessage == null)
                throw new NullReferenceException(nameof(Update.EditedMessage));

            ChatId = _Update.EditedMessage.Chat.Id;
            UserId = _Update.EditedMessage.From.Id;
            _EditedMessage = _Update.EditedMessage;
        }

        public override async Task Execute(Update _Update)
        {
            if (!User.MessagesIDs.ContainsKey(_EditedMessage.MessageId))
                return;

            int MessageId = User.MessagesIDs[_EditedMessage.MessageId];

            await Bot.Client.EditMessageTextAsync(User.Companion.ChatId, MessageId, _EditedMessage.Text);
        }
    }
}
