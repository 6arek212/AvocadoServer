using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication14.Controllers.Methods;

namespace WebApplication14.Controllers
{
    public class NotificationController : ApiController
    {

        [Route("api/Notification/getUserNotification")]
        [HttpGet]
        public Status getUserNotification(int user_id, String datetime, int offset)
        {
            return NotificationMethods.getUserNotification(user_id,datetime,offset);
        }



        [Route("api/Notification/getUserNotificationService")]
        [HttpGet]
        public Status getUserNotificationService(int user_id, String datetime, int offset)
        {
            return NotificationMethods.getUserNotificationService(user_id, datetime, offset);
        }

    }
}
