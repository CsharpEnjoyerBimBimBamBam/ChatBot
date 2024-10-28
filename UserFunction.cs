using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramChatBot
{
    public class UserFunction
    {
        public UserFunction(long ChatId, ChatBot Bot)
        {
            _ChatId = ChatId;
            _Bot = Bot;
            _User = _Database.Users[ChatId];
        }

        private long _ChatId;
        private ChatBot _Bot;
        private User _User;
        private Database _Database = Database.Load();
        private bool _IsAgeEntered = false;

        public async Task Register(Message _Message)
        {
            InlineKeyboardButton MaleButton = new InlineKeyboardButton("Мужской");
            MaleButton.CallbackData = Sex.Male.ToString();
            InlineKeyboardButton FemaleButton = new InlineKeyboardButton("Женский");
            FemaleButton.CallbackData = Sex.Female.ToString();
            InlineKeyboardMarkup Markup = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
            {
                new InlineKeyboardButton[] { MaleButton },
                new InlineKeyboardButton[] { FemaleButton }
            });

            if (_IsAgeEntered && _User.Sex != null)
            {
                await _Bot.SetUpdate(new Update() { Message = _Message });
                return;
            }

            if (_IsAgeEntered && _User.Sex == null)
            {
                await _Bot.Client.SendTextMessageAsync(_ChatId, Messages.IndicateGender, replyMarkup: Markup);
                _Bot.RegisterUserNextStep(_ChatId, Register);
                return;
            }

            if (string.IsNullOrEmpty(_Message.Text) || !VerifyUserAge(_Message.Text, out int Age))
            {
                await _Bot.Client.SendTextMessageAsync(_ChatId, Messages.EnterCorrectAge);
                _Bot.RegisterUserNextStep(_ChatId, Register);
                return;
            }
            _User.Age = Age;
            _IsAgeEntered = true;
            await _Bot.Client.SendTextMessageAsync(_ChatId, Messages.IndicateGender, replyMarkup: Markup);
            _Bot.RegisterUserNextStep(_ChatId, Register);
        }

        public async Task StopDialog()
        {
            Database Database = Database.Load();
            UserInSearch? _UserInSearch = Database.UsersInSearch.Find(User => User.User == _User);
            if (_UserInSearch != null)
            {
                Database.UsersInSearch.Remove(_UserInSearch);
                await _Bot.Client.SendTextMessageAsync(_ChatId, Messages.SearchStop);
                return;
            }

            if (!_User.InDialogue)
            {
                await _Bot.Client.SendTextMessageAsync(_ChatId, Messages.NotInDialogue);
                return;
            }

            User Companion = _User.Companion;

            Companion.MessagesIDs.Clear();
            Companion.CompanionId = null;

            _User.MessagesIDs.Clear();
            _User.CompanionId = null;

            InlineKeyboardButton PositiveRateButton = new InlineKeyboardButton("👍");
            InlineKeyboardButton NegativeRateButton = new InlineKeyboardButton("👎");
            InlineKeyboardButton ComplaintButton = new InlineKeyboardButton("Отправить жалобу");

            PositiveRateButton.CallbackData = $"positive_{Companion.ChatId}";
            NegativeRateButton.CallbackData = $"negative_{Companion.ChatId}";
            ComplaintButton.CallbackData = $"complaint_{Companion.ChatId}";

            InlineKeyboardButton[][] Buttons = new InlineKeyboardButton[][]
            {
                new InlineKeyboardButton[] { PositiveRateButton, NegativeRateButton },
                new InlineKeyboardButton[] { ComplaintButton }
            };

            InlineKeyboardMarkup Markup = new InlineKeyboardMarkup(Buttons);

            Message UserStopMessage = await _Bot.Client.SendTextMessageAsync(_ChatId, Messages.UserStopDialog, replyMarkup: Markup);
            _Database.AddComplaintToHistory(UserStopMessage.MessageId, Companion.MessageHistory);

            PositiveRateButton.CallbackData = $"positive_{_User.ChatId}";
            NegativeRateButton.CallbackData = $"negative_{_User.ChatId}";
            ComplaintButton.CallbackData = $"complaint_{_User.ChatId}";

            if (Companion.Status == ChatMemberStatus.Member)
            {
                Message CompanionStopMessage = await _Bot.Client.SendTextMessageAsync(Companion.ChatId, Messages.CompanionStopDialog, replyMarkup: Markup);
                _Database.AddComplaintToHistory(CompanionStopMessage.MessageId, _User.MessageHistory);
            }

            Companion.ClearMessageHistory();
            _User.ClearMessageHistory();
        }

        public async Task FindDialog(Sex? SearchSex = null)
        {
            UserInSearch? _UserInSearch = _Database.UsersInSearch.Find(UserInSearch => UserInSearch.User == _User);
            if (_UserInSearch != null)
            {
                await _Bot.Client.SendTextMessageAsync(_User.ChatId, Messages.AlreadyInSearch, replyMarkup: UserReplyKeyboard.Keyboard);
                return;
            }

            if (_User.InDialogue)
                await StopDialog();

            _Database.UsersInSearch.Add(new UserInSearch(_User.ChatId, SearchSex));
            await _Bot.Client.SendTextMessageAsync(_User.ChatId, Messages.SearchStart, replyMarkup: UserReplyKeyboard.Keyboard);
        }

        private bool VerifyUserAge(string Age, out int ParsedAge)
        {
            if (!int.TryParse(Age, out ParsedAge))
                return false;
            if (ParsedAge < 10 || ParsedAge > 99)
                return false;
            return true;
        }
    }
}
