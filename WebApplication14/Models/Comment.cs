using Json.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication14.Models
{
    public class Comment
    {
        private int comment_id;
        private int comment_user_id;
        private String comment_text;
        private DateTime comment_date_time;
        private String comment_user_name;
        private String comment_user_profile_image_path;

        public Comment()
        {

        }

        public Comment(int comment_id, int comment_user_id,
            string comment_text, DateTime comment_date_time, string comment_user_name, string comment_user_profile_image_path)
        {
            this.Comment_id = comment_id;
            this.Comment_user_id = comment_user_id;
            this.Comment_text = comment_text;
            this.Comment_date_time = comment_date_time;
            this.Comment_user_name = comment_user_name;
            this.Comment_user_profile_image_path = comment_user_profile_image_path;
        }

        public int Comment_id { get => comment_id; set => comment_id = value; }
        public int Comment_user_id { get => comment_user_id; set => comment_user_id = value; }
        public string Comment_text { get => comment_text; set => comment_text = value; }
        public DateTime Comment_date_time { get => comment_date_time; set => comment_date_time = value; }
        public string Comment_user_name { get => comment_user_name; set => comment_user_name = value; }
        public string Comment_user_profile_image_path { get => comment_user_profile_image_path; set => comment_user_profile_image_path = value; }

        public override string ToString()
        {
            return JsonNet.Serialize(this);
        }
    }
}