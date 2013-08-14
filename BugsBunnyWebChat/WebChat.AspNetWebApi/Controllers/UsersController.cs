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
using WebChat.AspNetWebApi.Models;
using WebChat.Models;

namespace WebChat.AspNetWebApi.Controllers
{
    public class UsersController : ApiController
    {
        private WebChatDbEntities db = new WebChatDbEntities();

        // GET api/Users
        public IEnumerable<GetAllUsersModel> GetUsers()
        {
            var allUsers = db.Users.Select(x => new GetAllUsersModel()
            {
                Id  = x.UserId,
                Name = x.Name
            });

            return allUsers;
        }

        // GET api/Users/5
        public User GetUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return user;
        }

        // POST api/Users
        public HttpResponseMessage PostUser(CreateUserModel user)
        {
            if (!string.IsNullOrEmpty(user.UserName))
            {
                var commonChannel = db.Channels.FirstOrDefault(x => x.Name.Contains("CommonCh"));
                var newUser = new User()
                {
                    Name = user.UserName,
                };

                db.Users.Add(newUser);
                db.SaveChanges();
                newUser.Channels.Add(commonChannel);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, user);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = newUser.UserId }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Users/5
        public HttpResponseMessage DeleteUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Users.Remove(user);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, user);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}