using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace TelegramChatBot
{
    public class Database
    {
        private Database()
        {
            MandatorySubscriptions = new List<MandatorySubscription>();
        }

        [NonSerialized] public static string FullPath = "users.json";
        [NonSerialized] public int MaxComplaintHistoryLength = 100;
        [NonSerialized] public static TimeSpan UpdateTime = TimeSpan.FromSeconds(10);
        [JsonIgnore] public List<ComplaintHistory> ComplaintMessagesHistory => new List<ComplaintHistory>(_ComplaintMessagesHistory);
        [JsonProperty("users_in_search")] public List<UserInSearch> UsersInSearch { get; private set; } = new List<UserInSearch>();
        [JsonProperty("banned_users")] public List<long> BannedUsers { get; private set; } = new List<long>();
        [JsonProperty("users")] public Dictionary<long, User> Users { get; private set; } = new Dictionary<long, User>();
        [JsonProperty("mandatory_subscriptions")] public List<MandatorySubscription> MandatorySubscriptions { get; private set; }
        private List<ComplaintHistory> _ComplaintMessagesHistory = new List<ComplaintHistory>();
        private static Database? _Instance = null;

        public static Database Load()
        {
            if (_Instance != null)
                return _Instance;
            if (!File.Exists(FullPath))
                throw new FileNotFoundException(FullPath);
            string Json = File.ReadAllText(FullPath);
            Database? DB = JsonConvert.DeserializeObject<Database>(Json, new JsonSerializerSettings
            {
                ObjectCreationHandling = ObjectCreationHandling.Replace
            });
            if (DB == null)
                throw new Exception("Database was not in a correct format");
            _Instance = DB;
            return _Instance;
        }

        public void AddComplaintToHistory(int ComplaintButtonId, IEnumerable<string> MessageHistory)
        {
            ComplaintHistory? History = _ComplaintMessagesHistory.Find(History => History.Messageid == ComplaintButtonId);
            if (History == null)
                _ComplaintMessagesHistory.Add(new ComplaintHistory(ComplaintButtonId, new List<string>(MessageHistory)));
            History = new ComplaintHistory(ComplaintButtonId, new List<string>(MessageHistory));
            if (_ComplaintMessagesHistory.Count > MaxComplaintHistoryLength)
            {
                _ComplaintMessagesHistory.RemoveAt(0);
            }
        }

        public void RemoveComplaintFromHistory(int ComplaintButtonId)
        {
            ComplaintHistory? History = _ComplaintMessagesHistory.Find(History => History.Messageid == ComplaintButtonId);
            if (History == null)
                return;
            _ComplaintMessagesHistory.Remove(History);
        }

        public static void Save()
        {
            if (_Instance == null)
                return;
            string Json = JsonConvert.SerializeObject(_Instance);
            File.WriteAllText(FullPath, Json);
        }

        public async void StartUpdate()
        {
            while (true)
            {
                await Task.Delay(UpdateTime);
                Save();
            }
        }
    }
}
