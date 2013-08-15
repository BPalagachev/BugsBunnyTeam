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

        private const string OAuthTokenFileName = "D:/OAuthTokenFileName.txt";

        // POST api/messages
        [HttpPost]
        public void PostFile(FileModel file)
        {
            DropboxServiceProvider dropboxServiceProvider =
                new DropboxServiceProvider(DropboxAppKey, DropboxAppSecret, AccessLevel.AppFolder);

            // Authenticate the application (if not authenticated) and load the OAuth token
            if (!File.Exists(OAuthTokenFileName))
            {
                AuthorizeAppOAuth(dropboxServiceProvider);
            }
            OAuthToken oauthAccessToken = LoadOAuthToken();

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
            DropboxLink sharedUrl = dropbox.GetShareableLinkAsync(uploadFileEntry.Path).Result;
            Process.Start(sharedUrl.Url);
           
            //Update database
            Message newMessage = new Message()
            {
                Content = sharedUrl.Url.ToString(),
                SendTime = DateTime.Now,
                User = (from users in db.Users
                        where users.UserId == file.UserId
                        select users).FirstOrDefault(),
                UserId = file.UserId,
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

        private static OAuthToken LoadOAuthToken()
        {
            string[] lines = File.ReadAllLines(OAuthTokenFileName);
            OAuthToken oauthAccessToken = new OAuthToken(lines[0], lines[1]);
            return oauthAccessToken;
        }

        private static void AuthorizeAppOAuth(DropboxServiceProvider dropboxServiceProvider)
        {
            OAuthToken oauthToken = dropboxServiceProvider.OAuthOperations.FetchRequestTokenAsync(null, null).Result;
            OAuth1Parameters parameters = new OAuth1Parameters();
            string authenticateUrl = dropboxServiceProvider.OAuthOperations.BuildAuthorizeUrl(
                oauthToken.Value, parameters);
            Process.Start(authenticateUrl);
            Thread.Sleep(10000);

            AuthorizedRequestToken requestToken = new AuthorizedRequestToken(oauthToken, null);
            OAuthToken oauthAccessToken =
                dropboxServiceProvider.OAuthOperations.ExchangeForAccessTokenAsync(requestToken, null).Result;
            string[] oauthData = new string[]
            {
                oauthAccessToken.Value,
                oauthAccessToken.Secret
            };
            File.WriteAllLines(OAuthTokenFileName, oauthData);
        }
    }
}