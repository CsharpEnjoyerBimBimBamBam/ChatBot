using Telegram.Bot.Types;

namespace TelegramChatBot.ButtonHandlers
{
    public class RemoveMSButtonHandler : ButtonCallDataHandler
    {
        public RemoveMSButtonHandler() => ButtonDataVariants = new List<HandlerData> { HandlerData.FromString("remove") };

        public override Task<bool> ValidateUser(Update _Update) => CheckIfUserIsAdmin();

        protected override async Task Execute(CallbackQuery Callback)
        {
            MandatorySubscription? Subscription = _Database.MandatorySubscriptions.Find(Subscription => Subscription.ChatId == ButtonDataId);

            if (Subscription == null)
            {
                await SendMessageText(Messages.MSNotFound);
                return;
            }

            _Database.MandatorySubscriptions.Remove(Subscription);
            await SendMessageText(Messages.MSRemoved);
        }
    }
}
