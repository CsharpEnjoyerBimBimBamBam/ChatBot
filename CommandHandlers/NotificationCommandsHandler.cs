using Telegram.Bot.Types;

namespace TelegramChatBot.CommandHandlers
{
    public class NotificationCommandsHandler : CommandHadler
    {
        public NotificationCommandsHandler() => Commands = new List<HandlerData> { HandlerData.FromString("/stop_send"), HandlerData.FromString("/start_send") };

        public static bool SendComplaints { get; private set; } = true;

        public override Task<bool> ValidateUser(Update _Update) => CheckIfUserIsAdmin();

        protected override async Task Execute(Message _Message)
        {
            if (MessageText == "/stop_send" && !SendComplaints)
            {
                await SendMessageText(Messages.NotificationsAlreadyDisabled);
                return;
            }

            if (MessageText == "/start_send" && SendComplaints)
            {
                await SendMessageText(Messages.NotificationsAlreadyEnabled);
                return;
            }

            if (MessageText == "/start_send")
            {
                SendComplaints = true;
                await SendMessageText(Messages.NotificationsEnabled);
                return;
            }

            SendComplaints = false;
            await SendMessageText(Messages.NotificationsDisabled);
            return;
        }
    }
}
