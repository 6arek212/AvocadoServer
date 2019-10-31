using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication14.Controllers.Methods;

namespace WebApplication14.Controllers
{
    public class Update_informationController : ApiController
    {
        [Route("api/Update/update_first_lastname")]
        [HttpGet]
        public Status update_first_lastname(String userid, String firstname, String lastname)
        {
            return Update_informationMethods.update_fist_last_name(userid, firstname, lastname);
        }

        [Route("api/Update/update_emailaddress")]
        [HttpGet]
        public Status update_emailaddress(String userid, String emailaddress)
        {
            return Update_informationMethods.update_emailaddress(userid, emailaddress);
        }

        [Route("api/Update/update_phonenumber")]
        [HttpGet]
        public Status update_phonenumber(String userid, String phonenumber)
        {
            return Update_informationMethods.update_phonenumber(userid, phonenumber);
        }
    }
}
