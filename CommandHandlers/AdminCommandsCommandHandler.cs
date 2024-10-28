using Telegram.Bot.Types;

namespace TelegramChatBot.CommandHandlers
{
    public class AdminCommandsCommandHandler : CommandHadler
    {
        public AdminCommandsCommandHandler() => Commands = new List<HandlerData> { HandlerData.FromString("/adcom") };

        private string _AdminCommands = "/adcom - показать команды для админов\n" +
                                        "/online - статистика онлайна\n" +
                                        "/user_info - информация о пользователе\n" +
                                        "/stop_send - для остановки отправки жалоб\n" +
                                        "/start_send - для начала отправки жалоб\n" +
                                        "/give_premium - выдать премиум подпсику\n" +
                                        "/remove_premium - отменить премиум подпсику\n" +
                                        "/ban - забанить пользователя\n" +
                                        "/unban - разбанить пользователя\n" +
                                        "/ad - для начала рассылки рекламы\n" +
                                        "/ms - обязательные подписки";

        public override Task<bool> ValidateUser(Update _Update) => CheckIfUserIsAdmin();

        protected override async Task Execute(Message _Message) => await SendMessageText(_AdminCommands);
    }
}
