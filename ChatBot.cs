using Microsoft.VisualBasic;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace TelegramChatBot
{
    public class ChatBot
    {
        public ChatBot(string Token)
        {
            Client = new TelegramBotClient(Token);
        }

        public TelegramBotClient Client { get; private set; }
        public long AdminChatId;
        private List<CommandHadler> _CommandHadlers = new List<CommandHadler>();
        private List<MessageHandler> _MessageHadlers = new List<MessageHandler>();
        private List<InlineButtonHandler> _ButtonHandlers = new List<InlineButtonHandler>();
        private List<ChatMemberHandler> _ChatMemberHandlers = new List<ChatMemberHandler>();
        private List<EditedMessageHandler> _EditedMessageHandlers = new List<EditedMessageHandler>();
        private List<ButtonCallDataHandler> _ButtonCallDataHandlers = new List<ButtonCallDataHandler>();
        private List<PreCheckoutQueryHandler> _PreCheckoutQueryHandlers = new List<PreCheckoutQueryHandler>();
        private Dictionary<long, Func<Message, Task>> _UsersNextStep = new Dictionary<long, Func<Message, Task>>();

        public void AddHandler(UpdateHadler Handler)
        {
            Handler.Bot = this;
            switch (Handler)
            {
                case CommandHadler _CommandHadler:
                    _CommandHadlers.Add(_CommandHadler);
                    return;
                case MessageHandler _MessageHadler:
                    _MessageHadlers.Add(_MessageHadler);
                    return;
                case InlineButtonHandler _InlineButtonHandler:
                    _ButtonHandlers.Add(_InlineButtonHandler);
                    return;
                case ChatMemberHandler _ChatMemberHandler:
                    _ChatMemberHandlers.Add(_ChatMemberHandler);
                    return;
                case EditedMessageHandler _EditedMessageHandler:
                    _EditedMessageHandlers.Add(_EditedMessageHandler);
                    return;
                case ButtonCallDataHandler _ButtonCallDataHandler:
                    _ButtonCallDataHandlers.Add(_ButtonCallDataHandler);
                    return;
                case PreCheckoutQueryHandler _PreCheckoutQueryHandler:
                    _PreCheckoutQueryHandlers.Add(_PreCheckoutQueryHandler);
                    return;
            }
        }

        public async Task StartReceiving()
        {
            FindDialogue();
            Database.Load().StartUpdate();
            while (true)
            {
                try
                {
                    await Client.ReceiveAsync((botClient, update, cancellationToken) => HandleUpdate(update),
                    async (botClient, exception, cancellationToken) => await OnPolllingError(exception));
                }
                catch
                {

                }
            }
        }

        public void RegisterUserNextStep(long ChatId, Func<Message, Task> NextStep)
        {
            if (NextStep == null)
                throw new NullReferenceException(nameof(NextStep));
            _UsersNextStep[ChatId] = NextStep;
        }

        public async Task<bool> TryCopyMessage(long ToChatId, long FromChatId, Message _Message, TimeSpan? Delay = null, int TryCount = 3)
        {
            if (Delay == null)
                Delay = TimeSpan.FromMilliseconds(100);

            for (int i = 0; i < TryCount; i++)
            {
                try
                {
                    await Client.CopyMessageAsync(ToChatId, FromChatId, _Message.MessageId, parseMode: ParseMode.MarkdownV2, replyMarkup: _Message.ReplyMarkup);
                    return true;
                }
                catch
                {
                    await Task.Delay((TimeSpan)Delay);
                }
            }
            return false;
        }

        public async Task TrySendMessage(long ChatId, string Message, int TryCount = 3, TimeSpan? Delay = null)
        {
            if (Delay == null)
                Delay = TimeSpan.FromMilliseconds(100);

            for (int i = 0; i < TryCount; i++)
            {
                try
                {
                    await Client.SendTextMessageAsync(ChatId, Message);
                    return;
                }
                catch
                {
                    await Task.Delay((TimeSpan)Delay);
                }
            }
        }

        public async Task SetUpdate(Update _Update) => await OnUpdate(_Update);

        public async Task ImitateCommand(long ChatId, string Command)
        {
            Message _CommandMessage = new Message();
            _CommandMessage.Text = Command;
            _CommandMessage.Chat = new Chat { Id = ChatId };
            _CommandMessage.From = new Telegram.Bot.Types.User { Id = ChatId };
            await SetUpdate(new Update { Message = _CommandMessage });
        }

        private async void HandleUpdate(Update _Update)
        {
            try
            {
                await OnUpdate(_Update);
            }
            catch (Exception e)
            {
                await OnPolllingError(e);
            }
        }

        private async void FindDialogue()
        {
            while (true)
            {
                try
                {
                    await FindDialogueAwaitable();
                }
                catch (Exception e)
                {
                    await OnPolllingError(e);
                }
            }
        }

        private async Task FindDialogueAwaitable()
        {
            Database _Database = Database.Load();
            List<UserInSearch> UsersInSearch = _Database.UsersInSearch;
            while (true)
            {
                await Task.Delay(100);

                if (UsersInSearch.Count < 2)
                    continue;

                List<UserInSearch> SortedUsers = UsersInSearch.OrderBy(_User => _User.User.IsPremium).ToList();
                List<UserInSearch> UserToRemoveFromSearch = new List<UserInSearch>();

                foreach (UserInSearch CurrentUserInSearch in SortedUsers)
                {
                    if (UserToRemoveFromSearch.Contains(CurrentUserInSearch))
                        continue;

                    UserInSearch? CompanionInSearch = SortedUsers.
                        Where(User => User != CurrentUserInSearch).
                        ToList().
                        Find(UserInSearch => CurrentUserInSearch.Compare(UserInSearch) && !UserToRemoveFromSearch.Contains(UserInSearch));

                    if (CompanionInSearch == null)
                        continue;

                    User User = CurrentUserInSearch.User;
                    User Companion = CompanionInSearch.User;

                    UserToRemoveFromSearch.Add(CurrentUserInSearch);
                    UserToRemoveFromSearch.Add(CompanionInSearch);

                    User.CompanionId = Companion.ChatId;
                    Companion.CompanionId = User.ChatId;

                    await TrySendMessage(User.ChatId, Messages.CreateDialogFindMessage(User, Companion), 2);
                    await TrySendMessage(Companion.ChatId, Messages.CreateDialogFindMessage(Companion, User), 2);
                }

                UserToRemoveFromSearch.ForEach(User => UsersInSearch.Remove(User));
            }
        }

        private async Task OnUpdate(Update update)
        {
            await HandleInlineButtonClick(update);
            await HandleMessage(update);
            if (update.MyChatMember != null)
                await ExecuteHandlers(_ChatMemberHandlers.ToArray(), update);

            if (update.EditedMessage != null)
                await ExecuteHandlers(_EditedMessageHandlers.ToArray(), update);

            if (update.PreCheckoutQuery != null)
                await ExecuteHandlers(_PreCheckoutQueryHandlers.ToArray(), update);
        }

        private Task OnPolllingError(Exception exception)
        {
            Console.WriteLine(exception);
            return Task.CompletedTask;
        }

        private async Task HandleMessage(Update _Update)
        {
            if (_Update.Message == null)
                return;

            Message _Message = _Update.Message;
            long ChatId = _Message.Chat.Id;
            if (_UsersNextStep.ContainsKey(ChatId))
            {
                Func<Message, Task> NextStep = _UsersNextStep[ChatId];
                _UsersNextStep.Remove(ChatId);
                await NextStep.Invoke(_Message);
                return;
            }

            MessageHandler[] MessageHandlers = _MessageHadlers.ToArray();

            string? MessageText = _Message.Text;
            if (MessageText == null)
            {
                await ExecuteHandlers(MessageHandlers, _Update);
                return;
            }

            UpdateHadler[] CommandHandlers = _CommandHadlers.FindAll(Handler =>
                Handler.Commands.Any(Command => Command.Text == MessageText || Command == HandlerData.Any)).ToArray();
            await ExecuteHandlers(CommandHandlers, _Update);
            if (CommandHandlers.Any(Handler => Handler != null))
                return;

            await ExecuteHandlers(MessageHandlers, _Update);
        }

        private async Task HandleInlineButtonClick(Update _Update)
        {
            if (_Update.CallbackQuery == null)
                return;

            CallbackQuery _CallbackQuery = _Update.CallbackQuery;
            if (_CallbackQuery.Data == null)
                return;

            string CallData = _CallbackQuery.Data;
            UpdateHadler? Handler = _ButtonHandlers.Find(Handler => Handler.CallDataVariants.Any(Data => Data.Text == CallData));
            UpdateHadler?[] AnyDataHandlers = _ButtonHandlers.FindAll(Handler => Handler.CallDataVariants.Any(Data => Data == HandlerData.Any)).ToArray();
            if (Handler != null)
            {
                await ExecuteHandler(Handler, _Update);
                return;
            }

            if (ValidateCallData(CallData, out string ButtonData))
            {
                UpdateHadler? CallDataHandler = _ButtonCallDataHandlers.Find(CallData => CallData.ButtonDataVariants.Any(Data => Data.Text == ButtonData));
                if (CallDataHandler != null)
                {
                    await ExecuteHandler(CallDataHandler, _Update);
                    return;
                }
            }

            await ExecuteHandlers(AnyDataHandlers, _Update);
        }

        private async Task ExecuteHandlers(UpdateHadler?[] Handlers, Update _Update)
        {
            foreach (UpdateHadler? Handler in Handlers)
                await ExecuteHandler(Handler, _Update);
        }

        private async Task ExecuteHandler(UpdateHadler? Handler, Update _Update)
        {
            if (Handler == null)
                return;
            UpdateHadler HandlerInstance = (UpdateHadler)Activator.CreateInstance(Handler.GetType()) ?? Handler;
            HandlerInstance.Bot = this;
            HandlerInstance.SetUpdateData(_Update);
            HandlerInstance.UpdateUserPremium();
            if (!await HandlerInstance.ValidateUser(_Update))
                return;
            await HandlerInstance.Execute(_Update);
        }

        private bool ValidateCallData(string CallData, out string ButtonData)
        {
            ButtonData = "";
            try
            {
                string[] SplitedData = CallData.Split("_");
                ButtonData = SplitedData[0];

                long.Parse(SplitedData[1]);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
