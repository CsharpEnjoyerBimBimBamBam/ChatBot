
namespace TelegramChatBot
{
    [Serializable]
    public class MandatorySubscription
    {
        public string Username = "";
        public string Url = "";
        public long ChatId;
        public int SubscribesCount;
        [NonSerialized] public bool IsSubscriptionsCalculating;
    }
}
