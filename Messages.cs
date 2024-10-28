
namespace TelegramChatBot
{
    public class Messages
    {
        public const string Welcome = "Привер, с помощью этого бота можно вести анонимный диалог с случайным собесдеником\n\n" +
                                      "После регистрации вы получите пробную премиум подписку на 1 час";
        public const string AfterRegistration = "Вы успешно зарегистрировались\n/next - чтобы найти собеседника";
        public const string AlreadyRegistered = "Вы уже зарегистрировались\n/next - чтобы найти собеседника";
        public const string NotRegistered = "Вы еще не зарегистрировались\n/start - чтобы начать регистрацию";
        public const string SearchStart = "Ищем собеседника";
        public const string SearchStop = "Поиск остановлен";
        public const string NotInDialogue = "Сначала нужно найти собеседника";
        public const string AlreadyInSearch = "Вы уже находитесь в поиске";
        public const string UserStopDialog = "Диалог остоновлен\n/next - чтобы найти собеседника\n\nПожалуйста оцените собеседника";
        public const string CompanionStopDialog = "Собеседник остановил диалог\n/next - чтобы найти собеседника\n\nПожалуйста оцените собеседника";
        public const string CompanionBlockBot = "Собеседник заблокировал бота";
        public const string DialogFind = "Собеседник найден, общайтесь\n/stop - чтобы остановить диалог\n/next - чтобы найти другого собеседника";
        public const string EnterCorrectAge = "Укажите корректный возраст (от 10 до 99)";
        public const string IndicateGender = "Укажите свой пол";
        public const string IndicateAge = "Укажите свой возраст";
        public const string RatingThanks = "Спасибо за оценку";
        public const string ComplaintSent = "Жалоба отправлена";
        public const string Banned = "Вы заблокированы за множество жалоб\nЧтобы снять блокировку приобретите премиум подписку\n/premium";
        public const string UserBanned = "Пользователь заблокирован";
        public const string UserUnbanned = "Пользователь разблокирован";
        public const string UserAlreadyBanned = "Пользователь уже заблокирован";
        public const string UserNotBanned = "Пользователь не заблокирован";
        public const string IndicateId = "Укажите id пользователя\n/cancel - для отмены";
        public const string InvalidId = "Некорректный id";
        public const string NotificationsEnabled = "Уведомления включены";
        public const string NotificationsDisabled = "Уведомления выключены";
        public const string NotificationsAlreadyEnabled = "Уведомления уже включены";
        public const string NotificationsAlreadyDisabled = "Уведомления уже выключены";
        public const string IndicateAd = "Отправьте сообщение для рассылки\n/cancel - для отмены";
        public const string AdAlreadySending = "Рассылка уже идет, если хотите начать еще одну, отправьте сообщение для рассылки\n/cancel - для отмены";
        public const string AdCanceled = "Рассылка отменена";
        public const string AdStart = "Рассылка начата";
        public const string NotSubscribed = "Вы еще не подписались на наши каналы";
        public const string Subscribed = "Вы подписались на все каналы\n/next - чтобы найти собеседника";
        public const string ActiveMS = "Активные обязательные подписки";
        public const string NoActiveMS = "Активных обязательных подписок нет";
        public const string MSAdded = "Обязательная подписка добавлена";
        public const string MSNotFound = "Обязательная подписка уже удалена или не добавлялась";
        public const string MSRemoved = "Обязательная подписка удалена";
        public const string EnterUsername = "Введите короткое имя (в формате @username)\n/cancel - для отмены";
        public const string EnterCorrectUsername = "Введите корректное короткое имя";
        public const string UsernameNotFound = "Канал с таким имеменем не найден";
        public const string SubscribesAlreadyCalculating = "Количество подписок уже подсчитывается";
        public const string PremiumRequired = "Для поиска по полу необходима премиум подписка\n/premium - чтобы узнать больше";
        public const string PremiumNotAvailable = "Покупка премиум подписки в данный момент недоступна";
        public const string ReferalsNotAvailable = "Реферальная система недостуна в данный момент";
        public const string ReferalRegistered = "Пользователь зарегистрировался по вашей реферальной ссылке\nВам выдан премиум на 1 час";
        public const string EnterLink = "Отправьте ссылку\n/cancel - для отмены";
        public const string InvalidLink = "Некорректная ссылка";
        public const string LinkChanged = "Ссылка изменена";
        public const string EnterIdAndDaysCount = "Отправьте id и количество дней премиума через пробел (в формате id days_count)\n/cancel - для отмены";
        public const string InvalidIdOrDaysCount = "Некорректный id или количество дней";
        public const string NoPayment = "Оплата еще не прошла";
        public const string PaymentCompleted = "Оплата успешно прошла, вы получили премиум статус";

        public static string CreateDialogFindMessage(User User, User Companion)
        {
            string Message = "Собеседник найден, общайтесь\n";

            if (User.IsPremium)
            {
                string _Age = Companion.Age?.ToString() ?? "Не указан";
                Message += $"Рейтинг: {Companion.Rating}\n" +
                           $"Пол: {Companion.Sex.Format()}\n" +
                           $"Возраст: {_Age}\n";
            }

            Message += $"/stop - чтобы остановить диалог\n" +
                       $"/next - чтобы найти другого собеседника";

            return Message;
        }
    }
}
