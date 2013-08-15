using System;
using System.Linq;

namespace WebChat.AspNetWebApi.Models
{
    public class FileModel
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public int UserId { get; set; }

        public bool IsProfilePic { get; set; }

        public int RecieverId { get; set; }
    }
}