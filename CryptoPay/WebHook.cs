using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;

namespace TelegramChatBot.CryptoPay
{
    public class WebHook
    {
        public WebHook(string Url)
        {
            _Listener.Prefixes.Add(Url);
            _Listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
        }

        private HttpListener _Listener = new HttpListener();

        public async void StartReceiving(Func<WebHookUpdate, Task> UpdateHandler)
        {
            _Listener.Start();
            while (true)
            {
                WebHookUpdate Update = await ReceiveUpdate();
                if (Update != null) 
                    await UpdateHandler.Invoke(Update);
            }
        }

        public async Task<WebHookUpdate> ReceiveUpdate()
        {
            HttpListenerContext Context = await _Listener.GetContextAsync();
            using (StreamReader Reader = new StreamReader(Context.Request.InputStream, Context.Request.ContentEncoding))
            {
                string Request = await Reader.ReadToEndAsync();
                Console.WriteLine(Request);
                return JsonConvert.DeserializeObject<WebHookUpdate>(Request);
            }
        }
    }
}
