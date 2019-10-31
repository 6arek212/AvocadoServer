using Json.Net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WebApplication14.Models
{
    public class SavedPost
    {
        private int saved_post_id;
        private int post_id;
        private String saved_datetime;
        private String description;

        public SavedPost(DataRow dr)
        {
            int.TryParse(dr["saved_post_id"].ToString(), out saved_post_id);
            int.TryParse(dr["post_id"].ToString(), out post_id);
            this.Saved_datetime = dr["saved_datetime"].ToString();
            this.Description = dr["description"].ToString(); ;
        }

        public int Saved_post_id { get => saved_post_id; set => saved_post_id = value; }
        public int Post_id { get => post_id; set => post_id = value; }
        public string Saved_datetime { get => saved_datetime; set => saved_datetime = value; }
        public string Description { get => description; set => description = value; }


        public override string ToString()
        {
            return JsonNet.Serialize(this);
        }
    }
}