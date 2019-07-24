using Json.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication14.Models
{
    public class Setting
    {
        private bool user_location_switch;
        private bool account_is_private;
        private int user_id;
        private String profilePic;
        private String user_first_name;
        private String user_last_name;


        public Setting()
        {

        }

        public bool User_location_switch { get => user_location_switch; set => user_location_switch = value; }
        public bool Account_is_private { get => account_is_private; set => account_is_private = value; }
        public int User_id { get => user_id; set => user_id = value; }
        public string ProfilePic { get => profilePic; set => profilePic = value; }
        public string User_first_name { get => user_first_name; set => user_first_name = value; }
        public string User_last_name { get => user_last_name; set => user_last_name = value; }

        public override string ToString()
        {
            return JsonNet.Serialize(this);
        }
    }
}