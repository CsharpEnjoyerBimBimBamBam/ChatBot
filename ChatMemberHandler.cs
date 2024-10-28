using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramChatBot
{
    public class ChatMemberHandler : UpdateHadler
    {
        private ChatMemberStatus Status;

        public override Task<bool> ValidateUser(Update _Update)
        {
            if (User == null)
                return Task.FromResult(false);
            return Task.FromResult(true);
        }

        public override void SetUpdateData(Update _Update)
        {
            ChatId = _Update.MyChatMember.Chat.Id;
            UserId = _Update.MyChatMember.From.Id;
            Status = _Update.MyChatMember.NewChatMember.Status;
        }

        public override Task Execute(Update _Update)
        {
            User.Status = Status;
            UserInSearch? _UserInSearch = _Database.UsersInSearch.Find(UserInSearch => UserInSearch.User == User);
            if (Status != ChatMemberStatus.Member && _UserInSearch != null)
                _Database.UsersInSearch.Remove(_UserInSearch);
            return Task.CompletedTask;
        }
    }
}
