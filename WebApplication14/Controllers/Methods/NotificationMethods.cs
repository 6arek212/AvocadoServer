using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebApplication14.Models;

namespace WebApplication14.Controllers.Methods
{
    public class NotificationMethods
    {
        public static int USER_NOTIFICATION = 1;



        public static Status getUserNotification(int user_id, String datetime,int offset)
        {
            //need to retun chat_id 


            String query = "update users_tbl set notification_count=0 where user_id=@user_id; " +
                "select " +
                "users_tbl.user_first_name," +
                "users_tbl.user_last_name, " +
                "users_tbl.user_profile_photo," +
                "notification_tbl.user_id_sent_notification," +
                "notification_tbl.notification_type," +
                "notification_tbl.notification_datetime," +
                "notification_tbl.notification_id," +
                "notification_tbl.post_id," +
                "notifiaction_type_tbl.type_name " +
                "from notification_tbl " +
                "left join users_tbl on users_tbl.user_id =notification_tbl.user_id_sent_notification " +
                "left join notifiaction_type_tbl on notifiaction_type_tbl.type_id = notification_tbl.notification_type " +
                "where  notification_tbl.user_id_received_notification=@user_id and notification_datetime<@datetime " +   
                "order by notification_datetime DESC " +
                "offset (@offset) rows " +
                "fetch next (20) rows only ";
              

            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@datetime", datetime);
            cmd.Parameters.AddWithValue("@offset", offset);
            cmd.Parameters.AddWithValue("@user_id", user_id);



            return universalRetriver(query, cmd, USER_NOTIFICATION);
        }


        public static Status getUserNotificationService(int user_id, String datetime, int offset)
        {
            String query = "update users_tbl set notification_count=0 where user_id=@user_id; " +
                "select " +
                "users_tbl.user_first_name," +
                "users_tbl.user_last_name, " +
                "users_tbl.user_profile_photo," +
                "notification_tbl.user_id_sent_notification," +
                "notification_tbl.notification_type," +
                "notification_tbl.notification_datetime," +
                "notification_tbl.notification_id," +
                "notification_tbl.post_id," +
                "notifiaction_type_tbl.type_name " +
                "from notification_tbl " +
                "left join users_tbl on users_tbl.user_id =notification_tbl.user_id_sent_notification " +
                "left join notifiaction_type_tbl on notifiaction_type_tbl.type_id = notification_tbl.notification_type " +
                "where  notification_tbl.user_id_received_notification=@user_id and notification_datetime>@datetime " +
                "order by notification_datetime ASC " +
                "offset (@offset) rows " +
                "fetch next (20) rows only ";


            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@datetime", datetime);
            cmd.Parameters.AddWithValue("@offset", offset);
            cmd.Parameters.AddWithValue("@user_id", user_id);



            return universalRetriver(query, cmd, USER_NOTIFICATION);
        }


        public static Notification initNotification(SqlDataReader reader)
        {
            Notification notification = new Notification();

            notification.User_id_sent_notification = int.Parse(reader["user_id_sent_notification"].ToString());
            notification.User_sent_name = reader["user_first_name"].ToString() + " " + reader["user_last_name"].ToString();
            notification.User_sent_profile_image = reader["user_profile_photo"].ToString();
            notification.Notification_type = int.Parse(reader["notification_type"].ToString());
            notification.Notification_datetime = reader["notification_datetime"].ToString();
            notification.Notification_id = int.Parse(reader["notification_id"].ToString());
            notification.Type_txt= reader["type_name"].ToString();


            if (reader["post_id"].ToString()!="")
            {
                notification.Post_id = int.Parse(reader["post_id"].ToString());
            }
            else
            {
                notification.Post_id = -1;
            }

            return notification;
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
                        if(type== USER_NOTIFICATION)
                        {
                            Notification notification = initNotification(reader);
                            status.Json_data += notification + ",";
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