using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebApplication14.Models;

namespace WebApplication14.Controllers.Methods
{
    public class ConnectionMethods
    {


        private static int CONNECTIONS = 1;
        private static int USER_FRIENDS = 2;

        





        public static Status onDeleteFriendRequest(int request_id, int user_id)
        {
            string query = "if((select is_accepted from friends_tbl where request_id=@request_id)!=1) " +
                "begin " +
                "delete from friends_tbl where request_id=@request_id; " +
                "if((select user_id_sent_notification from notification_tbl where request_id=@request_id)=@user_id) " +
                "begin " +
                "delete from notification_tbl where request_id=@request_id " +
                "end " +
                "end " +
                "else " +
                "begin " +
                "declare @u1 int ,@u2 int " +
                "set @u1=(select sender_id from friends_tbl where request_id=@request_id) " +
                "set @u2=(select receiver_id from friends_tbl where request_id=@request_id) " +
                "delete from friends_tbl where request_id=@request_id " +
                "update users_tbl set user_connection_count=user_connection_count-1 where user_id=@u1 " +
                "update users_tbl set user_connection_count=user_connection_count-1 where user_id=@u2 " +
                "end ";

            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@request_id", request_id);
            cmd.Parameters.AddWithValue("@user_id", user_id);


            return insertingToDB(query, cmd);
        }



        public static Status onDeleteFriend(int request_id, int u1, int u2)
        {
            string query = "if exists(select 1 from friends_tbl where request_id=@request_id) " +
                "begin " +
                "delete from friends_tbl where request_id=@request_id " +
                "update users_tbl set user_connection_count=user_connection_count-1 where user_id=@u1 " +
                "update users_tbl set user_connection_count=user_connection_count-1 where user_id=@u2 " +
                "end ";


            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@request_id", request_id);
            cmd.Parameters.AddWithValue("@u1", u1);
            cmd.Parameters.AddWithValue("@u2", u2);

            return insertingToDB(query, cmd);
        }







        /// <summary>
        ///                             function for sending friend request 
        ///                             return
        ///                             1 - task is done 
        ///                             0- my exception
        ///                             -1- System exception
        /// </summary>
        /// <param name="Sender_id"></param>
        /// <param name="Receiver_id"></param>
        /// <param name="date_time_sent"></param>
        /// <returns></returns>

        public static Status onSendingNewFriemdRequest(int Sender_id, int Receiver_id, String date_time_sent)
        {
            string query = "IF not EXISTS (select request_id from friends_tbl where (sender_id=@Sender_id or receiver_id=@Sender_id )" +
                " and (sender_id=@Receiver_id or receiver_id=@Receiver_id )) " +
                "begin " +
                "insert into friends_tbl( " +
                "sender_id ," +
                "receiver_id ," +
                "date_time_sent," +
                "is_accepted)" +
                "values(@Sender_id,@Receiver_id,@date_time_sent,@is_accepted); " +
                "select request_id from friends_tbl where sender_id=@Sender_id and receiver_id=@Receiver_id " +
                "end ";


            SqlCommand command = new SqlCommand();
            command.Parameters.AddWithValue("@Sender_id", Sender_id);
            command.Parameters.AddWithValue("@Receiver_id", Receiver_id);
            command.Parameters.AddWithValue("@date_time_sent", date_time_sent);
            command.Parameters.AddWithValue("@is_accepted", false);



            DataTable dt=GetDataTable(query, command);
            Status status = new Status();

            if (dt.Rows.Count > 0)
            {

                status.State = 1;
                status.Json_data = "{\"request_id\":" + dt.Rows[0]["request_id"].ToString()+"}";

            }
            else
            {
                status.State = 0;
                status.Exception = "theres is already a friend request";
            }
            return status;
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







        /// <summary>
        ///                 getting all user friends 
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="offset"></param>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static Status getFriends(int user_id, int offset, String datetime)
        {
            string query = "select " +
                "users_tbl.user_id," +
                "users_tbl.user_first_name," +
                " users_tbl.user_last_name," +
                "users_tbl.user_profile_photo," +
                "friends_tbl.request_id " +
                "from " +
                "users_tbl " +
                "left join friends_tbl on (users_tbl.user_id=friends_tbl.sender_id or users_tbl.user_id=friends_tbl.receiver_id) " +
                "and (@user_id=friends_tbl.sender_id or @user_id=friends_tbl.receiver_id) " +
                "where  users_tbl.user_id!=@user_id and (@user_id=friends_tbl.sender_id or @user_id=friends_tbl.receiver_id) " +
                "and friends_tbl.is_accepted=1 and friends_tbl.date_time_accepted<=@datetime " +
                "order by friends_tbl.date_time_accepted " +
                "offset (@offset) rows " +
                "fetch next 20 rows only ";

            SqlCommand command = new SqlCommand();
            command.Parameters.AddWithValue("@user_id", user_id);
            command.Parameters.AddWithValue("@offset", offset);
            command.Parameters.AddWithValue("@datetime", datetime);


            return universalRetriver(query, command, USER_FRIENDS);
        }








        /// <summary>
        ///                     function for accepting a friend request 
        ///                     updating the is_accepted filed in friend_tbl
        /// </summary>
        /// <param name="request_id"></param>
        /// <param name="date_time_accepted"></param>
        /// <returns></returns>

        public static Status onAcceptingFriendRequest(int request_id, String date_time_accepted)
        {
            string query = "if ((select is_accepted from friends_tbl where request_id=@request_id)!=1) " +
                "begin " +
                "update friends_tbl set is_accepted=@is_accepted , date_time_accepted=@date_time_accepted " +
                "where request_id=@request_id; "+
                "declare @u1 int ,@u2 int " +
                "set @u1=(select sender_id from friends_tbl where request_id=@request_id) " +
                "set @u2=(select receiver_id from friends_tbl where request_id=@request_id) " +
                "update users_tbl set user_connection_count=user_connection_count+1 where user_id=@u1 " +
                "update users_tbl set user_connection_count=user_connection_count+1 where user_id=@u2 " +
                "end ";
                

            SqlCommand command = new SqlCommand();
            command.Parameters.AddWithValue("@date_time_accepted", date_time_accepted);
            command.Parameters.AddWithValue("@is_accepted", true);
            command.Parameters.AddWithValue("@request_id", request_id);

            return insertingToDB(query, command);
        }








        public static Status onGettingOutgoingConnectionsRequest(int user_id)
        {
            String query = "select users_tbl.user_id ," +
                "users_tbl.user_first_name , " +
                "users_tbl.user_last_name , " +
                "users_tbl.user_profile_photo , " +
                "users_tbl.user_city ," +
                "users_tbl.user_country ," +
                "friends_tbl.is_accepted ," +
                "friends_tbl.request_id" +
                "from users_tbl " +
                "left join friends_tbl on friends_tbl.receiver_id = users_tbl.user_id " +
                "where friends_tbl.is_accepted=0 and friends_tbl.sender_id =@user_id";


            SqlCommand command = new SqlCommand();

            command.Parameters.AddWithValue("@user_id", user_id);

            return universalRetriver(query, command, CONNECTIONS);
        }







        public static Status onGettingIncommingConnectionsRequest(int user_id, int offset,String datetime)
        {
            String query = "" +
                "select " +
                "users_tbl.user_id ," +
                "users_tbl.user_first_name , " +
                "users_tbl.user_last_name , " +
                "users_tbl.user_profile_photo , " +
                "users_tbl.user_city ," +
                "users_tbl.user_country ," +
                "friends_tbl.is_accepted ," +
                "friends_tbl.request_id, " +
                "friends_tbl.sender_id " +
                "from users_tbl " +
                "left join friends_tbl on friends_tbl.sender_id = users_tbl.user_id " +
                "where (friends_tbl.is_accepted=0 and friends_tbl.receiver_id =@user_id ) " +
                "and date_time_sent<=@datetime " +
                "order by date_time_sent DESC " +
                "OFFSET (@offset) ROWS " +
                "FETCH NEXT 20 ROWS ONLY ";
              



            SqlCommand command = new SqlCommand();

            command.Parameters.AddWithValue("@user_id", user_id);
            command.Parameters.AddWithValue("@offset", offset);
            command.Parameters.AddWithValue("@datetime", datetime);

            return universalRetriver(query, command, CONNECTIONS);
        }









        //offset get 10 every time //,int offset
        ///"offset(@offset) rows " +
        //      "fetch next (@ROWS_FOR_COMMENTS) rows only "
        public static Status onSearchingUserByName(int user_id, String text_cmp, String datetime, int offset)
        {
            String query = 
                "select " +
                "users_tbl.user_id ," +
                "users_tbl.user_first_name , " +
                "users_tbl.user_last_name , " +
                "users_tbl.user_profile_photo , " +
                "users_tbl.user_city ," +
                "users_tbl.user_country ," +
                "friends_tbl.is_accepted ," +
                "friends_tbl.request_id, " +
                "friends_tbl.sender_id " +
                "from users_tbl " +
                "left join friends_tbl on (users_tbl.user_id=friends_tbl.sender_id " +
                "or users_tbl.user_id=friends_tbl.receiver_id) " +
                "and (friends_tbl.receiver_id=@user_id or friends_tbl.sender_id=@user_id) " +
                "where user_id!=@user_id " +
                "and (CONVERT(Nvarchar,users_tbl.user_first_name)+' '+CONVERT(Nvarchar,users_tbl.user_last_name)) like @text_cmp+'%' " +
                "and users_tbl.user_is_active=1 " +
                "and register_datetime<@datetime " +
                "order by users_tbl.register_datetime " +
                "offset (@offset) rows " +
                "fetch next 20 rows only ";


            SqlCommand command = new SqlCommand();

            command.Parameters.AddWithValue("@user_id", user_id);
            command.Parameters.AddWithValue("@datetime", datetime);
            command.Parameters.AddWithValue("@offset", offset);
            command.Parameters.Add("@text_cmp", SqlDbType.NVarChar,text_cmp.Length,text_cmp).Value = text_cmp;


            return universalRetriver(query, command, CONNECTIONS);
        }






        private static Friend initFriend(SqlDataReader reader)
        {

            Friend friend = new Friend();
            friend.User_id = reader.GetInt32(0);
            friend.User_name = reader["user_first_name"].ToString() + " " + reader["user_last_name"].ToString();
            friend.User_profile_image = reader["user_profile_photo"].ToString();
            friend.Request_id = reader.GetInt32(4);
            return friend;
        }







        private static UserSearch initUserSearch(SqlDataReader reader)
        {
            UserSearch usersearch = new UserSearch();
            usersearch.User_id = reader.GetInt32(0);
            usersearch.User_first_name = reader["user_first_name"].ToString();
            usersearch.User_last_name = reader["user_last_name"].ToString();
            usersearch.User_profile_photo = reader["user_profile_photo"].ToString();
            usersearch.User_city = reader["user_city"].ToString();
            usersearch.User_country = reader["user_country"].ToString();

            try
            {
                usersearch.Is_accepted = reader.GetBoolean(6);
                usersearch.Request_id = reader.GetInt32(7);
                usersearch.Sender_id = reader.GetInt32(8);
            }
            catch (Exception e1)
            {
                usersearch.Request_id = -1;
                usersearch.Sender_id = -1;

            }

            return usersearch;
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
                        if (type == CONNECTIONS)
                        {
                            UserSearch userSearch = initUserSearch(reader);
                            status.Json_data += userSearch+ ",";
                        }
                        else if (type == USER_FRIENDS)
                        {
                            Friend friend = initFriend(reader);
                            status.Json_data += friend + ",";          
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
















        /// <summary>
        ///                                       inserting function to the data base insertingToDBandRetunIdentity
        ///                                       
        ///                                 0- somthing went wrong 
        ///                                 1- done 
        ///                                 -1- System Exception
        ///                                        
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private static Status insertingToDBandRetunIdentity(string query, SqlCommand cmd)
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
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        status.Json_data = "{\"request_id\":" + reader.GetInt32(0) + "}";
                    }

                }

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