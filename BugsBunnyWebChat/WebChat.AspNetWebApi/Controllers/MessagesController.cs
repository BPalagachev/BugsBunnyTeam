using System;
using System.Linq;
using System.Web.Http;
using WebChat.AspNetWebApi.Models;
using WebChat.Models;
using System.IO;
using System.Diagnostics;
using System.Threading;
using Spring.Social.OAuth1;
using Spring.Social.Dropbox.Api;
using Spring.Social.Dropbox.Connect;
using Spring.IO;

namespace WebChat.AspNetWebApi.Controllers
{
    public class MessagesController : ApiController
    {
        private WebChatDbEntities db = new WebChatDbEntities();
        private const string DropboxAppKey = "wkbe4x03hza334g";
        private const string DropboxAppSecret = "1ucy36z7y6kga2k";

        // POST api/messages
        [HttpPost]
        public void PostFile(FileModel file)
        {
            DropboxServiceProvider dropboxServiceProvider =
                new DropboxServiceProvider(DropboxAppKey, DropboxAppSecret, AccessLevel.AppFolder);

            OAuthToken oauthAccessToken = new OAuthToken("9gyo6l0xq3l7kdd0", "ly7ayinrqbocfy8");

            // Login in Dropbox
            IDropbox dropbox = dropboxServiceProvider.GetApi(oauthAccessToken.Value, oauthAccessToken.Secret);

            // Create new folder
            string newFolderName = "New_Folder_" + DateTime.Now.Ticks;
            Entry createFolderEntry = dropbox.CreateFolderAsync(newFolderName).Result;

            // Upload a file
            IResource fileResource = new FileResource(file.Path + "/" + file.Name);
            Entry uploadFileEntry = dropbox.UploadFileAsync(fileResource,
                "/" + newFolderName + "/" + file.Name).Result;

            // Share a file
            DropboxLink sharedUrl = dropbox.GetMediaLinkAsync(uploadFileEntry.Path).Result;

           // Update database
            Message newMessage = new Message()
            {
                Content = sharedUrl.Url.ToString(),
                SendTime = DateTime.Now,
                User = (from users in db.Users
                        where users.UserId == file.UserId
                        select users).FirstOrDefault(),
                UserId = file.UserId,
                RecieverId = file.RecieverId
            };

            if (file.IsProfilePic)
            {
                User currentUser = (from users in db.Users
                                    where users.UserId == file.UserId
                                    select users).FirstOrDefault();
                currentUser.ProfilePicUrl = sharedUrl.Url.ToString();
            }

            db.Messages.Add(newMessage);
            db.SaveChanges();
        }
    }
}