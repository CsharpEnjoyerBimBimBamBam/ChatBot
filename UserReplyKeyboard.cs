using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramChatBot
{
    public class UserReplyKeyboard
    {
        private UserReplyKeyboard()
        {

        }

        public static string FindDialog = "🔍 Искать собеседника";
        public static string FindMale = "♂️ Найти парня";
        public static string FindFemale = "♀️ Найти девушку";
        public static string StopDialog = "❌Остановить диалог";
        public static string MyProfile = "📝 Мой профиль";
        public static string Premium = "🏆 Премиум";

        public static ReplyKeyboardMarkup Keyboard => GetKeyboard();

        public static ReplyKeyboardMarkup GetKeyboard()
        {
            List<List<KeyboardButton>> Buttons = new List<List<KeyboardButton>>
            {
                new List<KeyboardButton> { new KeyboardButton(FindDialog) },
                new List<KeyboardButton> { new KeyboardButton(FindMale), new KeyboardButton(FindFemale) },
                new List<KeyboardButton> { new KeyboardButton(StopDialog) },
                new List<KeyboardButton> { new KeyboardButton(MyProfile) },
                new List<KeyboardButton> { new KeyboardButton(Premium) }
            };

            return new ReplyKeyboardMarkup(Buttons);
        }
    }
}
