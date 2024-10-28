using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramChatBot
{
    public class StartCommandHandler : CommandHadler
    {
        public StartCommandHandler() => Commands = new List<HandlerData> { HandlerData.FromString("/start") };
        public static int NewUsersCount { get; private set; } = 0;

        public override Task<bool> ValidateUser(Update _Update) => Task.FromResult(true);

        protected override async Task Execute(Message _Message)
        {
            if (User != null)
            {
                await SendMessageText(Messages.AlreadyRegistered);
                return;
            }
            NewUsersCount++;
            await Bot.Client.SendTextMessageAsync(ChatId, Messages.Welcome, replyMarkup: UserReplyKeyboard.Keyboard);
            User NewUser = new User(ChatId);
            NewUser.AddPremiumTime(TimeSpan.FromHours(1));
            _Database.Users[ChatId] = NewUser;
            await SendMessageText(Messages.IndicateAge);
            Bot.RegisterUserNextStep(ChatId, new UserFunction(ChatId, Bot).Register);
        }
    }
}
