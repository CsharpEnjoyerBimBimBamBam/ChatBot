using Telegram.Bot.Types;

namespace TelegramChatBot
{
    public abstract class InlineButtonHandler : UpdateHadler
    {
        public List<HandlerData> CallDataVariants { get; protected set; } = new List<HandlerData> { HandlerData.Any };
        protected string CurrentData = "";

        protected abstract Task Execute(CallbackQuery Callback);

        public override async Task Execute(Update _Update)
        {
            if (_Update.CallbackQuery == null)
                throw new NullReferenceException(nameof(Update.CallbackQuery));
            CurrentData = _Update?.CallbackQuery?.Data ?? "";
            await Execute(_Update.CallbackQuery);
        }

        public override void SetUpdateData(Update _Update)
        {
            ChatId = _Update.CallbackQuery.From.Id;
            UserId = _Update.CallbackQuery.From.Id;
        }
    }
}
