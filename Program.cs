using System.Reflection;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramChatBot.CryptoPay;

namespace TelegramChatBot
{
    class Program
    {
        private static ChatBot _ChatBot;

        public static async Task Main()
        {
            _ChatBot = new ChatBot(Environment.GetEnvironmentVariable("API_KEY"));

            await SetCommands();
            Database.FullPath = "/data/users.json";
            Database.Load();
            _ChatBot.AdminChatId = 1371573064;
            SetHandlers();
            while (true)
            {
                try
                {
                    await _ChatBot.StartReceiving();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        private static void SetHandlers()
        {
            List<Type> HandlerTypes = Assembly.GetExecutingAssembly().GetTypes().Where(Type => Type.IsSubclassOf(typeof(UpdateHadler)) && !Type.IsAbstract).ToList();
            HandlerTypes.ForEach(Type => _ChatBot.AddHandler((UpdateHadler)Activator.CreateInstance(Type)));
        }

        private static async Task SetCommands()
        {
            List<BotCommand> Commands = new List<BotCommand>
            {
                new BotCommand { Command = "/start", Description = "🚀 Начать" },
                new BotCommand { Command = "/next", Description = "🔍 Найти собеседника" },
                new BotCommand { Command = "/male", Description = "♂️ Найти парня" },
                new BotCommand { Command = "/female", Description = "♀️ Найти девушку" },
                new BotCommand { Command = "/stop", Description = "❌ Остановить диалог" },
                new BotCommand { Command = "/account", Description = "📝 Аккаунт" },
                new BotCommand { Command = "/referals", Description = "🤝 Мои рефералы" },
                new BotCommand { Command = "/premium", Description = "🏆 Премиум подписка" },
                new BotCommand { Command = "/link", Description = "🔗 Отправить ссылку на аккаунт" },
            };

            await _ChatBot.Client.SetMyCommandsAsync(Commands);
        }
    }
}
