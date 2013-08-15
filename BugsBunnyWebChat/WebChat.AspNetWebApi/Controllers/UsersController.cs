using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using WebChat.AspNetWebApi.Models;
using WebChat.Models;

namespace WebChat.AspNetWebApi.Controllers
{
    public class UsersController : ApiController
    {
        private static readonly string DefaultAvatar = "http://static.comicvine.com/uploads/original/5/57293/1385670-bugs_bunny.jpg";
        private WebChatDbEntities db = new WebChatDbEntities();

        // GET api/Users
        public IEnumerable<GetAllUsersModel> GetUsers(int id = 0)
        {
            var allUsers = db.Users.OrderBy(x => x.Name)
                .Where(x => x.OnlineStatus.Value == true)
                .Select(x => new GetAllUsersModel()
            {
                Id = x.UserId,
                Name = x.Name,
                PictureUrl = x.ProfilePicUrl,
                OnlineStatus = x.OnlineStatus.Value
            });

            if (id != 0)
            {
                allUsers = allUsers.Where(x => x.Id == id);
            }

            return allUsers;
        }

        // GET api/Users/5
        public int GetUser(string name)
        {
            int user = db.Users.Where(x => x.Name == name)
                .Select(x => x.UserId)
                .FirstOrDefault();

            return user;
        }

        // POST api/Users
        public HttpResponseMessage PostUser(CreateUserModel user)
        {
            if (!string.IsNullOrEmpty(user.UserName))
            {
                var existingUser = db.Users.Where(x => x.Name == user.UserName).FirstOrDefault();
                if (existingUser == null)
                {
                    var commonChannel = db.Channels.FirstOrDefault(x => x.Name.Contains("CommonCh"));
                    var newUser = new User()
                    {
                        Name = user.UserName,
                        OnlineStatus = true,
                        ProfilePicUrl = DefaultAvatar
                    };

                    db.Users.Add(newUser);
                    db.SaveChanges();
                    newUser.Channels.Add(commonChannel);
                    db.SaveChanges();

                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, newUser.UserId);
                    response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = newUser.UserId }));
                    return response;
                }
                else
                {
                    existingUser.OnlineStatus = true;
                    db.SaveChanges();
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, existingUser.UserId);
                    response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = existingUser.UserId }));
                    return response;
                }

            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [HttpPost]
        public HttpResponseMessage LogoutUser(int userId)
        {
            var user = db.Users.Include("Channels")
                .First(x => x.UserId == userId);
            user.OnlineStatus = false;
            db.SaveChanges();

            var userChannels = user.Channels.ToList();

            for (var i = 0; i < userChannels.Count; i++)
            {
                if (userChannels[i].SenderId == user.UserId)
                {
                    userChannels[i].OpenForSender = false;
                }
                if (userChannels[i].RecieverId == user.UserId)
                {
                    userChannels[i].OpenForReciever = false;
                }

                if (userChannels[i].OpenForSender == false && userChannels[i].OpenForReciever == false)
                {
                    var senderId = userChannels[i].SenderId;
                    var recieverId = userChannels[i].RecieverId;

                    var sender = db.Users.FirstOrDefault(x => x.UserId == senderId);
                    var reciever = db.Users.FirstOrDefault(x => x.UserId == recieverId);

                    sender.Channels.Remove(userChannels[i]);
                    reciever.Channels.Remove(userChannels[i]);

                    db.Channels.Remove(userChannels[i]);
                }
            }
            

            db.SaveChanges();


            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}