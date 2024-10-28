using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramChatBot.CryptoPay;

namespace TelegramChatBot.ButtonHandlers
{
    public class CheckPaymentButtonHandler : ButtonCallDataHandler
    {
        public CheckPaymentButtonHandler() => ButtonDataVariants = new List<HandlerData> { HandlerData.FromString("checkpay") };

        public override Task<bool> ValidateUser(Update _Update) => CheckIfUserInDatabase();

        protected override async Task Execute(CallbackQuery Callback)
        {
            List<Invoice> PaidInvoices = (await CryptoClient.GetInstance().GetInvoices(InvoiceSearchStatus.Paid)).Items;
            Invoice? PaidInvoice = PaidInvoices.Find(Invoice => Invoice.InvoiceId == ButtonDataId);
            if (PaidInvoice == null)
            {
                await SendMessageText(Messages.NoPayment);
                return;
            }

            await Bot.Client.EditMessageTextAsync(ChatId, Callback.Message.MessageId, Messages.PaymentCompleted);

            double DaysCount = 30;
            double.TryParse(PaidInvoice.PayLoad, out DaysCount);

            User.AddPremiumTime(TimeSpan.FromDays(DaysCount));
        }
    }
}
