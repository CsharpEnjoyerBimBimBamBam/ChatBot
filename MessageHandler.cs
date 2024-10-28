using Telegram.Bot.Types;

namespace TelegramChatBot
{
    public abstract class MessageHandler : UpdateHadler
    {
        protected string MessageText = "";

        protected abstract Task Execute(Message _Message);

        public override async Task Execute(Update _Update)
        {
            if (_Update.Message == null)
                throw new NullReferenceException(nameof(Update.Message));
            await Execute(_Update.Message);
        }

        public override void SetUpdateData(Update _Update)
        {
            ChatId = _Update.Message.Chat.Id;
            UserId = _Update.Message.From.Id;
            MessageText = _Update.Message.Text ?? "";
        }
    }
}
