using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramChatBot.CommandHandlers
{
    public class AccountCommandHandler : CommandHadler
    {
        public AccountCommandHandler() => Commands = new List<HandlerData> 
        {
            HandlerData.FromString("/account"),
            HandlerData.FromString(UserReplyKeyboard.MyProfile),
            HandlerData.FromString("Мой профиль")
        };

        protected override async Task Execute(Message _Message)
        {
            InlineKeyboardButton ChangeButton = new InlineKeyboardButton("Изменить");
            ChangeButton.CallbackData = "edit_account";
            InlineKeyboardMarkup Markup = new InlineKeyboardMarkup(ChangeButton);
            await Bot.Client.SendTextMessageAsync(ChatId, CreateAcoountMessage(User), replyMarkup: Markup);
        }

        public static string CreateAcoountMessage(User _User)
        {
            string PremiumType = "Стандартная";
            if (_User.IsPremium)
               PremiumType = $"Премиум (осталось {_User.PremiumExpirationTime.Days} дней {_User.PremiumExpirationTime.Hours} часов)";

            string _Age = _User.Age?.ToString() ?? "Не указан";

            return $"Id: {_User.ChatId}\n" +
                   $"Рейтинг: {_User.Rating}\n" +
                   $"Пол: {_User.Sex.Format()}\n" +
                   $"Возраст: {_Age}\n" +
                   $"Тип подписки: {PremiumType}";
        }
    }
}
