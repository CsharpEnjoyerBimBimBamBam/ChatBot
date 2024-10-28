
namespace TelegramChatBot
{
    public struct HandlerData
    {
        public HandlerData(string Command) => Text = Command;

        public string Text { get; private set; }
        private bool _Any = false;
        public static HandlerData Any { get => new HandlerData { _Any = true }; }
        public static HandlerData FromString(string Command) => new HandlerData(Command);

        public override bool Equals(object? obj)
        {
            if (obj == null || !(obj is HandlerData _Command))
               return false;
            if (_Any)
                return true;
            if (_Command.Text == Text)
                return true;
            return false;
        }

        public override int GetHashCode() => base.GetHashCode();

        public static bool operator == (HandlerData Left, HandlerData Right) => Left.Equals(Right);

        public static bool operator != (HandlerData Left, HandlerData Right) => !(Left == Right);
    }
}
