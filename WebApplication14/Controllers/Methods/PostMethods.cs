using Json.Net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebApplication14.Models;

namespace WebApplication14.Controllers
{
    public class PostMethods
    {
        private static int Posts_FRIENDS = 1;
        private static int PROFILE_POSTS = 2;
        private static int LIKE = 3;
        private static int USER = 4;
        private static int POST_LIKES = 5;
        private static int POST_DISLIKES = 6;
        private static int POSTS_ROWS = 20;



        //Stating values for new post 
        private static int POST_COMMENTS_COUNT = 0;
        private static int POST_LIKES_COUNT = 0;
        private static int POST_DISLIKES_COUNT = 0;
        private static int POST_REPORTS_COUNT = 0;
        private static int POST_SHARE_COUNT = 0;






        /// <summary>
        ///                     On Likeing a Post 
        ///                     A method to adding a like to a specific post
        ///                     
        ///                     Paramas:
        ///                     user_id , post_id , datetime 
        ///                     
        ///                         returns 
        ///                         Status Object as Json
        /// </summary>
        /// <param name="User_id"></param>
        /// <param name="post_id"></param>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static Status onLike(int User_id, int post_id, String datetime)
        {
            string query = "if not exists(select 1 from dis_likes_tbl where user_id=@user_id and post_id=@post_id) " +
                "begin " +
                "if not exists(select 1 from likes_tbl where user_id=@user_id and post_id=@post_id) " +
                "begin " +
                "insert into likes_tbl(user_id,post_id,like_datetime) " +
                "values(@user_id,@post_id,@datetime) " +
                "select SCOPE_IDENTITY() as 'like_id' " +
                "end " +
                "end ";


            // String query = "EXEC OnLikingPost @user_id,@post_id,@datetime ";

            SqlCommand command = new SqlCommand();
            command.Parameters.AddWithValue("@user_id", User_id);
            command.Parameters.AddWithValue("@post_id", post_id);
            command.Parameters.AddWithValue("@datetime", datetime);


            return universalObjeccctRetriver(query, command, LIKE);
        }




        /// <summary>
        ///                     On DisLikeing a Post 
        ///                     A method to add a dislike to a specific post
        ///                     
        ///                     Paramas:
        ///                     user_id , post_id , datetime 
        ///                     
        ///                         returns 
        ///                         Status Object as Json
        /// </summary>
        /// <param name="User_id"></param>
        /// <param name="post_id"></param>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static Status onDisLike(int User_id, int post_id, String datetime)
        {
            string query = "if not exists(select 1 from likes_tbl where user_id=@user_id and post_id=@post_id) " +
                "begin " +
                "if not exists(select 1 from dis_likes_tbl where user_id=@user_id and post_id=@post_id) " +
                "begin " +
                "insert into dis_likes_tbl(user_id,post_id,dis_like_datetime) " +
                "values(@user_id,@post_id,@datetime) " +
                "select SCOPE_IDENTITY() as 'like_id' " +
                "end " +
                "end ";

            SqlCommand command = new SqlCommand();
            command.Parameters.AddWithValue("@user_id", User_id);
            command.Parameters.AddWithValue("@post_id", post_id);
            command.Parameters.AddWithValue("@datetime", datetime);


            return universalObjeccctRetriver(query, command, LIKE);
        }



        public static Status onDeletingLike(int like_id, int post_id)
        {
            string query = "if exists(select 1 from likes_tbl where like_id=@like_id) " +
                "begin " +
                "delete from likes_tbl where like_id=@like_id; " +
                "update posts_tbl set post_likes_count=post_likes_count-1 where post_id=@post_id " +
                "end ";

            SqlCommand command = new SqlCommand();
            command.Parameters.AddWithValue("@like_id", like_id);
            command.Parameters.AddWithValue("@post_id", post_id);

            return insertingToDB(query, command);
        }


        public static Status onDeletingDisLike(int dis_like_id, int post_id)
        {
            string query = "if exists(select 1 from dis_likes_tbl where dis_like_id=@dis_like_id) " +
                "begin " +
                "delete from dis_likes_tbl where dis_like_id=@dis_like_id; " +
                "update posts_tbl set post_dislike_count=post_dislike_count-1 where post_id=@post_id " +
                "end ";

            SqlCommand command = new SqlCommand();
            command.Parameters.AddWithValue("@dis_like_id", dis_like_id);
            command.Parameters.AddWithValue("@post_id", post_id);

            return insertingToDB(query, command);
        }






        public static Status onGettingProfileInfo(int user_id, int current_user_id)
        {
            string query = "if(@current_user_id=@user_id) " +
                "begin " +
                "select users_tbl.user_first_name,users_tbl.user_last_name,users_tbl.user_birth_date,users_tbl.user_profile_photo " +
                ",users_tbl.user_posts_count,users_tbl.user_photos_count,users_tbl.user_connection_count,users_tbl.user_reputation " +
                ",users_tbl.user_city,users_tbl.user_country,users_tbl.user_job,users_tbl.account_is_private,users_tbl.user_is_online,user_bio " +
                "from users_tbl where user_id=@user_id " +
                "end " +
                "" +
                "else " +
                "begin " +
                "select users_tbl.user_first_name,users_tbl.user_last_name,users_tbl.user_birth_date,users_tbl.user_profile_photo " +
                ",users_tbl.user_posts_count,users_tbl.user_photos_count,users_tbl.user_connection_count,users_tbl.user_reputation " +
                ",users_tbl.user_city,users_tbl.user_country,users_tbl.user_job,users_tbl.user_is_online,users_tbl.account_is_private,friends_tbl.request_id " +
                ",friends_tbl.sender_id,friends_tbl.is_accepted,user_bio " +
                "from users_tbl " +
                "left join friends_tbl on (friends_tbl.sender_id=@current_user_id or friends_tbl.receiver_id=@current_user_id) " +
                "and (friends_tbl.sender_id=@user_id or friends_tbl.receiver_id=@user_id) " +
                "where user_id=@user_id " +
                "end ";

            SqlCommand command = new SqlCommand();
            command.Parameters.AddWithValue("@user_id", user_id);
            command.Parameters.AddWithValue("@current_user_id", current_user_id);


            return universalRetriver(query, command, USER);
        }






        /// <summary>
        /// /                               inserting new post 
        ///                                     returns
        ///                                     1-task is done 
        ///                                     0-my exception
        ///                                     -1-System exception
        /// </summary>
        /// <param name="User_id"></param>
        /// <param name="Post_text"></param>
        /// <param name="Post_image_path"></param>
        /// <param name="Post_date_time"></param>
        /// <returns></returns>

        public static Status onInsertingNewPost(Post post)
        {
            string query = "insert into posts_tbl(" +
                "user_id ," +
                "post_text," +
                "post_date_time," +
                "post_type," +
                "post_comments_count ," +
                "post_likes_count ," +
                "post_dislike_count ," +
                "post_reports_count ," +
                "post_share_count," +
                "post_is_shared)" +
                "values (@user_id, @post_text,@post_date_time,@post_type,@post_comments_count ," +
                "@post_likes_count,@post_dislike_count,@post_reports_count,@post_share_count,@post_is_shared); " +
                "" +
                "select SCOPE_IDENTITY() as 'post_id'; ";


            SqlCommand command = new SqlCommand();
            command.Parameters.AddWithValue("@user_id", post.User_id);
            command.Parameters.AddWithValue("@post_text", post.Post_text);
            command.Parameters.AddWithValue("@post_date_time", post.Post_date_time);
            command.Parameters.AddWithValue("@post_type", post.Post_type);


            command.Parameters.AddWithValue("@post_comments_count", POST_COMMENTS_COUNT);
            command.Parameters.AddWithValue("@post_likes_count", POST_LIKES_COUNT);
            command.Parameters.AddWithValue("@post_dislike_count", POST_DISLIKES_COUNT);
            command.Parameters.AddWithValue("@post_reports_count", POST_REPORTS_COUNT);
            command.Parameters.AddWithValue("@post_share_count", POST_SHARE_COUNT);
            command.Parameters.AddWithValue("@post_is_shared", post.Post_is_shared);


            Status s1 = insertingToDBGetIdentity(query, command);
            if (s1.State == 1 && post.Post_images_url != null)
            {
                int post_id = int.Parse(s1.Json_data);
                String q = "insert into images(user_id,post_id,image_url) values (@user_id,@post_id,@image_url)";

                for (int i = 0; i < post.Post_images_url.Count; i++)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Parameters.AddWithValue("@user_id", post.User_id);
                    cmd.Parameters.AddWithValue("@post_id", post_id);
                    cmd.Parameters.AddWithValue("@image_url", post.Post_images_url.ElementAt(i));

                    s1 = insertingToDB(q, cmd);
                    if (s1.State != 1)
                    {
                        return s1;
                    }
                }

                return s1;

            }
            else
            {
                return s1;
            }
        }



        public static Status OnGettingProfilePosts(int user_id, int incomingUserId, String datetime, int offset)
        {
            String query = "";
            if (user_id == incomingUserId)
            {
                query = "select posts_tbl.* ,users_tbl.user_first_name, users_tbl.user_last_name ,like_id,user_profile_photo" +
                ",dis_like_id , " +
                "(select CAST(i.image_url as nvarchar(MAX))  + ',' " +
                "from images i " +
                "where i.post_id = posts_tbl.post_id " +
                "for xml path ('') " +
                ") as image_urls " +
                "" +
                "from posts_tbl " +
                "left join likes_tbl on posts_tbl.post_id=likes_tbl.post_id and likes_tbl.user_id=@user_id " +
                "left join dis_likes_tbl on posts_tbl.post_id=dis_likes_tbl.post_id and dis_likes_tbl.user_id=@user_id " +
                "left join users_tbl on users_tbl.user_id =posts_tbl.user_id " +
                "where posts_tbl.user_id=@user_id " +
                "and posts_tbl.post_date_time<=@datetime " +
                "order by post_date_time DESC " +
                "offset (@offset) rows " +
                "fetch next (20) rows only ";
            }
            else
            {
                query = "select posts_tbl.* ,users_tbl.user_first_name, users_tbl.user_last_name ,like_id,user_profile_photo" +
                ",dis_like_id , " +
                "(select CAST(i.image_url as nvarchar(MAX))  + ',' " +
                "from images i " +
                "where i.post_id = posts_tbl.post_id " +
                "for xml path ('') " +
                ") as image_urls " +
                "" +
                "from posts_tbl " +
                "left join likes_tbl on posts_tbl.post_id=likes_tbl.post_id and likes_tbl.user_id=@user_id " +
                "left join dis_likes_tbl on posts_tbl.post_id=dis_likes_tbl.post_id and dis_likes_tbl.user_id=@user_id " +         
                "left join users_tbl on users_tbl.user_id =posts_tbl.user_id " +
                "where posts_tbl.user_id=@incominguserId " +
                "and posts_tbl.post_date_time<=@datetime " +
                "order by post_date_time DESC " +
                "offset (@offset) rows " +
                "fetch next (20) rows only ";
            }



            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@user_id", user_id);
            cmd.Parameters.AddWithValue("@datetime", datetime);
            cmd.Parameters.AddWithValue("@offset", offset);
            cmd.Parameters.AddWithValue("@incominguserId", incomingUserId);

            return universalRetriver(query, cmd, PROFILE_POSTS);
        }





        public static Status getPostById(int post_id, int user_id)
        {
            String query = "select posts_tbl.* ,users_tbl.user_first_name, users_tbl.user_last_name ,like_id,user_profile_photo" +
                 ",dis_like_id , " +
                 "(select CAST(i.image_url as nvarchar(MAX))  + ',' " +
                 "from images i " +
                 "where i.post_id = posts_tbl.post_id  " +
                 "for xml path ('') " +
                 ") as image_urls " +
                 ",saved_posts.saved_post_id " +
                 "" +
                 "from posts_tbl " +
                 "left join saved_posts on saved_posts.post_id = posts_tbl.post_id  and saved_posts.user_id=@user_id " +
                 "left join likes_tbl on posts_tbl.post_id=likes_tbl.post_id and likes_tbl.user_id=@user_id " +
                 "left join dis_likes_tbl on posts_tbl.post_id=dis_likes_tbl.post_id and likes_tbl.user_id=@user_id " +
                 "left join users_tbl on users_tbl.user_id =posts_tbl.user_id " +
                 "where posts_tbl.post_id=@post_id ";

            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@post_id", post_id);
            cmd.Parameters.AddWithValue("@user_id", user_id);


            return universalRetriver(query, cmd, PROFILE_POSTS);
        }






        public static Status getSavedPosts(int user_id, String datetime, int offset)
        {
            String query = "select saved_post_id,posts_tbl.post_id,saved_datetime,saved_posts.description " +
                "from saved_posts " +
                "left join posts_tbl on saved_posts.post_id = posts_tbl.post_id " +
               "where saved_posts.user_id=@user_id and saved_datetime <= @datetime " +
               "order by saved_datetime DESC " +
               "offset (@offset) rows " +
               "fetch next 20 rows only ";



            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@user_id", user_id);
            cmd.Parameters.AddWithValue("@datetime", datetime);
            cmd.Parameters.AddWithValue("@offset", offset);


            Status status = new Status();
            try
            {
                DataTable dt = GetDataTable(query, cmd);


                if (dt.Rows.Count > 0)
                {
                    status.Json_data = "[";
                    foreach (DataRow dr in dt.Rows)
                    {
                        status.Json_data += new SavedPost(dr).ToString() + ",";
                    }
                    status.Json_data = status.Json_data.Remove(status.Json_data.Length - 1);
                    status.Json_data += "]";
                    status.State = 1;
                }
                else
                {
                    status.State = 0;
                    status.Exception = "no saved posts";
                }
            }
            catch (Exception e)
            {
                status.State = -1;
                status.Exception = e.Message;

            }
            return status;
        }





        public static Status savePost(int post_id, int user_id, String datetime, String description)
        {
            String query = "IF not EXISTS (select saved_post_id from saved_posts where user_id=@user_id and post_id=@post_id ) " +
                "begin " +
                "insert into saved_posts(user_id,post_id,saved_datetime,description) values " +
               "(@user_id,@post_id,@datetime,@description) " +
               "select SCOPE_IDENTITY() as 'saved_post_id' " +
               "end ";


            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@post_id", post_id);
            cmd.Parameters.AddWithValue("@user_id", user_id);
            cmd.Parameters.AddWithValue("@datetime", datetime);
            cmd.Parameters.AddWithValue("@description", description);


            DataTable dt = GetDataTable(query, cmd);

            Status status = new Status();
            status.State = 1;
            status.Json_data = "{\"saved_post_id\":" + dt.Rows[0]["saved_post_id"].ToString() + "}";

            return status;
        }





        public static Status removeSavedPost(int saved_post_id)
        {
            String query = "delete from saved_posts where saved_post_id=@saved_post_id";


            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@saved_post_id", saved_post_id);

            return insertingToDB(query, cmd);
        }





        public static Status reportPost(int post_id, int user_id, int report_type, String report_datetime)
        {
            String query = "IF not EXISTS (select report_id from posts_report where user_id=@user_id and post_id=@post_id) " +
                "begin" +
                "insert into posts_report(user_id,post_id,report_type,report_datetime) values " +
                "(@user_id,@post_id,@report_type,@report_datetime) " +
                "update posts_tbl set post_reports_count=post_reports_count+1 where post_id=@post_id " +
                "end ";


            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@post_id", post_id);
            cmd.Parameters.AddWithValue("@user_id", user_id);
            cmd.Parameters.AddWithValue("@report_type", report_type);
            cmd.Parameters.AddWithValue("@report_datetime", report_datetime);


            return insertingToDB(query, cmd);
        }



        public static Status hidePost(int post_id, int user_id, String datetime)
        {
            String query = "insert into posts_hidden_tbl(post_id,user_id,post_hidden_datetime) values " +
                "(@post_id,@user_id,@datetime)";


            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@post_id", post_id);
            cmd.Parameters.AddWithValue("@user_id", user_id);
            cmd.Parameters.AddWithValue("@datetime", datetime);


            return insertingToDB(query, cmd);
        }


        public static Status removeHidePost(int post_hidden_id)
        {
            String query = "delete from  posts_hidden_tbl where post_hidden_id=@post_hidden_id";

            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@post_hidden_id", post_hidden_id);

            return insertingToDB(query, cmd);
        }





        public static Status deletePost(int post_id)
        {
            String query = "if exists(select 1 from posts_tbl where post_id=post_id) " +
                "begin " +
                "" +
                "if((select post_is_shared from posts_tbl where post_id=@post_id)=1) " +
                "begin " +
                "declare @originalPostId int=(select original_post_id from posts_tbl where post_id=@post_id) " +
                "update posts_tbl set post_share_count=post_share_count-1 where post_id=@originalPostId " +
                "end " +
                "" +
                "declare @user_id int " +
                "set @user_id=(select user_id from posts_tbl where post_id=@post_id) " +
                "delete from posts_hidden_tbl where post_id=@post_id " +
                "delete from saved_posts where post_id=@post_id " +
                "delete from likes_tbl where post_id=@post_id " +
                "delete from dis_likes_tbl where post_id=@post_id " +
                "delete from comments_tbl where post_id=@post_id " +
                "delete from notification_tbl where post_id=@post_id " +
                "delete from images where post_id=@post_id " +
                "update posts_tbl set original_post_id=null where original_post_id=@post_id " +
                "delete from posts_tbl where post_id=@post_id " +
                "" +
                "update users_tbl set user_posts_count=user_posts_count-1 where user_id=@user_id " +  
                "" +
                "end ";


            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@post_id", post_id);


            return insertingToDB(query, cmd);
        }








        public static Status OnGettingPostLikes(int post_id, String datetime, int offset)
        {
            String query = "select users_tbl.user_id,users_tbl.user_first_name,users_tbl.user_last_name, " +
                "users_tbl.user_profile_photo,likes_tbl.like_datetime " +
                "from likes_tbl " +
                "left join users_tbl on users_tbl.user_id=likes_tbl.user_id " +
                "where likes_tbl.post_id=@post_id and like_datetime<= @datetime " +
                "order by like_datetime DESC " +
                "offset (@offset) rows " +
                "fetch next (20) rows only ";

            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@post_id", post_id);
            cmd.Parameters.AddWithValue("@datetime", datetime);
            cmd.Parameters.AddWithValue("@offset", offset);

            return universalRetriver(query, cmd, POST_LIKES);
        }






        public static Status getPostDisLikes(int post_id, String datetime, int offset)
        {
            String query = "select users_tbl.user_id,users_tbl.user_first_name,users_tbl.user_last_name, " +
                "users_tbl.user_profile_photo,dis_likes_tbl.dis_like_datetime " +
                "from dis_likes_tbl " +
                "left join users_tbl on users_tbl.user_id=dis_likes_tbl.user_id " +
                "where dis_likes_tbl.post_id=@post_id and dis_like_datetime<= @datetime " +
                "order by dis_like_datetime DESC " +
                "offset (@offset) rows " +
                "fetch next (20) rows only ";

            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@post_id", post_id);
            cmd.Parameters.AddWithValue("@datetime", datetime);
            cmd.Parameters.AddWithValue("@offset", offset);


            return universalRetriver(query, cmd, POST_DISLIKES);
        }





        public static Status getPostShares(int post_id, String datetime, int offset)
        {
            String query = "select users_tbl.user_id,users_tbl.user_first_name,users_tbl.user_last_name, " +
                "users_tbl.user_profile_photo,dis_likes_tbl.like_datetime " +
                "from dis_likes_tbl " +
                "left join users_tbl on users_tbl.user_id=dis_likes_tbl.user_id " +
                "where dis_likes_tbl.post_id=@post_id and like_datetime<= @datetime " +
                "order by like_datetime DESC " +
                "offset (@offset) rows " +
                "fetch next (20) rows only ";

            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@post_id", post_id);
            cmd.Parameters.AddWithValue("@datetime", datetime);
            cmd.Parameters.AddWithValue("@offset", offset);

            return universalRetriver(query, cmd, POST_DISLIKES);
        }









        /// <summary>
        ///  public post =1
        ///  friends post =0 
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="datetime"></param>
        /// <param name="offset"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Status onRequestingPosts2(int user_id, String datetime, int offset, int type)
        {
            String query = "if(@type=0)" +
                "begin " +
                "select posts_tbl.* ,users_tbl.user_first_name, users_tbl.user_last_name ,like_id, user_profile_photo , " +
                "dis_like_id ,posts_report.report_id, " +
                "(select CAST(i.image_url as nvarchar(MAX))  + ',' " +
                "from images i " +
                "where i.post_id = posts_tbl.post_id " +
                "for xml path ('') " +
                ") as image_urls " +
                ",saved_posts.saved_post_id " +
                "" +
                "from posts_tbl " +
                "left join saved_posts on saved_posts.post_id = posts_tbl.post_id  and saved_posts.user_id=@user_id " +
                "left join likes_tbl on posts_tbl.post_id=likes_tbl.post_id and likes_tbl.user_id=@user_id " +
                "left join dis_likes_tbl on posts_tbl.post_id=dis_likes_tbl.post_id and dis_likes_tbl.user_id=@user_id " +
                "left join users_tbl on users_tbl.user_id =posts_tbl.user_id " +
                "left join posts_report on posts_report.post_id = posts_tbl.post_id and posts_report.user_id= @user_id " +
                "where posts_tbl.user_id=@user_id and (posts_tbl.post_date_time <= @datetime ) and posts_tbl.post_type=@type " +
                "" +
                "union all " +
                "" +
                "select  posts_tbl.*,users_tbl.user_first_name, users_tbl.user_last_name ,like_id, user_profile_photo , " +
                "dis_like_id , posts_report.report_id , " +
                "(select CAST(i.image_url as nvarchar(MAX))  + ',' " +
                "from images i " +
                "where i.post_id = posts_tbl.post_id " +
                "for xml path ('') " +
                ") as image_urls " +
                ",saved_posts.saved_post_id " +
                "" +
                "from posts_tbl " +
                "left join saved_posts on saved_posts.post_id = posts_tbl.post_id  and saved_posts.user_id=@user_id " +
                "left join likes_tbl on posts_tbl.post_id=likes_tbl.post_id and likes_tbl.user_id=@user_id " +
                "left join dis_likes_tbl on posts_tbl.post_id=dis_likes_tbl.post_id and dis_likes_tbl.user_id=@user_id " +
                "left join users_tbl on users_tbl.user_id =posts_tbl.user_id " +
                "left join friends_tbl on (friends_tbl.receiver_id =posts_tbl.user_id " +
                "or friends_tbl.sender_id=posts_tbl.user_id) " +
                "and (friends_tbl.sender_id=@user_id or friends_tbl.receiver_id=@user_id) " +
                "left join posts_hidden_tbl on  posts_tbl.post_id = posts_hidden_tbl.post_id " +
                "and posts_hidden_tbl.user_id =@user_id " +
                "left join posts_report on posts_report.post_id = posts_tbl.post_id and posts_report.user_id= @user_id " +
                "where  friends_tbl.is_accepted=1 and posts_tbl.user_id!=@user_id  and posts_tbl.post_type=@type " +
                "and (posts_tbl.post_date_time <= @datetime ) " +
                "and posts_hidden_tbl.user_id is null " +
                "order by posts_tbl.post_date_time DESC " +
                "offset (@offset) rows " +
                "fetch next (@POSTS_ROWS) rows only " +
                "end " +
                "" +
                "" +
                "else " +
                "begin " +
                "select posts_tbl.* ,users_tbl.user_first_name, users_tbl.user_last_name ,like_id, user_profile_photo , " +
                "dis_like_id, posts_report.report_id , " +
                "(select CAST(i.image_url as nvarchar(MAX))  + ', ' " +
                "from images i " +
                "where i.post_id = posts_tbl.post_id " +
                "for xml path ('') " +
                ") as image_urls " +
                ",saved_posts.saved_post_id " +
                "" +
                "from posts_tbl " +
                "left join saved_posts on saved_posts.post_id = posts_tbl.post_id  and saved_posts.user_id=@user_id " +
                "left join likes_tbl on posts_tbl.post_id=likes_tbl.post_id and likes_tbl.user_id=@user_id " +
                "left join dis_likes_tbl on posts_tbl.post_id=dis_likes_tbl.post_id and dis_likes_tbl.user_id=@user_id " +
                "left join users_tbl on users_tbl.user_id =posts_tbl.user_id " +
                "left join posts_report on posts_report.post_id = posts_tbl.post_id and posts_report.user_id= @user_id " +
                "where posts_tbl.user_id=@user_id and (posts_tbl.post_date_time <= @datetime ) and post_type=@type " +
                "" +
                "union all " +
                "" +
                "select  posts_tbl.*,users_tbl.user_first_name, users_tbl.user_last_name ,like_id, user_profile_photo , " +
                "dis_like_id ,posts_report.report_id , " +
                "(select CAST(i.image_url as nvarchar(MAX))  + ', ' " +
                "from images i " +
                "where i.post_id = posts_tbl.post_id " +
                "for xml path ('') " +
                ") as image_urls " +
                ",saved_posts.saved_post_id " +
                "" +
                "from posts_tbl " +
                "left join saved_posts on saved_posts.post_id = posts_tbl.post_id  and saved_posts.user_id=@user_id " +
                "left join likes_tbl on posts_tbl.post_id=likes_tbl.post_id and likes_tbl.user_id=@user_id " +
                "left join dis_likes_tbl on posts_tbl.post_id=dis_likes_tbl.post_id and dis_likes_tbl.user_id=@user_id " +
                "left join users_tbl on users_tbl.user_id =posts_tbl.user_id " +
                "left join posts_hidden_tbl on  posts_tbl.post_id = posts_hidden_tbl.post_id " +
                "and posts_hidden_tbl.user_id =@user_id " +
                "left join posts_report on posts_report.post_id = posts_tbl.post_id and posts_report.user_id= @user_id " +
                "where posts_tbl.user_id!=@user_id and post_type=@type " +
                "and (posts_tbl.post_date_time <= @datetime ) " +
                "and posts_hidden_tbl.user_id is null " +
                "order by posts_tbl.post_date_time DESC " +
                "offset (@offset) rows " +
                "fetch next (@POSTS_ROWS) rows only " +
                "end ";


            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@user_id", user_id);
            cmd.Parameters.AddWithValue("@datetime", datetime);
            cmd.Parameters.AddWithValue("@POSTS_ROWS", POSTS_ROWS);
            cmd.Parameters.AddWithValue("@offset", offset);
            cmd.Parameters.AddWithValue("@type", type);



            return universalRetriver(query, cmd, Posts_FRIENDS);
        }



















        public static Status onRequestingPosts(int user_id, String datetime, int offset, int type)
        {
            String query = "if(@type!=3)" +
                "begin " +
                "select posts_tbl.* ,users_tbl.user_first_name, users_tbl.user_last_name ,like_id,user_profile_photo, " +
                "dis_like_id " +
                "from posts_tbl " +
                "left join likes_tbl on posts_tbl.post_id=likes_tbl.post_id and likes_tbl.user_id=@user_id " +
                "left join dis_likes_tbl on posts_tbl.post_id=dis_likes_tbl.post_id and dis_likes_tbl.user_id=@user_id " +
                "left join users_tbl on users_tbl.user_id =posts_tbl.user_id " +
                "where posts_tbl.user_id=@user_id and (posts_tbl.post_date_time <= @datetime ) and posts_tbl.post_type=@type " +
                "" +
                "union all " +
                "" +
                "select  posts_tbl.*,users_tbl.user_first_name, users_tbl.user_last_name ,like_id,user_profile_photo, " +
                "dis_like_id " +
                "from posts_tbl " +
                "left join likes_tbl on posts_tbl.post_id=likes_tbl.post_id and likes_tbl.user_id=@user_id " +
                "left join dis_likes_tbl on posts_tbl.post_id=dis_likes_tbl.post_id and dis_likes_tbl.user_id=@user_id " +
                "left join users_tbl on users_tbl.user_id =posts_tbl.user_id " +
                "left join friends_tbl on (friends_tbl.receiver_id =posts_tbl.user_id " +
                "or friends_tbl.sender_id=posts_tbl.user_id) " +
                "and (friends_tbl.sender_id=@user_id or friends_tbl.receiver_id=@user_id) " +
                "left join posts_hidden_tbl on  posts_tbl.post_id = posts_hidden_tbl.post_id " +
                "and posts_hidden_tbl.user_id =@user_id " +
                "where  friends_tbl.is_accepted=1 and posts_tbl.user_id!=@user_id  and posts_tbl.post_type=@type " +
                "and (posts_tbl.post_date_time <= @datetime ) " +
                "and posts_hidden_tbl.user_id is null " +
                "order by posts_tbl.post_date_time DESC " +
                "offset (@offset) rows " +
                "fetch next (@POSTS_ROWS) rows only " +
                "end " +
                "" +
                "" +
                "else " +
                "begin " +
                "select posts_tbl.* ,users_tbl.user_first_name, users_tbl.user_last_name ,like_id,user_profile_photo, " +
                "dis_like_id " +
                "from posts_tbl " +
                "left join likes_tbl on posts_tbl.post_id=likes_tbl.post_id and likes_tbl.user_id=@user_id " +
                "left join dis_likes_tbl on posts_tbl.post_id=dis_likes_tbl.post_id and dis_likes_tbl.user_id=@user_id " +
                "left join users_tbl on users_tbl.user_id =posts_tbl.user_id " +
                "where posts_tbl.user_id=@user_id and (posts_tbl.post_date_time <= @datetime ) " +
                "" +
                "union all " +
                "" +
                "select  posts_tbl.*,users_tbl.user_first_name, users_tbl.user_last_name ,like_id,user_profile_photo, " +
                "dis_like_id " +
                "from posts_tbl " +
                "left join likes_tbl on posts_tbl.post_id=likes_tbl.post_id and likes_tbl.user_id=@user_id " +
                "left join dis_likes_tbl on posts_tbl.post_id=dis_likes_tbl.post_id and dis_likes_tbl.user_id=@user_id " +
                "left join users_tbl on users_tbl.user_id =posts_tbl.user_id " +
                "left join friends_tbl on (friends_tbl.receiver_id =posts_tbl.user_id " +
                "or friends_tbl.sender_id=posts_tbl.user_id) " +
                "and (friends_tbl.sender_id=@user_id or friends_tbl.receiver_id=@user_id) " +
                "left join posts_hidden_tbl on  posts_tbl.post_id = posts_hidden_tbl.post_id " +
                "and posts_hidden_tbl.user_id =@user_id " +
                "where  friends_tbl.is_accepted=1 and posts_tbl.user_id!=@user_id " +
                "and (posts_tbl.post_date_time <= @datetime ) " +
                "and posts_hidden_tbl.user_id is null " +
                "order by posts_tbl.post_date_time DESC " +
                "offset (@offset) rows " +
                "fetch next (@POSTS_ROWS) rows only " +
                "end ";


            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@user_id", user_id);
            cmd.Parameters.AddWithValue("@datetime", datetime);
            cmd.Parameters.AddWithValue("@POSTS_ROWS", POSTS_ROWS);
            cmd.Parameters.AddWithValue("@offset", offset);
            cmd.Parameters.AddWithValue("@type", type);



            return universalRetriver(query, cmd, Posts_FRIENDS);
        }












        /// <summary>
        ///                            Sharing a post function
        ///                             returns
        ///                             1- task is done 
        ///                             0- my exception
        ///                             -1- System exception
        /// 
        /// </summary>
        /// <param name="User_id"></param>
        /// <param name="Post_text"></param>
        /// <param name="Post_image_path"></param>
        /// <param name="Post_date_time"></param>
        /// <param name="original_post_id"></param>
        /// <returns></returns>
        public static Status onSharingPost(Post post)
        {
            string query = "insert into posts_tbl(" +
               "user_id ," +
               "post_text," +
               "post_date_time," +
               "post_type," +
               "post_comments_count ," +
               "post_likes_count ," +
               "post_dislike_count ," +
               "post_reports_count ," +
               "post_share_count," +
               "post_is_shared," +
               "original_post_id)" +
               "values (@user_id, @post_text,@post_date_time,@post_type,@post_comments_count ," +
               "@post_likes_count,@post_dislike_count,@post_reports_count,@post_share_count,@post_is_shared,@original_post_id); " +
               "" +
               "select SCOPE_IDENTITY() as 'post_id'; " +
                "" +
                "if(@post_is_shared=1) " +
                "begin " +
                "update posts_tbl set post_share_count=post_share_count+1 where post_id=@original_post_id " +
                "end; ";


            SqlCommand command = new SqlCommand();
            command.Parameters.AddWithValue("@user_id", post.User_id);
            command.Parameters.AddWithValue("@post_text", post.Post_text);
            command.Parameters.AddWithValue("@post_date_time", post.Post_date_time);
            command.Parameters.AddWithValue("@post_type", post.Post_type);
            command.Parameters.AddWithValue("@original_post_id", post.Original_post_id);


            command.Parameters.AddWithValue("@post_comments_count", POST_COMMENTS_COUNT);
            command.Parameters.AddWithValue("@post_likes_count", POST_LIKES_COUNT);
            command.Parameters.AddWithValue("@post_dislike_count", POST_DISLIKES_COUNT);
            command.Parameters.AddWithValue("@post_reports_count", POST_REPORTS_COUNT);
            command.Parameters.AddWithValue("@post_share_count", POST_SHARE_COUNT);
            command.Parameters.AddWithValue("@post_is_shared", post.Post_is_shared);


            Status s1 = insertingToDBGetIdentity(query, command);
            if (s1.State == 1 && post.Post_images_url != null)
            {
                int post_id = int.Parse(s1.Json_data);
                String q = "insert into images(user_id,post_id,image_url) values (@user_id,@post_id,@image_url)";

                for (int i = 0; i < post.Post_images_url.Count; i++)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Parameters.AddWithValue("@user_id", post.User_id);
                    cmd.Parameters.AddWithValue("@post_id", post_id);
                    cmd.Parameters.AddWithValue("@image_url", post.Post_images_url.ElementAt(i));

                    s1 = insertingToDB(q, cmd);
                    if (s1.State != 1)
                    {
                        return s1;
                    }
                }

                return s1;

            }
            else
            {
                int post_id = int.Parse(s1.Json_data);
                string q = "insert into shares_tbl(user_id, post_id, share_datetime) values(@user_id, @post_id, @post_date_time); ";
                SqlCommand cmd = new SqlCommand();
                cmd.Parameters.AddWithValue("@user_id", post.User_id);
                cmd.Parameters.AddWithValue("@post_id", post_id);

                return s1;
            }
        }









        private static User initUser(SqlDataReader reader)
        {
            User user = new User();

            user.User_first_name = reader["user_first_name"].ToString();
            user.User_last_name = reader["user_last_name"].ToString();
            user.User_birth_date = reader["user_birth_date"].ToString();
            user.User_profile_photo = reader["user_profile_photo"].ToString();
            user.User_posts_count = int.Parse(reader["user_posts_count"].ToString());
            user.User_photos_count = int.Parse(reader["user_photos_count"].ToString());
            user.User_connection_count = int.Parse(reader["user_connection_count"].ToString());
            user.User_reputation = int.Parse(reader["user_reputation"].ToString());
            user.User_city = reader["user_city"].ToString();
            user.User_country = reader["user_country"].ToString();
            user.User_job = reader["user_job"].ToString();
            user.User_is_online = bool.Parse(reader["user_is_online"].ToString());
            user.User_is_private = bool.Parse(reader["account_is_private"].ToString());
            user.User_bio = reader["user_bio"].ToString();

            try
            {
                user.Friend_request_id = int.Parse(reader["request_id"].ToString());
                user.Is_accepted = bool.Parse(reader["is_accepted"].ToString());
                user.Sender_id = int.Parse(reader["sender_id"].ToString());

            }
            catch (Exception ex1)
            {
                user.Friend_request_id = -1;
                user.Is_accepted = false;
                user.Sender_id = -1;
            }



            return user;
        }








        private static Post initPost(SqlDataReader reader, int type)
        {
            //report id ***

            Post post = new Post();

            post.Post_id = reader.GetInt32(0);
            post.User_id = reader.GetInt32(1);
            post.Post_text = reader.GetString(2);
            post.Post_date_time = reader.GetDateTime(3);
            post.Post_type = reader.GetInt32(4);
            post.Post_comments_count = reader.GetInt32(5);
            post.Post_likes_count = reader.GetInt32(6);
            post.Post_dislike_count = reader.GetInt32(7);
            post.Post_reports_count = reader.GetInt32(8);
            post.Post_share_count = reader.GetInt32(9);
            post.Post_is_shared = reader.GetBoolean(10);

            try
            {
                post.Original_post_id = reader.GetInt32(11);
            }
            catch (Exception e1)
            {

            }

            post.User_first_name = reader.GetString(12);
            post.User_last_name = reader.GetString(13);


            if (reader["like_id"].ToString() != "")
                post.Like_id = int.Parse(reader["like_id"].ToString());
            else
                post.Like_id = -1;



            if (reader["dis_like_id"].ToString() != "")
                post.Dis_like_id = int.Parse(reader["dis_like_id"].ToString());
            else
                post.Dis_like_id = -1;


            post.User_profile_photo = reader["user_profile_photo"].ToString() + ",";


            ///-------------- handling post images 

            String images = reader["image_urls"].ToString();



            List<String> strings = new List<String>();

            int start = 0;
            int len = 0;


            for (int i = 0; i < images.Length; i++)
            {
                if (images[i] == ',')
                {
                    strings.Add(images.Substring(start, len));
                    start = i + 1;
                    len = 0;
                }
                else
                    len++;
            }

            post.Post_images_url = strings;

            if (type == Posts_FRIENDS)
                int.TryParse(reader["saved_post_id"].ToString(), out post.saved_post_id);


            return post;
        }






        private static Like iniDistLike(SqlDataReader reader)
        {
            Like like = new Like();

            like.User_id = reader.GetInt32(0);
            like.User_name = reader["user_first_name"].ToString() + " " + reader["user_last_name"].ToString();
            like.Profile_image = reader["user_profile_photo"].ToString();
            like.Time = reader["dis_like_datetime"].ToString();

            return like;
        }



        private static Like initLike(SqlDataReader reader)
        {
            Like like = new Like();

            like.User_id = reader.GetInt32(0);
            like.User_name = reader["user_first_name"].ToString() + " " + reader["user_last_name"].ToString();
            like.Profile_image = reader["user_profile_photo"].ToString();
            like.Time = reader["like_datetime"].ToString();

            return like;
        }





        private static DataTable GetDataTable(string query, SqlCommand cmd)
        {
            SqlConnection con = new SqlConnection(ConnectionInit.ConnectionString);
            DataTable dt = new DataTable();
            cmd.Connection = con;
            cmd.CommandText = query;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);
            return dt;
        }





        private static Status universalRetriver(string query, SqlCommand cmd, int type)
        {
            SqlConnection con = new SqlConnection(ConnectionInit.ConnectionString);
            Status status = new Status();



            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = query;

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    status.Json_data = "[";

                    while (reader.Read())
                    {
                        if (type == Posts_FRIENDS)
                        {
                            Post post = initPost(reader, Posts_FRIENDS);
                            status.Json_data += post + ",";
                        }
                        else if (type == PROFILE_POSTS)
                        {
                            Post post = initPost(reader, PROFILE_POSTS);
                            status.Json_data += post + ",";

                        }
                        else if (type == USER)
                        {
                            User user = initUser(reader);
                            status.Json_data += user + ",";
                        }
                        else if (type == POST_LIKES)
                        {
                            Like like = initLike(reader);
                            status.Json_data += like + ",";
                        }
                        else if (type == POST_DISLIKES)
                        {
                            Like like = iniDistLike(reader);
                            status.Json_data += like + ",";
                        }
                    }

                    status.Json_data = status.Json_data.Remove(status.Json_data.Length - 1);
                    status.State = 1;
                    status.Exception = "Done";
                    status.Json_data += "]";
                }
                else
                {
                    status.State = 0;
                    status.Exception = "not found";
                    status.Json_data = "";

                }

                reader.Close();
            }
            catch (Exception e1)
            {
                status.State = -1;
                status.Exception = e1.Message;
            }
            finally
            {
                con.Close();
            }

            return status;
        }













        private static Status universalObjeccctRetriver(string query, SqlCommand cmd, int type)
        {
            SqlConnection con = new SqlConnection(ConnectionInit.ConnectionString);
            Status status = new Status();

            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = query;

                SqlDataReader reader = cmd.ExecuteReader();


                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        if (type == LIKE)
                        {
                            status.Json_data = "{\"like_id\":" + reader["like_id"].ToString() + "}";

                        }
                    }

                    status.State = 1;
                    status.Exception = "Done";
                }
                else
                {
                    status.State = 0;
                    status.Exception = "not found";
                    status.Json_data = "";
                }

                reader.Close();
            }
            catch (Exception e1)
            {
                status.State = -1;
                status.Exception = e1.Message;
            }
            finally
            {
                con.Close();
            }

            return status;
        }







        /// <summary>
        ///                                       inserting function to the data base 
        ///                                       
        ///                                 0- somthing went wrong 
        ///                                 1- done 
        ///                                 -1- System Exception
        ///                                        
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private static Status insertingToDBGetIdentity(string query, SqlCommand cmd)
        {
            Status status = new Status();
            SqlConnection con = new SqlConnection(ConnectionInit.ConnectionString);

            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = query;

                SqlDataReader reader = cmd.ExecuteReader();


                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        status.Json_data = reader["post_id"].ToString();
                    }

                    status.State = 1;
                    status.Exception = "Done";
                }
                else
                {
                    status.State = 0;
                    status.Exception = "not found";
                    status.Json_data = "";
                }
            }
            catch (Exception e1)
            {
                status.State = -1;
                status.Exception = e1.Message;
            }
            finally
            {
                con.Close();
            }

            return status;
        }












        /// <summary>
        ///                                       inserting function to the data base 
        ///                                       
        ///                                 0- somthing went wrong 
        ///                                 1- done 
        ///                                 -1- System Exception
        ///                                        
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private static Status insertingToDB(string query, SqlCommand cmd)
        {
            Status status = new Status();
            status.State = 0;
            status.Exception = "somthing went wrong";
            SqlConnection con = new SqlConnection(ConnectionInit.ConnectionString);

            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
                status.State = 1;
                status.Exception = "Done";
            }
            catch (Exception e1)
            {
                status.State = -1;
                status.Exception = e1.Message;
            }
            finally
            {
                con.Close();
            }

            return status;
        }


    }
}