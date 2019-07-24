using Json.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication14.Models
{
    public class UserRegister
    {
        private int user_id;
        private String first_name;
        private String last_name;
        private String email;
        private String password;
        private String register_datetime;
        private String user_birth_date;
        private String user_gender;
        private String user_country;
        private String user_city;
        private String profile_photo;

        public string First_name { get => first_name; set => first_name = value; }
        public string Last_name { get => last_name; set => last_name = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        public string Register_datetime { get => register_datetime; set => register_datetime = value; }
        public string User_birth_date { get => user_birth_date; set => user_birth_date = value; }
        public string User_gender { get => user_gender; set => user_gender = value; }
        public string User_country { get => user_country; set => user_country = value; }
        public string User_city { get => user_city; set => user_city = value; }
        public string Profile_photo { get => profile_photo; set => profile_photo = value; }
        public int User_id { get => user_id; set => user_id = value; }

        public override string ToString()
        {
            return JsonNet.Serialize(this);
        }
    }
}