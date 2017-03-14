using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Microsoft.Bot.Builder.Dialogs;
using InfusionDemoBot1.Dialogs;

namespace InfusionDemoBot1
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                await Conversation.SendAsync(activity, () => new GiphyDialog());
                return new HttpResponseMessage(HttpStatusCode.Accepted);
            }
            else
            {
                var response = Request.CreateResponse(HttpStatusCode.BadRequest);
                return response;

            }
        }
        
    }
}