using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramChatBot.CommandHandlers
{
    public class OnlineCommandHandler : CommandHadler
    {
        public OnlineCommandHandler() => Commands = new List<HandlerData> { HandlerData.FromString("/online") };

        public override Task<bool> ValidateUser(Update _Update) => CheckIfUserIsAdmin();

        protected override async Task Execute(Message _Message) => await SendMessageText(CreateOnlineMessage());

        private string CreateOnlineMessage()
        {
            int FemaleInSearchCount = 0, 
                FemaleInDialogCount = 0, 
                UsersInDialogCount = 0, 
                FemaleCount = 0,
                BannedUsersCount = 0;

            _Database.UsersInSearch.ForEach(UserInSearch =>
            {
                if (UserInSearch.User.Sex == Sex.Female)
                    FemaleInSearchCount++;
            });

            foreach (KeyValuePair<long, User> Element in _Database.Users)
            {
                User _CurrentUser = Element.Value;
                if (_CurrentUser.Sex == Sex.Female)
                    FemaleCount++;
                if (_CurrentUser.InDialogue)
                    UsersInDialogCount++;
                if (_CurrentUser.InDialogue && _CurrentUser.Sex == Sex.Female)
                    FemaleInDialogCount++;
                if (_CurrentUser.Status == ChatMemberStatus.Kicked)
                    BannedUsersCount++;
            }

            int UsersCount = _Database.Users.Count;

            return $"Пользователей в поиске - {_Database.UsersInSearch.Count}\n" +
                   $"Из них {FemaleInSearchCount} женщин и {_Database.UsersInSearch.Count - FemaleInSearchCount} мужчин\n\n" +
                   $"Пользователей в диалоге - {UsersInDialogCount}\n" +
                   $"Из них {FemaleInDialogCount} женщин и {UsersInDialogCount - FemaleInDialogCount} мужчин\n\n" +
                   $"Пользователей с момента запуска - {StartCommandHandler.NewUsersCount}\n" +
                   $"Пользователей с рефералов - {UserMessageHandler.UserFromReferalsCount}\n" +
                   $"Всего пользователей - {_Database.UsersInSearch.Count + UsersInDialogCount}\n\n" +
                   $"Всего пользователей в БД - {UsersCount}\n" +
                   $"Из них {FemaleCount} женщин и {UsersCount - FemaleCount} мужчин\n" +
                   $"Заблокировали бота - {BannedUsersCount}, активных - {UsersCount - BannedUsersCount}\n\n" +
                   $"Заблокированных пользователей - {_Database.BannedUsers.Count}";
        }
    }
}
