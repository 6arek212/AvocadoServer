using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;
using System;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Nexmo.Api;

namespace WebApplication14.Controllers
{
    public class MessegeController : ApiController
    {


        [Route("api/Messege/a1")]
        [HttpPost]
        public void a1()
        {
            bb();
        }


        [Route("api/Messege/forgotPasswordSendEmail")]
        [HttpPost]
        public Status forgotPasswordSendEmail(String email)
        {

            Status status = new Status();


            try
            {
                MailMessage Mes = new MailMessage();
                DataTable dt = getUserPassword(email);

                if (dt.Rows.Count > 0)
                {
                    DataRow rt = dt.Rows[0];
                    String password = rt["user_passsword"].ToString();
                    String name = rt["user_first_name"].ToString();
                    Mes.To.Add(email);
                    Mes.From = new MailAddress("Avocado");
                    Mes.Subject = "Avocado: Password Recovry";
                    Mes.Body = "Hello, " + name +
                        "<br/>We recived your request to recover your Avocado account password" +
                        "<br/><br/>your password is : <b>" + password + "</b>" +
                        "<br/>feel free to contact our customer service for any problem" +
                        "<br/><br/><br/>all rights reserved to Avocado © 2019-2020 " +
                        "<br/>Tarik husin & Ahmad gbareen CEO's and founders of Avocado ";
                    Mes.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential("tarik.id.10@gmail.com", "6arek212");
                    smtp.EnableSsl = true;

                    smtp.Send(Mes);

                    status.State = 1;
                    status.Exception = "done";
                }
                else
                {
                    status.State = 0;
                    status.Exception = "error email not found";
                }
            }
            catch (Exception e)
            {
                status.State = -1;
                status.Exception = e.Message;
            }
            return status;
        }





        private DataTable getUserPassword(String email)
        {
            SqlConnection con = new SqlConnection(ConnectionInit.ConnectionString);
            DataTable dt = new DataTable();

            string query = "if exists( select user_id from users_tbl where CONVERT(VARCHAR, user_email)=@email " +
                "and user_is_active=1 ) " +
                "begin " +
                "select user_passsword,user_first_name " +
                "from users_tbl where CONVERT(VARCHAR, user_email)=@email " +
                "end ";


            SqlCommand command = new SqlCommand(query, con);
            command.Parameters.AddWithValue("@email", email);

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(dt);


            return dt;
        }





     

        private void aa()
        {
            // Find your Account Sid and Token at twilio.com/console
            // DANGER! This is insecure. See http://twil.io/secure
            const string accountSid = "AC70b087462a270826c7fc9285a219f93e";
            const string authToken = "8b1f7cc023e617384dbf6852b54a79e7";

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: "Join Earth's mightiest heroes. Like Kevin Bacon.",
                from: new Twilio.Types.PhoneNumber("+972525145565"),
                to: new Twilio.Types.PhoneNumber("+972525155174")
            );

            Console.WriteLine(message.Sid);

        }



        private void bb()
        {

            var client = new Client(creds: new Nexmo.Api.Request.Credentials
            {
                ApiKey = "03977bca",
                ApiSecret = "2aHvrD47bzyYeJ4l"
            });


            var results = client.SMS.Send(request: new SMS.SMSRequest
            {
                from = "Avocado",
                to = "9720523550593",
                text = "Hello Fashosh"
            });
            
        }



    }
}
