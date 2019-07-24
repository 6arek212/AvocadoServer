using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebApplication14.Models;

namespace WebApplication14.Controllers.Methods
{
    public class CommentsMethods
    {

        private static int COMMENTS_TYPE = 3;
        private static int ROWS_FOR_COMMENTS = 10;


        public static Status onAddingNewComment(int Post_id, int User_id, String Comment_text, String Comment_date_time)
        {
            String query = "insert into comments_tbl (post_id,user_id,comment_text,comment_date_time) values (" +
                "@post_id," +
                "@user_id," +
                "@comment_text," +
                "@comment_date_time)";



            SqlCommand command = new SqlCommand();

            command.Parameters.AddWithValue("@post_id", Post_id);
            command.Parameters.AddWithValue("@user_id", User_id);
            command.Parameters.AddWithValue("@comment_text", Comment_text);
            command.Parameters.AddWithValue("@comment_date_time", Comment_date_time);

            return insertingToDB(query, command);
        }








        public static Status OndeleteComment(int comment_id,int post_id)
        {
            String query = "delete from comments_tbl where comment_id=@comment_id; " +
                "update posts_tbl set post_comments_count=post_comments_count-1 where post_id=@post_id";

            SqlCommand command = new SqlCommand();

            command.Parameters.AddWithValue("@comment_id", comment_id);
            command.Parameters.AddWithValue("@post_id", post_id);

            return insertingToDB(query, command);
        }






        public static Status onGettingComments(int Post_id, int offset, String datetime)
        {
            String query = " select comments_tbl.comment_id,comments_tbl.user_id ," +
                " comments_tbl.comment_text , comments_tbl.comment_date_time ," +
                " users_tbl.user_first_name , users_tbl.user_profile_photo " +
                "from comments_tbl " +
                "left join posts_tbl on posts_tbl.post_id= comments_tbl.post_id " +
                "left join users_tbl on users_tbl.user_id= comments_tbl.user_id " +
                "where posts_tbl.post_id= @post_id and comments_tbl.comment_date_time<=@datetime " +
                "order by comments_tbl.comment_date_time DESC " +
                "offset (@offset) rows " +
                "fetch next (@ROWS_FOR_COMMENTS) rows only ";


            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@post_id", Post_id);
            cmd.Parameters.AddWithValue("@offset", offset);
            cmd.Parameters.AddWithValue("@ROWS_FOR_COMMENTS", ROWS_FOR_COMMENTS);
            cmd.Parameters.AddWithValue("@datetime", datetime);

            return universalRetriver(query, cmd, COMMENTS_TYPE);
        }



        public static Status onGettingUpdatedComments(int Post_id, int Comments_incomming_count, String datetime)
        {
            String query = "declare @post_current_post_count int ,@comments_count_toget int " +
                "set @post_current_post_count=(select post_comments_count from posts_tbl where post_id=@post_id) " +
                "set @comments_count_toget=@post_current_post_count-@comments_incomming_count " +
                "if(@comments_count_toget>0)" +
                "begin " +
                "select top (@comments_count_toget) comments_tbl.comment_id,comments_tbl.user_id ," +
                " comments_tbl.comment_text , comments_tbl.comment_date_time ," +
                " users_tbl.user_first_name , users_tbl.user_profile_photo " +
                "from comments_tbl " +
                "left join posts_tbl on posts_tbl.post_id= comments_tbl.post_id " +
                "left join users_tbl on users_tbl.user_id= comments_tbl.user_id " +
                "where posts_tbl.post_id= @post_id and comments_tbl.comment_date_time>@datetime " +
                "order by comments_tbl.comment_date_time DESC " +
                "end ";


            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@post_id", Post_id);
            cmd.Parameters.AddWithValue("@comments_incomming_count", Comments_incomming_count);
            cmd.Parameters.AddWithValue("@datetime", datetime);

            Status status = new Status();
            status = universalRetriver(query, cmd, COMMENTS_TYPE);

            if (status.Json_data.Length < 3)
            {
                status.State = 0;
                status.Exception = "no more found";
            }

            return status;
        }




        private static Comment initComment(SqlDataReader reader)
        {
            Comment comment = new Comment();

            comment.Comment_id = reader.GetInt32(0);
            comment.Comment_user_id = reader.GetInt32(1);
            comment.Comment_text = reader.GetString(2);
            comment.Comment_date_time = reader.GetDateTime(3);
            comment.Comment_user_name = reader.GetString(4);
            comment.Comment_user_profile_image_path = reader.GetString(5);


            return comment;
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











        private static Status universalRetriver(string query, SqlCommand cmd, int type)
        {
            SqlConnection con = new SqlConnection(ConnectionInit.ConnectionString);
            Status status = new Status();
            status.State = 0;
            status.Exception = "not found";
            status.Json_data = "[";

            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = query;

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {

                    while (reader.Read())
                    {

                        if (type == COMMENTS_TYPE)
                        {
                            Comment comment = initComment(reader);
                            status.Json_data += comment.ToString() + ",";
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

    }
}