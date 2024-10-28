using Telegram.Bot.Types;

namespace TelegramChatBot.CommandHandlers
{
    public class StopCommandHandler : CommandHadler
    {
        public StopCommandHandler() => Commands = new List<HandlerData> 
        { 
            HandlerData.FromString("/stop"),
            HandlerData.FromString(UserReplyKeyboard.StopDialog),
            HandlerData.FromString("Остановить диалог")
        };

        protected override async Task Execute(Message _Message)
        {
            await new UserFunction(ChatId, Bot).StopDialog();
        }
    }
}
