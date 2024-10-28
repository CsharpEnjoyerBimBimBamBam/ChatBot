using Telegram.Bot.Types;

namespace TelegramChatBot.CommandHandlers
{
    public class NextCommandHandler : CommandHadler
    {
        public NextCommandHandler() => Commands = new List<HandlerData> 
        {
            HandlerData.FromString("/next"), 
            HandlerData.FromString(UserReplyKeyboard.FindDialog),
            HandlerData.FromString("Искать собеседника")
        };

        protected override Task Execute(Message _Message) => new UserFunction(ChatId, Bot).FindDialog();
    }
}
