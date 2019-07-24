using Json.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication14.Models
{
    public class UserSearch
    {

        private int user_id;
        private String user_first_name;
        private String user_last_name;
        private String user_profile_photo;
        private String user_city;
        private String user_country;
        private bool is_accepted;
        private int request_id;
        private int sender_id;
        private float distance;




        public UserSearch()
        {

        }

        public UserSearch(int user_id, string user_first_name, string user_last_name,
            string user_profile_photo, string user_city, string user_country, bool is_accepted,
            int request_id, int sender_id, float distance)
        {
            this.user_id = user_id;
            this.user_first_name = user_first_name;
            this.user_last_name = user_last_name;
            this.user_profile_photo = user_profile_photo;
            this.user_city = user_city;
            this.user_country = user_country;
            this.is_accepted = is_accepted;
            this.request_id = request_id;
            this.sender_id = sender_id;
            this.Distance = distance;
        }

        public int User_id { get => user_id; set => user_id = value; }
        public string User_first_name { get => user_first_name; set => user_first_name = value; }
        public string User_last_name { get => user_last_name; set => user_last_name = value; }
        public string User_profile_photo { get => user_profile_photo; set => user_profile_photo = value; }
        public string User_city { get => user_city; set => user_city = value; }
        public string User_country { get => user_country; set => user_country = value; }
        public bool Is_accepted { get => is_accepted; set => is_accepted = value; }
        public int Request_id { get => request_id; set => request_id = value; }
        public int Sender_id { get => sender_id; set => sender_id = value; }
        public float Distance { get => distance; set => distance = value; }

        public override string ToString()
        {
            return JsonNet.Serialize(this);
        }

    }
}