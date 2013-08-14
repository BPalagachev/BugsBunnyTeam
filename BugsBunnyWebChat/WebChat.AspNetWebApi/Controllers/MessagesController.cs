using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebChat.AspNetWebApi.Models;
using WebChat.Models;

namespace WebChat.AspNetWebApi.Controllers
{
    public class MessagesController : ApiController
    {
        private WebChatDbEntities db = new WebChatDbEntities();

        //// GET api/messages
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/messages/5
        [HttpPost]
        public string GetOpenChannels(GetMessageChannel value)
        {
            var channelName = Guid.NewGuid().ToString();

            var channel = new Channel()
            {
                Name = channelName,
                OpenForSender = true,
                OpenForReciever = false
            };

            var sender = db.Users.FirstOrDefault(x => x.UserId == value.SenderId);
            var reciever = db.Users.FirstOrDefault(x => x.UserId == value.RecieverId);

            if (sender == null || reciever == null)
            {
                throw new NullReferenceException();
            }

            sender.Channels.Add(channel);
            reciever.Channels.Add(channel);

            db.SaveChanges();
            return channelName;
        }

        //// POST api/messages
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/messages/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/messages/5
        //public void Delete(int id)
        //{
        //}
    }
}
