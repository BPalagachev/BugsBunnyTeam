using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WebChat.AspNetWebApi.Models;
using WebChat.Models;

namespace WebChat.AspNetWebApi.Controllers
{
    public class ChannelsController : ApiController
    {
        private WebChatDbEntities db = new WebChatDbEntities();


        [HttpGet]
        public string OpenChannel(int SenderId, int RecieverId)
        {
            var channelName = Guid.NewGuid().ToString();

            var channel = new Channel()
            {
                Name = channelName,
                OpenForSender = true,
                OpenForReciever = false,
                RecieverId = RecieverId,
                SenderId = SenderId
            };

            var sender = db.Users.FirstOrDefault(x => x.UserId == SenderId);
            var reciever = db.Users.FirstOrDefault(x => x.UserId == RecieverId);

            if (sender == null || reciever == null)
            {
                throw new NullReferenceException();
            }

            sender.Channels.Add(channel);
            reciever.Channels.Add(channel);

            db.SaveChanges();
            return channelName;
        }

        [HttpPost]
        public IEnumerable<GetOpenChannelsByUser> ReturnOpenedChannels(int userId)
        {
            var user = db.Users.Include("Channels").FirstOrDefault(x=>x.UserId == userId);
            if (user == null)
            {
                throw new NullReferenceException();
            }

            var openChannels = user.Channels
                .Where(x => x.OpenForReciever == false && x.RecieverId == userId);

            var userChannels = openChannels
                .Select(x => new GetOpenChannelsByUser()
                {
                    Id = x.ChannelId,
                    Name = x.Name,
                    Participants = x.Users.Select(us => us.Name),
                    UserId = x.Users
                        .Where(y => y.UserId != userId)
                        .Select(y => y.UserId)
                        .FirstOrDefault(),
                    UserName = x.Users
                        .Where(y => y.UserId != userId)
                        .Select(y => y.Name)
                        .FirstOrDefault()

                }).ToList();


            foreach (var chan in openChannels)
            {
                chan.OpenForReciever = true;
            }

            db.SaveChanges();

            return userChannels;
        }

    }
}
