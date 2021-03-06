//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebChat.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public User()
        {
            this.Messages = new HashSet<Message>();
            this.Sessions = new HashSet<Session>();
            this.Channels = new HashSet<Channel>();
        }
    
        public int UserId { get; set; }
        public string Name { get; set; }
        public Nullable<int> SessionId { get; set; }
        public Nullable<System.DateTime> CheckInTime { get; set; }
        public Nullable<System.DateTime> CheckOutTime { get; set; }
        public string ChannelId { get; set; }
        public string ProfilePicUrl { get; set; }
        public Nullable<bool> OnlineStatus { get; set; }
    
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
        public virtual ICollection<Channel> Channels { get; set; }
    }
}
