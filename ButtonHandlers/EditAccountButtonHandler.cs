using Telegram.Bot.Types;

namespace TelegramChatBot.ButtonHandlers
{
    public class EditAccountButtonHandler : InlineButtonHandler
    {
        public EditAccountButtonHandler() => CallDataVariants = new List<HandlerData> { HandlerData.FromString("edit_account") };

        protected override async Task Execute(CallbackQuery Callback)
        {
            await SendMessageText(Messages.IndicateAge);
            User.Age = null;
            User.Sex = null;
            Bot.RegisterUserNextStep(ChatId, new UserFunction(ChatId, Bot).Register);
        }
    }
}
