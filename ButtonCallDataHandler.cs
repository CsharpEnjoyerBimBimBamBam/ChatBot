using Telegram.Bot.Types;

namespace TelegramChatBot
{
    public abstract class ButtonCallDataHandler : UpdateHadler
    {
        public List<HandlerData> ButtonDataVariants { get; protected set; } = new List<HandlerData> { HandlerData.Any };
        protected long ButtonDataId;
        protected string ButtonData = "";
        private string CurrentData = "";

        protected abstract Task Execute(CallbackQuery Callback);

        public override async Task Execute(Update _Update)
        {
            if (_Update?.CallbackQuery == null)
                throw new NullReferenceException(nameof(Update.CallbackQuery));
            CurrentData = _Update.CallbackQuery.Data ?? "";
            if (!ValidateCallData())
                return;
            await Execute(_Update.CallbackQuery);
        }

        public override void SetUpdateData(Update _Update)
        {
            ChatId = _Update.CallbackQuery.From.Id;
            UserId = _Update.CallbackQuery.From.Id;
        }

        private bool ValidateCallData()
        {
            try
            {
                string[] SplitedData = CurrentData.Split("_");
                string _ButtonData = SplitedData[0];

                ButtonData = _ButtonData;
                ButtonDataId = long.Parse(SplitedData[1]);

                if (!ButtonDataVariants.Any(Data => Data.Text == _ButtonData))
                    return false;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
