using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramChatBot.ButtonHandlers
{
    public class AddMSButtonHandler : InlineButtonHandler
    {
        public AddMSButtonHandler() => CallDataVariants = new List<HandlerData> { HandlerData.FromString("add_ms") };

        public override Task<bool> ValidateUser(Update _Update) => CheckIfUserIsAdmin();

        protected override async Task Execute(CallbackQuery Callback)
        {
            await SendMessageText(Messages.EnterUsername);
            Bot.RegisterUserNextStep(ChatId, AddMandatorySubscription);
        }

        private async Task AddMandatorySubscription(Message _Message)
        {
            string? Username = _Message.Text;

            if (Username == "/cancel")
                return;

            if (string.IsNullOrEmpty(Username) || Username.Length == 1 || Username[0].ToString() != "@")
            {
                await SendMessageText(Messages.EnterCorrectUsername);
                Bot.RegisterUserNextStep(ChatId, AddMandatorySubscription);
                return;
            }

            Chat _Chat;

            try
            {
                _Chat = await Bot.Client.GetChatAsync(new ChatId(Username));
            }
            catch
            {
                await SendMessageText(Messages.UsernameNotFound);
                Bot.RegisterUserNextStep(ChatId, AddMandatorySubscription);
                return;
            }

            MandatorySubscription Subscription = new MandatorySubscription
            {
                Username = Username,
                ChatId = _Chat.Id,
                Url = $"https://t.me/{Username.Substring(1)}"
            };

            _Database.MandatorySubscriptions.Add(Subscription);
            await SendMessageText(Messages.MSAdded);
        }
    }
}
