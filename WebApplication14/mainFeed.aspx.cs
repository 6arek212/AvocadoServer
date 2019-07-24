using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication14.Controllers;
using WebApplication14.Controllers.API_controllers;

namespace WebApplication14
{
    public partial class mainFeed : System.Web.UI.Page
    {
        List<Post> posts;
        int user_id;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["user_id"] != null)
            {

                PostController api = new PostController();
                user_id = int.Parse(Session["user_id"].ToString());

                posts = JsonConvert.DeserializeObject<List<Post>>(api.GetFriendPosts2(user_id, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), 0, 0).Json_data);

                bin(posts);



            }
            else
            {
                Response.Redirect("login.aspx");
            }


        }



        private void bin(List<Post> postslist)
        {
            this.Repeater1.DataSource = postslist;
            this.Repeater1.DataBind();
        }




        protected void addPost_Click(object sender, EventArgs e)
        {
            Response.Redirect("addingPost.aspx");
        }
    }
}