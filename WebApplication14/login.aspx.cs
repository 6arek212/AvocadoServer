using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication14.Controllers;

namespace WebApplication14
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void mLogin_Click(object sender, EventArgs e)
        {
            LoginRegisterInfoController a1 = new LoginRegisterInfoController();
            Status st = a1.loginWithEmailAndPassword(mEmail.Text.ToString(), mPassword.Text.ToString());

             if (st.State == 1)
            {
                int user_id = GetFirstInstance<int>("User_id", st.Json_data);
                Session["user_id"] = user_id;
                Response.Redirect("mainFeed.aspx");
            }


        }





        public T GetFirstInstance<T>(string propertyName, string json)
        {
            using (var stringReader = new StringReader(json))
            using (var jsonReader = new JsonTextReader(stringReader))
            {
                while (jsonReader.Read())
                {
                    if (jsonReader.TokenType == JsonToken.PropertyName
                        && (string)jsonReader.Value == propertyName)
                    {
                        jsonReader.Read();

                        var serializer = new JsonSerializer();
                        return serializer.Deserialize<T>(jsonReader);
                    }
                }
                return default(T);
            }
        }
    }
}