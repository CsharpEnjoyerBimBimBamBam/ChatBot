using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramChatBot.ButtonHandlers
{
    public class RatingButtonHandler : ButtonCallDataHandler
    {
        public RatingButtonHandler() => ButtonDataVariants = new List<HandlerData> { HandlerData.FromString("positive"), HandlerData.FromString("negative") };

        public override async Task<bool> ValidateUser(Update _Update)
        {
            if (!await CheckIfUserInDatabase())
                return false;

            if (await CheckIfUserIsBanned())
                return false;

            return true;
        }

        protected override async Task Execute(CallbackQuery Callback)
        {
            await Bot.Client.EditMessageTextAsync(ChatId, Callback.Message.MessageId, Messages.RatingThanks);

            User RatedUser = _Database.Users[ButtonDataId];

            if (ButtonData == "positive")
            {
                RatedUser.Rating++;
                return;
            }
            RatedUser.Rating--;
        }
    }
}
