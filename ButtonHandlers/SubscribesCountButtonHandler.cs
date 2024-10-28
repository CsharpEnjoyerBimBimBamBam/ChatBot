using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramChatBot.ButtonHandlers
{
    public class SubscribesCountButtonHandler : ButtonCallDataHandler
    {
        public SubscribesCountButtonHandler() => ButtonDataVariants = new List<HandlerData> { HandlerData.FromString("calcms") };

        private TimeSpan _Delay = TimeSpan.FromMilliseconds(10);

        public override Task<bool> ValidateUser(Update _Update) => CheckIfUserIsAdmin();

        protected override async Task Execute(CallbackQuery Callback)
        {
            MandatorySubscription? Subscription = _Database.MandatorySubscriptions.Find(Subscription => Subscription.ChatId == ButtonDataId);
            if (Subscription == null)
            {
                await SendMessageText(Messages.MSNotFound);
                return;
            }

            if (Subscription.IsSubscriptionsCalculating)
            {
                await SendMessageText(Messages.SubscribesAlreadyCalculating);
                return;
            }

            await SendMessageText(CreateStartCalculatingMessage(Subscription.Username));

            SendSubscribesCountMessage(Subscription);
        }

        private async void SendSubscribesCountMessage(MandatorySubscription Subscription)
        {
            try
            {
                DateTime StartTime = DateTime.Now;
                Subscription.IsSubscriptionsCalculating = true;
                int SubscribesCount = await CalculateSubscribesCount(Subscription.ChatId);
                Subscription.SubscribesCount = SubscribesCount;
                string SubscribesCountMessage = CreateSubscribesCountMessage(Subscription.Username, SubscribesCount, DateTime.Now - StartTime);
                await Bot.Client.SendTextMessageAsync(Bot.AdminChatId, SubscribesCountMessage);
            }
            catch
            {
                await Bot.Client.SendTextMessageAsync(Bot.AdminChatId, CreateSubscribesCountUnsuccessfulMessage(Subscription.Username));
            }
            finally
            {
                Subscription.IsSubscriptionsCalculating = false;
            }
        }

        private async Task<int> CalculateSubscribesCount(long ChatId)
        {
            int SubscribesCount = 0;
            ChatId _ChatId = new ChatId(ChatId);
            List<Task> ChatMemberTasks = new List<Task>(_Database.Users.Count);

            foreach (KeyValuePair<long, User> Element in _Database.Users)
            {
                User CurrentUser = Element.Value;
                ChatMemberTasks.Add(CalculateChatMember(CurrentUser.ChatId));

                await Task.Delay(_Delay);
            }

            foreach (Task MemberTask in ChatMemberTasks)
                await MemberTask;

            return SubscribesCount;

            async Task CalculateChatMember(long UserChatId)
            {
                try
                {
                    ChatMember Member = await Bot.Client.GetChatMemberAsync(_ChatId, UserChatId);
                    if (Member.Status == ChatMemberStatus.Member)
                        SubscribesCount++;
                }
                catch
                {
                    
                }
            }
        }

        private string CreateSubscribesCountMessage(string Username, int ClicksCount, TimeSpan CalculatingTime)
        {
            return $"Кол-во подписок для {Username} - {ClicksCount}\nВремя подсчета {CalculatingTime}";
        }

        private string CreateSubscribesCountUnsuccessfulMessage(string Username)
        {
            return $"Подсчет кол-ва подписок для {Username} не удался";
        }

        private string CreateStartCalculatingMessage(string Username)
        {
            return $"Подсчет кол-ва подписок для {Username} начался\n" +
                   $"Примерное время подсчета - {TimeSpan.FromSeconds(_Delay.TotalSeconds * _Database.Users.Count)}";
        }
    }
}
