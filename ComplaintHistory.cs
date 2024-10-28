
namespace TelegramChatBot
{
    public class ComplaintHistory
    {
        public ComplaintHistory(int _Messageid, IEnumerable<string> _History)
        {
            Messageid = _Messageid;
            History = _History.ToList();
        }

        public int Messageid;
        public List<string> History = new List<string>();

        public override bool Equals(object? obj)
        {
            if (obj == null || !(obj is ComplaintHistory _ComplaintHistory))
                return false;

            return _ComplaintHistory.Messageid == Messageid;
        }

        public override int GetHashCode() => base.GetHashCode();
    }
}
