using Json.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication14.Models
{
    public class Like
    {
        private int user_id;
        private String user_name;
        private String time;
        private String profile_image;


        public Like()
        {

        }

        public Like(int user_id, string user_name, string time, string profile_image)
        {
            this.user_id = user_id;
            this.user_name = user_name;
            this.time = time;
            this.profile_image = profile_image;
        }

        public string User_name { get => user_name; set => user_name = value; }
        public string Time { get => time; set => time = value; }
        public string Profile_image { get => profile_image; set => profile_image = value; }
        public int User_id { get => user_id; set => user_id = value; }

        public override string ToString()
        {
            return JsonNet.Serialize(this);
        }
    }
}