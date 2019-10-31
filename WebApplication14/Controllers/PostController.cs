using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication14.Models;

namespace WebApplication14.Controllers.API_controllers
{
    public class PostController : ApiController
    {



        //  ------------------------ > Posts Methods <----------------------------//

        public Status GetFriendPosts(int User_id, String  datetime,int offset,int type)
        {
            return PostMethods.onRequestingPosts(User_id, datetime, offset,type);
        }



        [HttpGet]
        [Route("api/Post/GetFriendPosts2")]
        public Status GetFriendPosts2(int User_id, String datetime, int offset, int type)
        {
            return PostMethods.onRequestingPosts2(User_id, datetime, offset, type);
        }



        [HttpPost]
        [Route("api/Post/addNewPost")]
        public Status addNewPost([FromBody]Post post)
        {
            return PostMethods.onInsertingNewPost(post);
        }


        [HttpPost]
        [Route("api/Post/shareAPost")]
        public Status shareAPost([FromBody]Post post)
        {
            return PostMethods.onSharingPost(post);
        }


        [HttpPost]
        [Route("api/Post/likePost")]
        public Status likePost(int User_id, int Post_id, String Datetime)
        {
            return PostMethods.onLike(User_id, Post_id, Datetime);
        }


        [HttpPost]
        [Route("api/Post/removeLike")]
        public Status removeLike(int like_id,int post_id)
        {
            return PostMethods.onDeletingLike(like_id, post_id);
        }



        [HttpPost]
        [Route("api/Post/dislikePost")]
        public Status dislikePost(int User_id, int Post_id, String Datetime)
        {
            return PostMethods.onDisLike(User_id, Post_id, Datetime);
        }

        [HttpPost]
        [Route("api/Post/removeDisLike")]
        public Status removeDisLike(int dis_like_id,int post_id)
        {
            return PostMethods.onDeletingDisLike(dis_like_id, post_id);
        }



        [HttpGet]
        [Route("api/Post/getProfileInfo")]
        public Status getProfileInfo(int profile_user_id,int current_user_id)
        {
            return PostMethods.onGettingProfileInfo(profile_user_id,current_user_id);
        }


        [HttpGet]
        [Route("api/Post/getProfilePosts")]
        public Status getProfilePosts(int user_id, String datetime, int offset)
        {
            return PostMethods.OnGettingProfilePosts(user_id, datetime, offset);
        }




        [HttpGet]
        [Route("api/Post/getPostLikes")]
        public Status getPostLikes(int post_id,String datetime, int offset)
        {
            return PostMethods.OnGettingPostLikes(post_id,datetime, offset);
        }


        [HttpGet]
        [Route("api/Post/getPostDisLikes")]
        public Status getPostDisLikes(int post_id, String datetime, int offset)
        {
            return PostMethods.getPostDisLikes(post_id, datetime, offset);
        }





        [HttpGet]
        [Route("api/Post/getPostShares")]
        public Status getPostShares(int post_id, String datetime, int offset)
        {
            return PostMethods.getPostShares(post_id, datetime, offset);
        }



        [HttpGet]
        [Route("api/Post/getPostById")]
        public Status getPostById(int post_id,int user_id)
        {
            return PostMethods.getPostById(post_id, user_id);
        }



        [HttpGet]
        [Route("api/Post/deletePost")]
        public Status deletePost(int post_id)
        {
            return PostMethods.deletePost(post_id);
        }


        [HttpPost]
        [Route("api/Post/hidePost")]
        public Status hidePost(int post_id,int user_id,String datetime)
        {
            return PostMethods.hidePost(post_id,user_id, datetime);
        }



        [HttpPost]
        [Route("api/Post/removeHidePost")]
        public Status removeHidePost(int post_hidden_id)
        {
            return PostMethods.removeHidePost(post_hidden_id);
        }



        [HttpPost]
        [Route("api/Post/reportPost")]
        public Status reportPost(int post_id, int user_id,int report_type, String report_datetime)
        {
            return PostMethods.reportPost(post_id, user_id, report_type, report_datetime);
        }




        [HttpPost]
        [Route("api/Post/savePost")]
        public Status savePost(int post_id, int user_id, String datetime,String description)
        {
            return PostMethods.savePost(post_id, user_id, datetime, description);
        }




        [HttpGet]
        [Route("api/Post/getSavedPosts")]
        public Status getSavedPosts(int user_id, String datetime,int offset)
        {
            return PostMethods.getSavedPosts(user_id, datetime, offset);
        }



        [HttpPost]
        [Route("api/Post/removeSavedPost")]
        public Status removeSavedPost(int saved_post_id)
        {
            return PostMethods.removeSavedPost(saved_post_id);
        }

    }
}
