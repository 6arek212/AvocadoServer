using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using WebApplication14.Controllers;
using WebApplication14.Controllers.API_controllers;

namespace WebApplication14
{
    public partial class addingPost : System.Web.UI.Page
    {
        int user_id = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user_id"] != null)
            {
                user_id = int.Parse(Session["user_id"].ToString());

                List<ListItem> postTyps = new List<ListItem>();
                postTyps.Add(new ListItem("friends"));
                postTyps.Add(new ListItem("public"));
                postType.DataSource = postTyps;
                postType.DataBind();


            }
            else
            {
                Response.Redirect("login.aspx");
            }
        }

        protected void addPostBtn_Click(object sender, EventArgs e)
        {
            Post post = new Post();
            string images = "";

            if (FileUpload1.HasFile)
            {
                images = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")
                    + Path.GetExtension(FileUpload1.FileName);
                String root = HttpContext.Current.Server.MapPath("~/images/" + images);
                FileUpload1.SaveAs(root);

                post.Post_images_url = new List<string>();
                post.Post_images_url.Add(images);
            }

            post.User_id = user_id;
            post.Post_text = postText.Text.ToString();
            post.Post_type = postType.SelectedIndex;
            post.Post_date_time = DateTime.Now;
            PostController api = new PostController();
            api.addNewPost(post);


            Response.Redirect("mainFeed.aspx");
        }
    }
}