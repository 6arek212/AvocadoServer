using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace WebApplication14.Controllers
{
    public class UploadPhotoController : ApiController
    {
        [Route("api/UploadPhoto/PostUserImage")]
        [HttpPost]
        public async Task<HttpResponseMessage> PostUserImage(int id)
        {
            String root="";
            String name="";
            Dictionary<string, Object> dict = new Dictionary<string, Object>();
            try
            {

                var httpRequest = HttpContext.Current.Request;

                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {

                        int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB  

                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                        var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        var extension = ext.ToLower();
                        if (!AllowedFileExtensions.Contains(extension))
                        {

                            var message = string.Format("Please Upload image of type .jpg,.gif,.png.");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                       /* else if (postedFile.ContentLength > MaxContentLength)
                        {

                            var message = string.Format("Please Upload a file upto 1 mb.");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }*/
                        else
                        { 
                             root = HttpContext.Current.Server.MapPath("~/images/"+id);
                             name = postedFile.FileName;
                            if (!Directory.Exists(root))
                            {
                                Directory.CreateDirectory(root);

                                // var root2 = System.Web.Hosting.HostingEnvironment.MapPath("../pic/"+ postedFile.FileName);
                                // var filePath = HttpContext.Current.Server.MapPath("~/Userimage/" + postedFile.FileName + extension);
                            }

                            


                            root += "/"+ postedFile.FileName;

                            postedFile.SaveAs(root);

                        }
                    }

                    var message1 = string.Format("Image Updated Successfully.");
                    var message2 ="images"+id+name;
                    Status status = new Status();
                    status.State = 1;
                    status.Json_data = "http://tarik775-001-site1.itempurl.com/images/" + id + "/" + name;
                    return Request.CreateResponse(HttpStatusCode.Created, status);
                  //  return Request.CreateErrorResponse(HttpStatusCode.Created, message2); ;
                }
                var res = string.Format("Please Upload a image.");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
            catch (Exception ex)
            {
                var res = string.Format("some Message "+ex.Message);
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
        }

    }
}