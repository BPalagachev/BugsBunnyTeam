using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebChat.AspNetWebApi.Models
{
    public class GetOpenChannelsByUser
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<String> Participants { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }
    }
}