using Newtonsoft.Json;

namespace TelegramChatBot
{
    [Serializable]
    public class UserInSearch
    {
        public UserInSearch(long _UserChatId, Sex? _SearchSex)
        {
            UserChatId = _UserChatId;
            SearchSex = _SearchSex;
        }

        public UserInSearch()
        {
            
        }

        [JsonProperty("search_sex")] public Sex? SearchSex { get; private set; }
        [JsonProperty("chat_id")] public long UserChatId { get; private set; }
        [JsonIgnore] public User User => Database.Load().Users[UserChatId];

        public bool Compare(UserInSearch Other)
        {
            if (Other == null)
                return false;

            return CompareSex(SearchSex, Other.User.Sex) && CompareSex(Other.SearchSex, User.Sex);
        }

        private bool CompareSex(Sex? Current, Sex? Other)
        {
            if (Current == null)
                return true;

            return Current == Other;
        }
    }
}
