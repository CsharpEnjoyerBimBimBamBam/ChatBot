using Telegram.Bot.Types;

namespace TelegramChatBot.CommandHandlers
{
    public class PremiumManagementCommandsHandler : CommandHadler
    {
        public PremiumManagementCommandsHandler() => Commands = new List<HandlerData> 
        { 
            HandlerData.FromString("/give_premium"),
            HandlerData.FromString("/remove_premium")
        };

        public override Task<bool> ValidateUser(Update _Update) => CheckIfUserIsAdmin();

        protected override async Task Execute(Message _Message)
        {
            if (MessageText == "/give_premium")
            {
                await SendMessageText(Messages.EnterIdAndDaysCount);
                Bot.RegisterUserNextStep(ChatId, GivePremium);
                return;
            }

            await SendMessageText(Messages.IndicateId);
            Bot.RegisterUserNextStep(ChatId, RemovePremium);
        }

        private async Task GivePremium(Message _Message)
        {
            if (string.IsNullOrEmpty(_Message?.Text))
            {
                await SendMessageText(Messages.InvalidIdOrDaysCount);
                Bot.RegisterUserNextStep(ChatId, GivePremium);
                return;
            }

            if (_Message.Text == "/cancel")
                return;

            if (!ValidateIdAndDaysCount(_Message.Text, out long Id, out int DaysCount))
            {
                await SendMessageText(Messages.InvalidIdOrDaysCount);
                Bot.RegisterUserNextStep(ChatId, GivePremium);
                return;
            }

            if (!_Database.Users.ContainsKey(Id))
            {
                await SendMessageText(Messages.InvalidId);
                Bot.RegisterUserNextStep(ChatId, GivePremium);
                return;
            }

            _Database.Users[Id].AddPremiumTime(TimeSpan.FromDays(DaysCount));
            await SendMessageText($"Примиум выдан пользователю с id {Id} на {DaysCount} дней");
        }

        private async Task RemovePremium(Message _Message)
        {
            if (string.IsNullOrEmpty(_Message?.Text))
            {
                await SendMessageText(Messages.InvalidId);
                Bot.RegisterUserNextStep(ChatId, GivePremium);
                return;
            }

            if (_Message.Text == "/cancel")
                return;

            if (!long.TryParse(_Message.Text, out long Id) || !_Database.Users.ContainsKey(Id))
            {
                await SendMessageText(Messages.InvalidId);
                Bot.RegisterUserNextStep(ChatId, GivePremium);
                return;
            }

            _Database.Users[Id].PremiumExpirationDate = null;
            await SendMessageText($"Премиум подписка пользователя с id {Id} отменена");
        }

        private bool ValidateIdAndDaysCount(string MessageText, out long Id, out int DaysCount)
        {
            Id = 0;
            DaysCount = 0;
            try
            {
                string[] SplitedText = MessageText.Split(" ");
                Id = long.Parse(SplitedText[0]);
                DaysCount = int.Parse(SplitedText[1]);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
