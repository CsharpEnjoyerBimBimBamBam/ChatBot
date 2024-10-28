using System.Xml.Linq;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramChatBot.CommandHandlers
{
    internal class AdvertisingMailingCommandHandler : CommandHadler
    {
        public AdvertisingMailingCommandHandler() => Commands = new List<HandlerData> { HandlerData.FromString("/ad") };

        private TimeSpan _AdSendDelay = TimeSpan.FromMilliseconds(30);
        private static bool _IsSending = false;

        public override Task<bool> ValidateUser(Update _Update) => CheckIfUserIsAdmin();

        protected override async Task Execute(Message _Message)
        {
            if (_IsSending)
                await SendMessageText(Messages.AdAlreadySending);
            else
                await SendMessageText(Messages.IndicateAd);
            Bot.RegisterUserNextStep(ChatId, ValidateAdMessage);
        }

        private async Task ValidateAdMessage(Message _Message)
        {
            if (_Message?.Text == "/cancel")
            {
                await SendMessageText(Messages.AdCanceled);
                return;
            }

            int NotKickedUsersCount = _Database.Users.Count(User => User.Value.Status != ChatMemberStatus.Kicked);
            await SendMessageText($"Рассылка начата, примерное время - {TimeSpan.FromSeconds(NotKickedUsersCount * _AdSendDelay.TotalSeconds)}");          
            StartSendAdvertising(_Message);
        }

        private async void StartSendAdvertising(Message _Message)
        {
            _IsSending = true;
            DateTime StartTime = DateTime.Now;
            List<Task<bool>> AdTasks = new List<Task<bool>>(_Database.Users.Count);
            List<User> AllUsers = _Database.Users.Values.ToList();

            foreach (User CurrentUser in AllUsers)
            {
                if (CurrentUser.Status != ChatMemberStatus.Member)
                    continue;

                AdTasks.Add(Bot.TryCopyMessage(CurrentUser.ChatId, ChatId, _Message));
                await Task.Delay(_AdSendDelay);
            }

            int SuccesfulCount = 0;

            foreach (Task<bool> AdTask in AdTasks)
            {
                if (await AdTask)
                    SuccesfulCount++;
            }

            TimeSpan AdSendTime = DateTime.Now - StartTime;
            _IsSending = false;
            await Bot.TrySendMessage(Bot.AdminChatId, $"Рассылка завершена, время рассылки - {AdSendTime}\nКол-во удачных рассылок - {SuccesfulCount}");
        }
    }
}
