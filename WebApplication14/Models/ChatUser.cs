using Json.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication14.Models
{
    public class ChatUser
    {


        private int user_id;
        private String user_first_name;
        private String user_last_name;
        private int chat_id;
        private String profile_photo;



        public ChatUser()
        {

        }

        public ChatUser(int user_id, string user_first_name,
            string user_last_name, int chat_id, string profile_photo)
        {
            this.user_id = user_id;
            this.user_first_name = user_first_name;
            this.user_last_name = user_last_name;
            this.chat_id = chat_id;
            this.profile_photo = profile_photo;
        }

        public int User_id { get => user_id; set => user_id = value; }
        public string User_first_name { get => user_first_name; set => user_first_name = value; }
        public string User_last_name { get => user_last_name; set => user_last_name = value; }
        public int Chat_id { get => chat_id; set => chat_id = value; }
        public string Profile_photo { get => profile_photo; set => profile_photo = value; }

        public override string ToString()
        {
            return JsonNet.Serialize(this);
        }

    }
}