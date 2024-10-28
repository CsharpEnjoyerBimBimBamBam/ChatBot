using Telegram.Bot.Types;

namespace TelegramChatBot
{
    public abstract class CommandHadler : MessageHandler
    {
        public List<HandlerData> Commands { get; protected set; } = new List<HandlerData> { HandlerData.Any };

        public override async Task Execute(Update _Update)
        {
            if (_Update.Message?.Text == null)
                throw new NullReferenceException(nameof(Update.Message.Text));
            await Execute(_Update.Message);
        }
    }
}
