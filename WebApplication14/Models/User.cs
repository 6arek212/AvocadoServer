using Json.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication14.Models
{
    public class User
    {
        private String user_first_name;
        private String user_last_name;
        private String user_birth_date;
        private String user_profile_photo;
        private int user_posts_count;
        private int user_photos_count;
        private int user_connection_count;
        private int user_reputation;
        private String user_city;
        private String user_country;
        private String user_job;
        private bool user_is_private;
        private bool user_is_online;
        private int friend_request_id;
        private bool is_accepted;
        private int sender_id;
        private string user_bio;


        public User()
        {

        }


      
        public string User_bio { get => user_bio; set => user_bio = value; }
        public string User_first_name { get => user_first_name; set => user_first_name = value; }
        public string User_last_name { get => user_last_name; set => user_last_name = value; }
        public string User_birth_date { get => user_birth_date; set => user_birth_date = value; }
        public string User_profile_photo { get => user_profile_photo; set => user_profile_photo = value; }
        public int User_posts_count { get => user_posts_count; set => user_posts_count = value; }
        public int User_photos_count { get => user_photos_count; set => user_photos_count = value; }
        public int User_connection_count { get => user_connection_count; set => user_connection_count = value; }
        public int User_reputation { get => user_reputation; set => user_reputation = value; }
        public string User_city { get => user_city; set => user_city = value; }
        public string User_country { get => user_country; set => user_country = value; }
        public string User_job { get => user_job; set => user_job = value; }
        public bool User_is_private { get => user_is_private; set => user_is_private = value; }
        public bool User_is_online { get => user_is_online; set => user_is_online = value; }
        public int Friend_request_id { get => friend_request_id; set => friend_request_id = value; }
        public bool Is_accepted { get => is_accepted; set => is_accepted = value; }
        public int Sender_id { get => sender_id; set => sender_id = value; }

        public override string ToString()
        {
            return JsonNet.Serialize(this);
        }
    }

   
}