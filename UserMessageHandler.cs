using Microsoft.VisualBasic;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramChatBot
{
    public class UserMessageHandler : MessageHandler
    {
        public static int UserFromReferalsCount { get; private set; }

        public override async Task<bool> ValidateUser(Update _Update)
        {
            if (await CheckMessageForReferal(_Update.Message))
            {
                UserFromReferalsCount++;
                return false;
            }
            
            if (!await base.ValidateUser(_Update))
                return false;

            if (!User.InDialogue)
            {
                await SendMessageText(Messages.NotInDialogue);
                return false;
            }

            if (User.Companion.Status != ChatMemberStatus.Member)
            {
                await SendMessageText(Messages.CompanionBlockBot);
                return false;
            }

            return true;
        }

        protected override async Task Execute(Message _Message)
        {
            Message? _MessageReplyTo = _Message.ReplyToMessage;
            MessageId? SentMessageId;
            if (_MessageReplyTo != null)
            {
                if (User.MessagesIDs.ContainsKey(_MessageReplyTo.MessageId))
                {
                    int ReplyToMessageId = User.MessagesIDs[_MessageReplyTo.MessageId];
                    SentMessageId = await Bot.Client.CopyMessageAsync(User.Companion.ChatId, User.ChatId, _Message.MessageId, replyToMessageId: ReplyToMessageId);
                }
                else
                {
                    SentMessageId = await Bot.Client.CopyMessageAsync(User.Companion.ChatId, User.ChatId, _Message.MessageId);
                }
            }
            else
            {
                SentMessageId = await Bot.Client.CopyMessageAsync(User.Companion.ChatId, User.ChatId, _Message.MessageId);
            }

            if (SentMessageId != null)
            {
                User.MessagesIDs[_Message.MessageId] = SentMessageId.Id;
                User.Companion.MessagesIDs[SentMessageId.Id] = _Message.MessageId;
            }

            User.AddMessageToHistory("Подозреваемый: " + MessageText);
            User.Companion.AddMessageToHistory("Отправитель жалобы: " + MessageText);
        }

        private async Task<bool> CheckMessageForReferal(Message? _Message)
        {
            if (_Message?.Text == null)
                return false;

            if (User != null)
                return false;

            try
            {
                string[] SplitedMessage = _Message.Text.Split(" ");
                if (SplitedMessage[0] != "/start")
                    return false;
                
                long ReferalId = long.Parse(SplitedMessage[1]);
                await Bot.ImitateCommand(ChatId, "/start");
                User Referal = _Database.Users[ReferalId];
                Referal.AddPremiumTime(TimeSpan.FromHours(1));
                await Bot.Client.SendTextMessageAsync(Referal.ChatId, Messages.ReferalRegistered);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
