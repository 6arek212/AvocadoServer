using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication14.Controllers.Methods;

namespace WebApplication14.Controllers
{
    public class CommentsController : ApiController
    {
        [Route("api/Comments/gettingUpdatedComments")]
        [HttpGet]
        public Status gettingUpdatedComments(int Post_id, int Comments_incomming_count,String datetime)
        {
            return CommentsMethods.onGettingUpdatedComments(Post_id, Comments_incomming_count, datetime);
        }

        [Route("api/Comments/requestPostComments")]
        [HttpGet]
        public Status requestPostComments(int Post_id, int Offset,String datetime)
        {
            return CommentsMethods.onGettingComments(Post_id, Offset,datetime);
        }


        [Route("api/Comments/addNewCommentToPost")]
        [HttpGet]
        public Status addNewCommentToPost(int Post_id, int User_id, String Comment_text, String Comment_date_time)
        {
            return CommentsMethods.onAddingNewComment(Post_id, User_id, Comment_text, Comment_date_time);
        }


        [Route("api/Comments/deleteComment")]
        [HttpPost]
        public Status deleteComment(int comment_id,int post_id)
        {
            return CommentsMethods.OndeleteComment(comment_id, post_id);
        }

    }
}
