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
    public class ChannelsController : ApiController
    {
        private WebChatDbEntities db = new WebChatDbEntities();


        // GET api/channels
        //public IEnumerable<string> Get()
        //{

        //    return new string[] { "value1", "value2" };
        //}

       

        // GET api/channels/5
        public IEnumerable<GetOpenChannelsByUser> OpenChannels(int userId)
        {
            var user = db.Users.Include("Channels").FirstOrDefault();
            if (user == null)
            {
                throw new NullReferenceException();
            }

            var openChannels = user.Channels
                .Where(x => x.OpenForReciever == false);

            var userChannels = openChannels.Select(x => new GetOpenChannelsByUser()
                {
                    Id = x.ChannelId,
                    Name = x.Name,
                    Participants = x.Users.Select(us=>us.Name)
                });

            foreach (var chan in openChannels)
            {
                chan.OpenForReciever = true;
            }
            

            return userChannels;
        }

        //// POST api/channels
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/channels/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/channels/5
        //public void Delete(int id)
        //{
        //}
    }
}
