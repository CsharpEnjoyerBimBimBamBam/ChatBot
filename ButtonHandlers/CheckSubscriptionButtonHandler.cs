using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramChatBot.ButtonHandlers
{
    public class CheckSubscriptionButtonHandler : InlineButtonHandler
    {
        public CheckSubscriptionButtonHandler() => CallDataVariants = new List<HandlerData> { HandlerData.FromString("checksub") };

        public override async Task<bool> ValidateUser(Update _Update) => await CheckIfUserInDatabase() && await VerifyUserSubscribes();

        protected override async Task Execute(CallbackQuery Callback)
        {
            await Bot.Client.DeleteMessageAsync(ChatId, Callback.Message.MessageId);
            await SendMessageText(Messages.Subscribed);
        }
    }
}
