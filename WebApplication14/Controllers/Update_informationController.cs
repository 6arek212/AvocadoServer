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
        public Status update_emailaddress(String userid, String emailaddress,String password)
        {
            return Update_informationMethods.update_emailaddress(userid, emailaddress, password);
        }

        [Route("api/Update/update_phonenumber")]
        [HttpGet]
        public Status update_phonenumber(String userid, String phonenumber)
        {
            return Update_informationMethods.update_phonenumber(userid, phonenumber);
        }


        [Route("api/Update/update_password")]
        [HttpPost]
        public Status update_password(String userid, String current_password, String new_password)
        {
            return Update_informationMethods.update_password(userid, current_password, new_password);
        }


        [Route("api/Update/updateBirthDate")]
        [HttpPost]
        public Status updateBirthDate(int user_id, String birthDate)
        {
            return Update_informationMethods.updateBirthDate(user_id, birthDate);
        }


        [Route("api/Update/updateContry")]
        [HttpPost]
        public Status updateContry(int user_id, String country)
        {
            return Update_informationMethods.updateContry(user_id, country);
        }


        [Route("api/Update/updateGender")]
        [HttpPost]
        public Status updateGender(int user_id, int gender)
        {
            return Update_informationMethods.updateGender(user_id, gender);
        }


        [Route("api/Update/getUserInformation")]
        [HttpGet]
        public Status getUserInformation(int user_id)
        {
            return Update_informationMethods.getUserInformation(user_id);
        }

        
    }
}
