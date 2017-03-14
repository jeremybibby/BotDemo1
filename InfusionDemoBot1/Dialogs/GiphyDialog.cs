using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace InfusionDemoBot1.Dialogs
{
    [Serializable]
    public class GiphyDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(HandleDialog);
        }

        private async Task HandleDialog(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var inboundMessage = await argument;
            var outboundMessage = context.MakeMessage();

            var searchString = string.Concat(inboundMessage.Text.Split(' ').Select(d => d + "+"));
            if (searchString.Length > 0)
            {
                // strip the last + from the string
                searchString = searchString.Substring(0, searchString.Length - 1);

                // fire up a web client and get the search results
                var client = new HttpClient() { BaseAddress = new Uri("http://api.giphy.com") };
                var result = await client.GetStringAsync("/v1/gifs/search?q=" + searchString + "&api_key=dc6zaTOxFJmzC");

                // parse and pull out the relevant data
                var data = ((dynamic)JObject.Parse(result)).data;
                var gif = data[(new Random()).Next(data.Count)];
                var gifUrl = gif.images.fixed_height.url.Value;
                var slug = gif.slug.Value;

                outboundMessage.Attachments = new List<Attachment>();
                outboundMessage.Attachments.Add(new Attachment()
                {
                    ContentUrl = gifUrl,
                    ContentType = "image/gif",
                    Name = slug + ".gif"
                });

                await context.PostAsync(outboundMessage);
                context.Wait(HandleDialog);                
            }
        }
    }
}