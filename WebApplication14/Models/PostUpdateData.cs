using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication14.Models
{
    public class PostUpdateData
    {
        String text;
        int type;
        int post_id;

        public PostUpdateData()
        {
        }

            public PostUpdateData(string text, int type, int post_id)
        {
            this.text = text;
            this.type = type;
            this.post_id = post_id;
        }

        public string Text { get => text; set => text = value; }
        public int Type { get => type; set => type = value; }
        public int Post_id { get => post_id; set => post_id = value; }
    }
}