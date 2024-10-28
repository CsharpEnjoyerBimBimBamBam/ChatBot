using Telegram.Bot.Types;

namespace TelegramChatBot.ButtonHandlers
{
    public class RemoveAdButtonHandler : InlineButtonHandler
    {
        public RemoveAdButtonHandler() => CallDataVariants = new List<HandlerData> { HandlerData.FromString("removead") };

        public override Task<bool> ValidateUser(Update _Update) => CheckIfUserInDatabase();

        protected override async Task Execute(CallbackQuery Callback)
        {
            await Bot.ImitateCommand(ChatId, UserReplyKeyboard.Premium);
        }
    }
}
