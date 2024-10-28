using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramChatBot.ButtonHandlers
{
    public class ChangeMSLinkButtonHandler : ButtonCallDataHandler
    {
        public ChangeMSLinkButtonHandler() => ButtonDataVariants = new List<HandlerData> { HandlerData.FromString("changemslink") };

        public override Task<bool> ValidateUser(Update _Update) => CheckIfUserIsAdmin();

        protected override async Task Execute(CallbackQuery Callback)
        {
            MandatorySubscription? Subscription = _Database.MandatorySubscriptions.Find(Subscription => Subscription.ChatId == ButtonDataId);

            if (Subscription == null)
            {
                await SendMessageText(Messages.MSNotFound);
                return;
            }

            await SendMessageText(Messages.EnterLink);
            Bot.RegisterUserNextStep(ChatId, (Message) => ChangeLink(Message, Subscription));
        }

        private async Task ChangeLink(Message _Message, MandatorySubscription Subscription)
        {
            if (string.IsNullOrEmpty(_Message?.Text))
            {
                await SendMessageText(Messages.InvalidLink);
                Bot.RegisterUserNextStep(ChatId, (Message) => ChangeLink(Message, Subscription));
                return;
            }

            if (_Message.Text == "/cancel")
                return;

            if (!Uri.IsWellFormedUriString(_Message.Text, UriKind.Absolute))
            {
                await SendMessageText(Messages.InvalidLink);
                Bot.RegisterUserNextStep(ChatId, (Message) => ChangeLink(Message, Subscription));
                return;
            }

            Subscription.Url = _Message.Text;
            await SendMessageText(Messages.LinkChanged);
        }
    }
}
