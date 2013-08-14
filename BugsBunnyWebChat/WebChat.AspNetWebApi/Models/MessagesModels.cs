using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebChat.AspNetWebApi.Models
{
    public class GetMessageChannel
    {
        public int SenderId { get; set; }

        public int RecieverId { get; set; }
    }
}