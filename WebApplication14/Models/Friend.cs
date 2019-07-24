using Json.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication14.Models
{
    public class Friend
    {
        private String user_name;
        private String user_profile_image;
        private int user_id;
        private int request_id;

        public string User_name { get => user_name; set => user_name = value; }
        public string User_profile_image { get => user_profile_image; set => user_profile_image = value; }
        public int User_id { get => user_id; set => user_id = value; }
        public int Request_id { get => request_id; set => request_id = value; }


        public override string ToString()
        {
            return JsonNet.Serialize(this);
        }
    }
}