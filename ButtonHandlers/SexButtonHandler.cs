using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramChatBot.ButtonHandlers
{
    public class SexButtonHandler : InlineButtonHandler
    {
        public SexButtonHandler() => CallDataVariants = new List<HandlerData> 
        {
            HandlerData.FromString(Sex.Male.ToString()),
            HandlerData.FromString(Sex.Female.ToString())
        };

        public override Task<bool> ValidateUser(Update _Update) => Task.FromResult(true);

        protected override async Task Execute(CallbackQuery Query)
        {
            Sex UserSex = Enum.Parse<Sex>(CurrentData);
            _Database.Users[ChatId].Sex = UserSex;

            await Bot.Client.DeleteMessageAsync(ChatId, Query.Message.MessageId);
            await SendMessageText(Messages.AfterRegistration);
        }
    }
}
