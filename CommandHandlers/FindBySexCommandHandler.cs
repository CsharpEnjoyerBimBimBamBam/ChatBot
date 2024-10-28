using Telegram.Bot.Types;

namespace TelegramChatBot.CommandHandlers
{
    public class FindBySexCommandHandler : CommandHadler
    {
        public FindBySexCommandHandler() => Commands = new List<HandlerData> 
        {
            HandlerData.FromString("/male"),
            HandlerData.FromString(UserReplyKeyboard.FindMale),
            HandlerData.FromString("/female"),
            HandlerData.FromString(UserReplyKeyboard.FindFemale),
            HandlerData.FromString("Найти парня"),
            HandlerData.FromString("Найти девушку")
        };

        public override async Task<bool> ValidateUser(Update _Update)
        {
            if (!await base.ValidateUser(_Update))
                return false;

            return await CheckIfUserIsPremium();
        }

        protected override async Task Execute(Message _Message)
        {
            Sex SearchSex = Sex.Male;
            if (MessageText == "/female" || MessageText == UserReplyKeyboard.FindFemale)
                SearchSex = Sex.Female;
            await new UserFunction(ChatId, Bot).FindDialog(SearchSex);
        }
    }
}
