using Json.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication14.Models
{
    public class Notification
    {

        private int notification_id;
        private int user_id_sent_notification;
        private String user_sent_profile_image;
        private String user_sent_name;
        private int notification_type;
        private String notification_datetime;
        private int post_id;
        private String type_txt;


        public Notification()
        {

        }

        public Notification(int notification_id, int user_id_sent_notification, 
            string user_sent_profile_image, string user_sent_name, int notification_type, 
            string notification_datetime, int post_id, string type_txt)
        {
            this.notification_id = notification_id;
            this.user_id_sent_notification = user_id_sent_notification;
            this.user_sent_profile_image = user_sent_profile_image;
            this.user_sent_name = user_sent_name;
            this.notification_type = notification_type;
            this.notification_datetime = notification_datetime;
            this.post_id = post_id;
            this.Type_txt = type_txt;
        }

        public int Notification_id { get => notification_id; set => notification_id = value; }
        public int User_id_sent_notification { get => user_id_sent_notification; set => user_id_sent_notification = value; }
        public string User_sent_profile_image { get => user_sent_profile_image; set => user_sent_profile_image = value; }
        public string User_sent_name { get => user_sent_name; set => user_sent_name = value; }
        public int Notification_type { get => notification_type; set => notification_type = value; }
        public string Notification_datetime { get => notification_datetime; set => notification_datetime = value; }
        public int Post_id { get => post_id; set => post_id = value; }
        public string Type_txt { get => type_txt; set => type_txt = value; }

        public override string ToString()
        {
            return JsonNet.Serialize(this);
        }
    }
}