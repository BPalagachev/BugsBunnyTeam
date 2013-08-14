using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebChat.AspNetWebApi.Models
{
    public class CreateUserModel
    {
        public string UserName { get; set; }
    }

    public class GetAllUsersModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}