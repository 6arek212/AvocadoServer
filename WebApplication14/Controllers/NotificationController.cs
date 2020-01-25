using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebApplication14.Controllers.Methods;

namespace WebApplication14.Controllers
{
    public class NotificationController : ApiController
    {

        [Route("api/Notification/getUserNotification")]
        [HttpGet]
        public Status getUserNotification(int user_id, String datetime, int offset)
        {
            return NotificationMethods.getUserNotification(user_id, datetime, offset);
        }



        [Route("api/Notification/getUserNotificationService")]
        [HttpGet]
        public Status getUserNotificationService(int user_id, String datetime, int offset)
        {
            return NotificationMethods.getUserNotificationService(user_id, datetime, offset);
        }




        [Route("api/Notification/updateToken")]
        [HttpGet]
        public Status updateToken(string token,int user_id)
        {
            return NotificationMethods.updateToken(token,user_id);
        }





        private string serverKey = "AAAALaZPb5U:APA91bGGWwQbmqWMMtG5WOkVB22pYKcJCtVqhf3y-Tm6vRs1yEsMh7vNnuH9V-wamaz3TFOZAt-ufGgPywcdE9eqUPiSgsRkscWNFUbROqjBbNBDfAg49vXyWy8NMFmvUCDe7nhQy4eb";
        private string senderId = "196063752085";


        [Route("api/Notification/SendNotification")]
        [HttpPost]
        public async Task<bool> SendNotificationAsync(string token, string title, string body)
        {
            using (var client = new HttpClient())
            {
                var firebaseOptionsServerId = serverKey;
                var firebaseOptionsSenderId = senderId;

                client.BaseAddress = new Uri("https://fcm.googleapis.com");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization",
                    $"key={firebaseOptionsServerId}");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Sender", $"id={firebaseOptionsSenderId}");


                var data = new
                {
                    to = token,
                    content_available = true,
                    notification = new
                    {
                        body = body,
                        title = title,
                    },
                    priority = "high"
                };

                var json = JsonConvert.SerializeObject(data);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

                var result = await client.PostAsync("/fcm/send", httpContent);
                return result.StatusCode.Equals(HttpStatusCode.OK);
            }
        }



    }
}
