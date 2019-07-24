using Json.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication14.Controllers
{
    public class Post
    {

        private int post_id;
        private int user_id;
        private String user_first_name;
        private String user_last_name;
        private String user_profile_photo;
        private String post_text;
        private List<String> post_images_url;
        private DateTime post_date_time;
        private int post_type;
        private int post_comments_count;
        private int post_likes_count;
        private int post_dislike_count;
        private int post_reports_count;
        private int post_share_count;
        private bool post_is_shared;
        private int original_post_id;
        private int like_id;
        private int dis_like_id;




        public Post()
        {

        }

        public int Post_id { get => post_id; set => post_id = value; }
        public int User_id { get => user_id; set => user_id = value; }
        public string User_first_name { get => user_first_name; set => user_first_name = value; }
        public string User_last_name { get => user_last_name; set => user_last_name = value; }
        public string User_profile_photo { get => user_profile_photo; set => user_profile_photo = value; }
        public string Post_text { get => post_text; set => post_text = value; }
        public List<string> Post_images_url { get => post_images_url; set => post_images_url = value; }
        public DateTime Post_date_time { get => post_date_time; set => post_date_time = value; }
        public int Post_type { get => post_type; set => post_type = value; }
        public int Post_comments_count { get => post_comments_count; set => post_comments_count = value; }
        public int Post_likes_count { get => post_likes_count; set => post_likes_count = value; }
        public int Post_dislike_count { get => post_dislike_count; set => post_dislike_count = value; }
        public int Post_reports_count { get => post_reports_count; set => post_reports_count = value; }
        public int Post_share_count { get => post_share_count; set => post_share_count = value; }
        public bool Post_is_shared { get => post_is_shared; set => post_is_shared = value; }
        public int Original_post_id { get => original_post_id; set => original_post_id = value; }
        public int Like_id { get => like_id; set => like_id = value; }
        public int Dis_like_id { get => dis_like_id; set => dis_like_id = value; }

        public override string ToString()
        {
            return JsonNet.Serialize(this);
        }
    }
}