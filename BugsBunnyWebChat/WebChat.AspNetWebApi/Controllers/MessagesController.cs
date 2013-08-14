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
