using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication14.Controllers.Methods;
namespace WebApplication14.Controllers
{
    public class BioController : ApiController
    {
        [Route("api/Bio/update_bio")]
        [HttpGet]
        public  Status update_bio(String userid,String bio)
        {
            return BiographyMethod.update_user_bio(userid, bio);
        }
    }
}
