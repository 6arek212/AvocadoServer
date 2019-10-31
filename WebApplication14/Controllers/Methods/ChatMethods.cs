using Json.Net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebApplication14.Models;

namespace WebApplication14.Controllers.Methods
{
    public class ChatMethods
    {


        private static int CHECK_IF_EXISTS_REQUESTS = 1;
        private static int SENDDING_MESSAGE = 2;
        private static int GET_USERS_AND_CHATS = 3;
        private static int GET_CHATS = 4;
        private static int UNREADED_MESSAGE = 5;


        private static int FETCH_ROWS = 10;



        public static Status OnCheckingCreatingChat(int sender_id, int receiver_id, String datetime, String message)
        {
            String query = "declare @chat_id int " +
                "IF EXISTS ( SELECT * FROM chats_tbl WHERE (chat_sender_id = @sender_id and chat_receiver_id=@receiver_id) " +
                "or (chat_sender_id = @receiver_id and chat_receiver_id=@sender_id)) " +
                "begin " +
                "set @chat_id=( select chat_id from chats_tbl WHERE (chat_sender_id = @sender_id and chat_receiver_id=@receiver_id) " +
                "or (chat_sender_id = @receiver_id and chat_receiver_id=@sender_id)) " +
                "" +
                "select @chat_id " +
                "end " +
                "" +
                "else " +
                "begin " +
                "" +
                "set @chat_id=( SELECT chat_id FROM chats_tbl WHERE (chat_sender_id = @sender_id and chat_receiver_id=@receiver_id) " +
                "or (chat_sender_id = @receiver_id and chat_receiver_id=@sender_id)) " +
                "" +
                "insert into chats_tbl (chat_sender_id,chat_receiver_id,chat_datetime_created, " +
                "chat_messages_count,chat_last_message_user_id,chat_last_message,chat_last_message_datetime, " +
                "chat_removed_sender,chat_removed_receiver,sender_not_read,receiver_not_read) " +
                "values (@sender_id,@receiver_id,@datetime,0,@sender_id,@message,@datetime, " +
                "0,0,0,0) " +
                "set @chat_id=(select SCOPE_IDENTITY()) " +
                "select @chat_id " +
                "" +
                "end; " +
                "" +
                "insert into chat_messages_tbl (chat_id,message_sender_id,message_text,message_datetime) " +
                "values (@chat_id,@sender_id,@message,@datetime); " +
                "select SCOPE_IDENTITY(); ";

            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@sender_id", sender_id);
            cmd.Parameters.AddWithValue("@receiver_id", receiver_id);
            cmd.Parameters.AddWithValue("@datetime", datetime);
            cmd.Parameters.AddWithValue("@message", message);

            return universalObjeccctRetriver(query, cmd, CHECK_IF_EXISTS_REQUESTS);
        }



        /*
         * 
         * select users_tbl.user_id , users_tbl.user_first_name , users_tbl.user_last_name , " +
                "chats_tbl.chat_id,chats_tbl.chat_last_message,chats_tbl.chat_last_message_datetime, " +
                "chats_tbl.chat_last_message_user_id,chats_tbl.sender_not_read,chats_tbl.receiver_not_read, " +
                "chats_tbl.chat_sender_id,chat_receiver_id " +
         * 
         * 
         * /

            */

        public static Status OnGettingUserAndChats(int user_id, String text, int offset)
        {
            String query = "select users_tbl.user_id , users_tbl.user_first_name , users_tbl.user_last_name , " +
                "chats_tbl.chat_id, users_tbl.user_profile_photo " +
                "from users_tbl " +
                "left join chats_tbl on " +
                "(chats_tbl.chat_sender_id=users_tbl.user_id or chats_tbl.chat_receiver_id=users_tbl.user_id)and " +
                "(chats_tbl.chat_sender_id=@user_id or chats_tbl.chat_receiver_id=@user_id) " +
                "left join friends_tbl on (friends_tbl.sender_id=users_tbl.user_id or friends_tbl.receiver_id=users_tbl.user_id) " +
                "and(friends_tbl.sender_id=@user_id or friends_tbl.receiver_id=@user_id) " +
                "where users_tbl.user_id!=@user_id and friends_tbl.is_accepted=1 " +
                "and (CONVERT(VARCHAR,users_tbl.user_first_name)+' '+CONVERT(VARCHAR,users_tbl.user_last_name)) like @text+'%' " +
                "order by friends_tbl.date_time_accepted DESC " +
                "offset (@offset) rows " +
                "fetch next (@FETCH_ROWS) rows only ";


            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@user_id", user_id);
            cmd.Parameters.AddWithValue("@FETCH_ROWS", FETCH_ROWS);
            cmd.Parameters.AddWithValue("@text", text);
            cmd.Parameters.AddWithValue("@offset", offset);



            return universalRetriver(query, cmd, GET_USERS_AND_CHATS);
        }



        public static Status OnSendingMessage(int chat_id, int user_id, String message, String datetime)
        {
            String query = "insert into chat_messages_tbl (chat_id,message_sender_id,message_text,message_datetime) " +
                "values(@chat_id,@user_id,@message,@datetime); " +
                "SELECT SCOPE_IDENTITY(); ";

            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@chat_id", chat_id);
            cmd.Parameters.AddWithValue("@user_id", user_id);
            cmd.Parameters.AddWithValue("@message", message);
            cmd.Parameters.AddWithValue("@datetime", datetime);

            return universalObjeccctRetriver(query, cmd, SENDDING_MESSAGE);
        }



        public static Status OnGettingChats(int user_id)
        {
            String query = "select users_tbl.user_first_name, users_tbl.user_last_name, users_tbl.user_profile_photo, " +
                "chat_id,chat_sender_id,chat_receiver_id,chat_datetime_created,chat_messages_count,chat_last_message_user_id, " +
                "chat_last_message,chat_last_message_datetime,sender_not_read,receiver_not_read " +
                "from chats_tbl " +
                "left join users_tbl on (users_tbl.user_id=chats_tbl.chat_sender_id and chats_tbl.chat_sender_id!=@user_id) " +
                "or(users_tbl.user_id=chats_tbl.chat_receiver_id and chats_tbl.chat_receiver_id!=@user_id) " +
                "where " +
                "(chat_sender_id=@user_id and (chat_sender_id=@user_id and chat_removed_sender=0) ) or   " +
                "( chat_receiver_id=@user_id and (chat_receiver_id=@user_id and  chat_removed_receiver=0))  " +
                "order by chat_datetime_created DESC";

            /*"order by chat_datetime_created DESC " +
            "offset (@offset) rows " +
            "fetch next (@rows) rows only  ";*/


        SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@user_id", user_id);
            //  cmd.Parameters.AddWithValue("@offset", offset);
            // cmd.Parameters.AddWithValue("@rows", 20);

            return universalRetriver(query, cmd, GET_CHATS);
        }


        public static Status OnGettingUserChatsWithUpdatedValues(int user_id)
        {
            String query = "select chat_id,chat_sender_id,chat_receiver_id,chat_datetime_created,chat_messages_count,chat_last_message_user_id, " +
               "chat_last_message,chat_last_message_datetime,sender_not_read,receiver_not_read " +
               "from chats_tbl " +
               "where " +
               "(chat_sender_id=@user_id or chat_receiver_id=@user_id) and  " +
               "((chat_sender_id=@user_id and chat_removed_sender=0) or (chat_receiver_id=@user_id and  chat_removed_receiver=0)) " +
               "and " +
               "((chat_sender_id=@user_id and sender_not_read>0) or (chat_receiver_id=@user_id and  receiver_not_read>0)) " +
               "order by chat_last_message_datetime DESC ";


            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@user_id", user_id);

            return universalRetriver(query, cmd, GET_CHATS);
        }







        





        public static Status OnGettingUnreadedMessages(int user_id, int chat_id)
        {
            String query = "declare @not_readed int " +
                "if((select chat_sender_id from chats_tbl where chat_id=@chat_id)=@user_id) " +
                "begin " +
                "set @not_readed=(select sender_not_read from chats_tbl where chat_id=@chat_id) " +
                "if(@not_readed >0) " +
                "begin " +
                "update chats_tbl set sender_not_read=0 where chat_id=@chat_id " +
                "end " +
                "end " +
                "" +
                "else " +
                "begin " +
                "set @not_readed=(select receiver_not_read from chats_tbl where chat_id=@chat_id) " +
                "if(@not_readed >0) " +
                "begin " +
                "update chats_tbl set receiver_not_read=0 where chat_id=@chat_id " +
                "end " +
                "end " +
                "" +
                "if(@not_readed >0) " +
                "begin " +
                "select top (@not_readed) chat_messages_tbl.message_id,chat_messages_tbl.message_sender_id " +
                ",chat_messages_tbl.message_text,chat_messages_tbl.message_datetime " +
                "from chat_messages_tbl " +
                "where chat_id=@chat_id and message_sender_id!=@user_id " +
                "order by chat_messages_tbl.message_datetime DESC " +
                "end " +
                "update users_tbl set unread_messages_count=0 where user_id=@user_id ";


            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@user_id", user_id);
            cmd.Parameters.AddWithValue("@chat_id", chat_id);


            return universalRetriver(query, cmd, UNREADED_MESSAGE);
        }








        private static Chat initChat(SqlDataReader reader)
        {
            Chat chat = new Chat();


            chat.User_first_name = reader.GetString(0);
            chat.User_last_name = reader.GetString(1);
            chat.User_profile_photo = reader.GetString(2);
            chat.Chat_id = reader.GetInt32(3);
            chat.Chat_sender_id = reader.GetInt32(4);
            chat.Chat_receiver_id = reader.GetInt32(5);
            chat.Chat_datetime_created = reader.GetDateTime(6);
            chat.Chat_messages_count = reader.GetInt32(7);
            chat.Chat_last_message_user_id = reader.GetInt32(8);
            chat.Chat_last_message = reader.GetString(9);
            chat.Chat_last_message_datetime = reader.GetDateTime(10);
            chat.Sender_not_read = reader.GetInt32(11);
            chat.Receiver_not_read = reader.GetInt32(12);


            return chat;
        }




        private static ChatUser initChatUser(SqlDataReader reader)
        {

            ChatUser chat = new ChatUser();

            chat.User_id = reader.GetInt32(0);
            chat.User_first_name = reader.GetString(1);
            chat.User_last_name = reader.GetString(2);

            chat.Profile_photo = reader["user_profile_photo"].ToString();

            try
            {
                chat.Chat_id = reader.GetInt32(3);

            }
            catch (Exception e1)
            {
                chat.Chat_id = -1;
            }

            return chat;
        }


        public static Status OnDeletingChat(int chat_id,int user_id)
        {
            String query = "declare @receiver int,@sender int " +
                "set @receiver=(select chat_receiver_id from chats_tbl) " +
                "set @sender=(select chat_sender_id from chats_tbl) " +
                "if(@user_id=@sender) " +
                "begin " +
                "update chats_tbl set chat_removed_sender=1 where chat_id=@chat_id " +
                "end " +
                "else " +
                "begin " +
                "update chats_tbl set chat_removed_receiver=1 where chat_id=@chat_id " +
                "end " +
                "if((select chat_removed_sender from chats_tbl where chat_id=@chat_id)=1 and (select chat_removed_receiver from chats_tbl where chat_id=@chat_id)=1) " +
                "begin " +
                "delete from chat_messages_tbl where chat_id=@chat_id " +
                "delete from chats_tbl where chat_id=@chat_id " +
                "end ";


            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@user_id", user_id);
            cmd.Parameters.AddWithValue("@chat_id", chat_id);

            Status status = new Status();

            try
            {
                getDataTable(query, cmd);
                status.State = 1;
            }
            catch (Exception e)
            {
                status.State = -1;
                status.Exception = e.Message;
            }
            return status;
        }




        private static DataTable getDataTable(String query, SqlCommand cmd)
        {
            SqlConnection con = new SqlConnection(ConnectionInit.ConnectionString);
            DataTable dt = new DataTable();
            cmd.Connection = con;
            cmd.CommandText = query;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);
            return dt;
        }


        private static Message initMessage(SqlDataReader reader)
        {
            Message message = new Message();

            message.Message_id = reader.GetInt32(0);
            message.Message_sender_id = reader.GetInt32(1);
            message.Message_text = reader.GetString(2);
            message.Message_datetime = reader.GetDateTime(3);

            return message;
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
                    while (reader.HasRows)
                    {
                        while (reader.Read())
                        {

                            if (type == GET_USERS_AND_CHATS)
                            {
                                ChatUser chat = initChatUser(reader);
                                status.Json_data += chat.ToString() + ",";

                            }
                            else if (type == GET_CHATS)
                            {
                                Chat chat = initChat(reader);
                                status.Json_data += chat.ToString() + ",";
                            }
                            else if (type == UNREADED_MESSAGE)
                            {
                                Message message = initMessage(reader);
                                status.Json_data += message + ",";
                            }

                        }

                        reader.NextResult();
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
            status.Json_data = "{";

            try
            {
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = query;

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            if (type == CHECK_IF_EXISTS_REQUESTS)
                            {

                                int Chat_id = reader.GetInt32(0);
                                status.Json_data += "\"Chat_id\":" + "\"" + JsonNet.Serialize(Chat_id) + "\",";
                            }
                            else if (type == SENDDING_MESSAGE)
                            {
                                int Message_id = int.Parse(reader.GetSqlValue(0).ToString());
                                status.Json_data += "\"Message_id\":" + "\"" + JsonNet.Serialize(Message_id) + "\"";
                            }



                        }

                        if (reader.NextResult())
                        {
                            if (reader.Read())
                            {
                                if (type == CHECK_IF_EXISTS_REQUESTS)
                                {
                                    int Message_id = int.Parse(reader.GetSqlValue(0).ToString());
                                    status.Json_data += "\"Message_id\":" + "\"" + JsonNet.Serialize(Message_id) + "\"";
                                }

                            }
                        }

                    }
                    status.Json_data += "}";
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
                    if (reader.Read())
                    {
                        status.Json_data = reader.GetSqlValue(0).ToString();
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