using Newtonsoft.Json;
using Telegram.Bot.Types.Enums;

namespace TelegramChatBot
{
    [Serializable]
    public class User
    {
        public User(long _ChatId) => ChatId = _ChatId;

        public User() { }

        [JsonProperty("id")] public long ChatId;
        [JsonProperty("sex")] public Sex? Sex;
        [JsonProperty("age")] public int? Age;
        [JsonProperty("companion")] public long? CompanionId;
        [JsonProperty("rating")] public int Rating;
        [JsonProperty("status")] public ChatMemberStatus Status = ChatMemberStatus.Member;
        [JsonProperty("premium")] public DateTime? PremiumExpirationDate;
        [NonSerialized] public static int MaxMessageHistoryLength = 100;
        [JsonIgnore] public Dictionary<int, int> MessagesIDs { get; private set; } = new Dictionary<int, int>();
        [JsonIgnore] public IReadOnlyList<string> MessageHistory { get => _MessageHistory; }
        [JsonIgnore] public User? Companion
        {
            get
            {
                Database _Database = Database.Load();
                if (CompanionId == null || !_Database.Users.ContainsKey((long)CompanionId))
                    return null;
                return _Database.Users[(long)CompanionId];
            }
        }
        [JsonIgnore] public bool IsBanned => Database.Load().BannedUsers.Contains(ChatId);
        [JsonIgnore] public bool InDialogue => CompanionId != null;
        [JsonIgnore] public bool IsPremium => PremiumExpirationDate != null;
        [JsonIgnore] public TimeSpan PremiumExpirationTime => (PremiumExpirationDate - DateTime.Now) ?? TimeSpan.Zero;
        private List<string> _MessageHistory = new List<string>();

        public void AddMessageToHistory(string Message)
        {
            _MessageHistory.Add(Message);
            if (_MessageHistory.Count > MaxMessageHistoryLength)
                _MessageHistory.RemoveAt(0);
        }

        public void ClearMessageHistory() => _MessageHistory = new List<string>();

        public void AddPremiumTime(TimeSpan Time)
        {
            PremiumExpirationDate = DateTime.Now + PremiumExpirationTime + Time;
        }
    }
}
