using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication14.Controllers.Methods;

namespace WebApplication14.Controllers
{
    public class LocationController : ApiController
    {

        [Route("api/Location/updateUserLocation")]
        [HttpGet]
        public Status updateUserLocation(int user_id, double latitude, double longitude)
        {
            return LocatioMethods.OnUpdatingUserLocation(user_id, latitude, longitude);
        }





        [Route("api/Location/getNearByUsers")]
        [HttpGet]
        public Status getNearByUsers(int user_id, float latitude, float longitude, int distance,String text_cmp, String datetime, int offset)
        {
            if (text_cmp == null)
                return LocatioMethods.OngettingNearByUsers(user_id, latitude, longitude, distance, "", datetime, offset);
            return LocatioMethods.OngettingNearByUsers(user_id, latitude, longitude, distance, text_cmp, datetime, offset);
        }


    }
}
