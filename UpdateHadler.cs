using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Exceptions;

namespace TelegramChatBot
{
    public abstract class UpdateHadler
    {
        public ChatBot Bot;
        protected long ChatId;
        protected long UserId;
        protected Database _Database = Database.Load();

        protected User User
        { 
            get 
            {
                if (!_Database.Users.ContainsKey(ChatId))
                    return null;
                return _Database.Users[ChatId];
            } 
        }

        public abstract Task Execute(Update _Update);

        public abstract void SetUpdateData(Update _Update);

        public virtual async Task<bool> ValidateUser(Update _Update)
        {
            if (!await CheckIfUserInDatabase())
                return false;

            if (!await VerifyUserSubscribes())
                return false;

            if (await CheckIfUserIsBanned())
                return false;

            return true;
        }

        public void UpdateUserPremium()
        {
            if (User == null)
                return;

            if (User.IsPremium && User.PremiumExpirationTime <= TimeSpan.Zero)
                User.PremiumExpirationDate = null;
        }

        protected async Task<bool> CheckIfUserIsPremium()
        {
            if (!User.IsPremium)
            {
                await SendMessageText(Messages.PremiumRequired);
                return false;
            }

            return true;
        }

        protected async Task<bool> CheckIfUserInDatabase()
        {
            if (User == null || User.Age == null || User.Sex == null)
            {
                await SendMessageText(Messages.NotRegistered);
                return false;
            }
            return true;
        }

        protected async Task<bool> CheckIfUserIsBanned()
        {
            if (_Database.BannedUsers.Contains(ChatId))
            {
                await SendMessageText(Messages.Banned);
                return true;
            }
            return false;
        }

        protected Task<bool> CheckIfUserIsAdmin() => Task.FromResult(ChatId == Bot.AdminChatId);

        protected async Task<bool> CheckIfUserInDialog()
        {
            if (!User.InDialogue)
            {
                await SendMessageText(Messages.NotInDialogue);
                return false;
            }
            return true;
        }

        protected async Task<bool> VerifyUserSubscribes()
        {
            if (User.IsPremium)
                return true;
            bool IsSubscribed = true;
            List<List<InlineKeyboardButton>> Buttons = new List<List<InlineKeyboardButton>>();
            int CurrentSubscribe = 1;
            foreach (MandatorySubscription Subscription in _Database.MandatorySubscriptions)
            {
                try
                {
                    ChatMember Member = await Bot.Client.GetChatMemberAsync(new ChatId(Subscription.ChatId), UserId);
                    if (Member.Status == ChatMemberStatus.Member || Member.Status == ChatMemberStatus.Creator || Member.Status == ChatMemberStatus.Administrator)
                        continue;

                    InlineKeyboardButton Button = new InlineKeyboardButton($"{CurrentSubscribe}. Подписаться");
                    Button.Url = Subscription.Url;
                    Buttons.Add(new List<InlineKeyboardButton> { Button });
                    IsSubscribed = false;
                }
                catch
                {

                }

                CurrentSubscribe++;
            }

            InlineKeyboardButton CheckButton = new InlineKeyboardButton("Проверить");
            InlineKeyboardButton RemoveADButton = new InlineKeyboardButton("Убрать рекламу");
            CheckButton.CallbackData = "checksub";
            RemoveADButton.CallbackData = "removead";

            Buttons.Add(new List<InlineKeyboardButton> { RemoveADButton });
            Buttons.Add(new List<InlineKeyboardButton> { CheckButton });

            InlineKeyboardMarkup Markup = new InlineKeyboardMarkup(Buttons);
            if (!IsSubscribed)
                await Bot.Client.SendTextMessageAsync(ChatId, Messages.NotSubscribed, replyMarkup: Markup);

            return IsSubscribed;
        }

        protected async Task SendMessageText(string MessageText) => await Bot.Client.SendTextMessageAsync(ChatId, MessageText);
    }
}
