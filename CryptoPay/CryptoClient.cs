using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Globalization;
using System.Net.Http.Headers;
using Telegram.Bot.Requests.Abstractions;

namespace TelegramChatBot.CryptoPay
{
    public class CryptoClient
    {
        private CryptoClient(string _ApiToken, bool UseTestApi = false)
        {
            ApiToken = _ApiToken;
            BaseUrl = "https://pay.crypt.bot/api/";
            if (UseTestApi)
                BaseUrl = "https://testnet-pay.crypt.bot/api/";

            _Client = new HttpClient();
            _Client.DefaultRequestHeaders.Add("Crypto-Pay-API-Token", ApiToken);
        }

        public string BaseUrl { get; private set; }
        public string ApiToken { get; private set; }
        private HttpClient _Client = new HttpClient();
        private static CryptoClient? _Instance;

        public static CryptoClient Initialize(string _ApiToken, bool UseTestApi = false)
        {
            if (_Instance != null)
                throw new Exception("Instance already initialized");

            _Instance = new CryptoClient(_ApiToken, UseTestApi);
            return _Instance;
        }

        public static CryptoClient GetInstance()
        {
            if (_Instance == null)
                throw new Exception("Instance not initialized");

            return _Instance;
        }

        public async Task<Invoice> CreateInvoice(CryptoCurrencyAsset Asset, double Amount, string PayLoad = "")
        {
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { "asset", Asset.ToString() },
                { "amount", Amount.ToString(CultureInfo.InvariantCulture) },
                { "payload", PayLoad.ToString() }
            };

            Uri _Uri = GetRequestUri("createInvoice", Parameters);
            return (await GetResponse<CreateInvoiceResponse>(_Uri)).Result;
        }

        public async Task<Invoice> CreateInvoice(FiatCurrencyAsset Asset, double Amount, string PayLoad = "")
        {
            Dictionary<string, string> Parameters = new Dictionary<string, string>
            {
                { "currency_type", CurrencyType.Fiat.ToSerializedString() },
                { "fiat", Asset.ToString() },
                { "amount", Amount.ToString(CultureInfo.InvariantCulture) },
                { "payload", PayLoad.ToString() }
            };

            Uri _Uri = GetRequestUri("createInvoice", Parameters);
            return (await GetResponse<CreateInvoiceResponse>(_Uri)).Result;
        }

        public async Task<RemoveInvoiceResponse> RemoveInvoice(long InvoiceId)
        {
            Uri _Uri = GetRequestUri("deleteInvoice", new Dictionary<string, string> { { "invoice_id", InvoiceId.ToString() } });
            return await GetResponse<RemoveInvoiceResponse>(_Uri);
        }

        public async Task<InvoiceCollection> GetInvoices(InvoiceSearchStatus? Status = null)
        {
            Dictionary<string, string>? Parameters = null;

            if (Status != null)
            {
                Parameters = new Dictionary<string, string>
                {
                    { "status", Status.ToSerializedString() }
                };
            }

            return await GetResponse<InvoiceCollection>(GetRequestUri("getInvoices", Parameters));
        }

        public async Task<GetExchangeRatesResponse> GetExchangeRates() => await GetResponse<GetExchangeRatesResponse>(GetRequestUri("getExchangeRates"));

        private async Task<T> GetResponse<T> (Uri _Uri) where T : CryptoBotResponse
        {
            HttpResponseMessage Response = await _Client.GetAsync(_Uri);
            string StringResponse = await Response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(StringResponse);
        }

        private Uri GetRequestUri(string Method, Dictionary<string, string>? Parameters = null)
        {
            UriBuilder Builder = new UriBuilder(BaseUrl + Method);

            if (Parameters == null)
                return Builder.Uri;

            foreach (var Parameter in Parameters)
            {
                Builder.Query += $"{Parameter.Key}={Parameter.Value}&";
            }

            return Builder.Uri;
        }
    }
}
