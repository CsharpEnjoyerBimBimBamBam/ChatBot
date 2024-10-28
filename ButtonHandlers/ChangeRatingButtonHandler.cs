using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TelegramChatBot.ButtonHandlers
{
    public class ChangeRatingButtonHandler : ButtonCallDataHandler
    {
        public ChangeRatingButtonHandler() => ButtonDataVariants = new List<HandlerData> { HandlerData.FromString("changerating") };

        public override Task<bool> ValidateUser(Update _Update) => CheckIfUserIsAdmin();

        protected override async Task Execute(CallbackQuery Callback)
        {
            await SendMessageText("Отправьте новый рейтинг пользователя\n/cancel - для отмены");
            Bot.RegisterUserNextStep(ChatId, ChangeUserRating);
        }

        private async Task ChangeUserRating(Message _Message)
        {
            string InvalidRating = "Отправьте корректный рейтинг";

            if (_Message?.Text == null)
            {
                await SendMessageText(InvalidRating);
                Bot.RegisterUserNextStep(ChatId, ChangeUserRating);
                return;
            }

            if (_Message.Text == "/cancel")
                return;

            if (!int.TryParse(_Message.Text, out int Rating))
            {
                await SendMessageText(InvalidRating);
                Bot.RegisterUserNextStep(ChatId, ChangeUserRating);
                return;
            }

            _Database.Users[ButtonDataId].Rating = Rating;
            await SendMessageText($"Рейтинг пользователя с id {ButtonDataId} изменен на {Rating}");
        }
    }
}
