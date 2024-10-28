using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramChatBot.CommandHandlers
{
    public class LinkCommandHandler : CommandHadler
    {
        public LinkCommandHandler() => Commands = new List<HandlerData> { HandlerData.FromString("/link") };

        public override async Task<bool> ValidateUser(Update _Update)
        {
            if (!await base.ValidateUser(_Update))
                return false;

            if (!await CheckIfUserInDialog())
                return false;

            return true;
        }

        protected override async Task Execute(Message _Message)
        {
            string Link = $"(tg://user?id={ChatId})";
            await Bot.Client.SendTextMessageAsync(User.Companion.ChatId, "[Собеседник отправил ссылку на свой аккаунт]" + Link, parseMode: ParseMode.MarkdownV2);
            await SendMessageText("Вы отправили ссылку на свой аккаунт");
        }
    }
}
