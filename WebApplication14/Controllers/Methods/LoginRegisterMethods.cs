using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebApplication14.Models;

namespace WebApplication14.Controllers.Methods
{
    public class LoginRegisterMethods
    {

        //Starting values for new user
        private static String LAST_NAME = "";
        private static String PROFILE_PHOTO = "";
        private static int POST_COUNT = 0;
        private static int PHOTOS_COUNT = 0;
        private static int CONNECTION_COUNT = 0;
        private static int REPUTATION = 100;
        private static String CURRENT_LOCATION = "";
        private static String CITY = "";
        private static String COUNTRY = "";
        private static int IS_ACTIVE = 1;

        private static int LOGIN_CODE = 1;
        private static int REGISTER_CODE = 2;

        // function for registering new user and giving a starting values 

        public static Status OnInsertingNewUser(string user_first_name, String last_name, string user_email, string user_passsword, String register_datetime)
        {
            string query = "insert into users_tbl(" +
                "user_first_name," +
                "user_last_name," +
                "user_email," +
                "user_passsword," +
                "user_profile_photo," +
                "user_posts_count," +
                "user_photos_count," +
                "user_connection_count," +
                "user_reputation," +
                "user_city," +
                "user_country," +
                "user_job," +
                "user_location_swich," +
                "account_is_private," +
                "user_is_online," +
                "user_is_active," +
                "register_datetime) values (@first_name,@last_name,@email,@password,@profile_photo,@post_count,@photos_count,@connection_count," +
                "@reputation,@city,@country,@user_job,@user_location_swich,@account_is_private,@user_is_online,@is_active,@register_datetime); " +
                "" +
                "select user_id from users_tbl where CONVERT(VARCHAR, user_email)=@email and CONVERT(VARCHAR, user_passsword)=@password ";

            SqlCommand command = new SqlCommand();
            command.Parameters.AddWithValue("@first_name", user_first_name);
            command.Parameters.AddWithValue("@last_name", last_name);
            command.Parameters.AddWithValue("@email", user_email);
            command.Parameters.AddWithValue("@password", user_passsword);
            command.Parameters.AddWithValue("@register_datetime", register_datetime);


            command.Parameters.AddWithValue("@user_job", "");
            command.Parameters.AddWithValue("@user_location_swich", false);
            command.Parameters.AddWithValue("@account_is_private", false);
            command.Parameters.AddWithValue("@user_is_online", false);
            command.Parameters.AddWithValue("@profile_photo", PROFILE_PHOTO);
            command.Parameters.AddWithValue("@post_count", POST_COUNT);
            command.Parameters.AddWithValue("@photos_count", PHOTOS_COUNT);
            command.Parameters.AddWithValue("@connection_count", CONNECTION_COUNT);
            command.Parameters.AddWithValue("@reputation", REPUTATION);
            command.Parameters.AddWithValue("@current_location", CURRENT_LOCATION);
            command.Parameters.AddWithValue("@city", CITY);
            command.Parameters.AddWithValue("@country", COUNTRY);
            command.Parameters.AddWithValue("@is_active", IS_ACTIVE);


            return insertingToDBandRetunIdentity(query, command, REGISTER_CODE);
        }




        public static Status userOnline(int user_id, bool state)
        {
            string query = "update users_tbl set user_is_online=@state where user_id=@user_id";

            SqlCommand command = new SqlCommand();
            command.Parameters.AddWithValue("@user_id", user_id);
            command.Parameters.AddWithValue("@state", state);

            return insertingToDB(query, command);
        }




        //     1-private account       2-location 
        public static Status settings(int user_id, bool state, int type)
        {
            string query = "if(@type=1) " +
                "begin " +
                "update users_tbl set user_location_swich=@state where user_id=@user_id " +
                "end " +
                "" +
                "else " +
                "begin " +
                "update users_tbl set account_is_private=@state where user_id=@user_id " +
                "end ";

            SqlCommand command = new SqlCommand();
            command.Parameters.AddWithValue("@user_id", user_id);
            command.Parameters.AddWithValue("@state", state);
            command.Parameters.AddWithValue("@type", type);

            return insertingToDB(query, command);
        }




        public static Status getSettings(int user_id)
        {
            string query = "select account_is_private,user_location_swich from users_tbl where user_id=@user_id ";

            SqlCommand command = new SqlCommand();
            command.Parameters.AddWithValue("@user_id", user_id);


            return insertingToDB(query, command);
        }



        public static Status deleteAccount(int user_id, String password)
        {

            string query = "if(@password=CONVERT(VARCHAR, (select user_passsword from users_tbl where user_id=@user_id))) " +
                "begin " +
                "update users_tbl set user_is_active=0 where user_id=@user_id " +
                "end " +
                "else " +
                "" +
                "begin " +
                "select user_id from users_tbl where user_id=@user_id " +
                "end";




            SqlCommand command = new SqlCommand();
            command.Parameters.AddWithValue("@user_id", user_id);
            command.Parameters.AddWithValue("@password", password);


            Status status = new Status();
            try
            {
                DataTable dt = insertToDBWithDT(query, command);

                if (dt.Rows.Count > 0)
                {
                    status.State = 0;
                    status.Exception = "password incorrect";
                }
                else
                {
                    status.State = 1;
                    status.Exception = "done";
                }

            }
            catch (Exception e)
            {
                status.State = -1;
                status.Exception = e.Message;
            }


            return status;
        }




        private static DataTable insertToDBWithDT(String query, SqlCommand cmd)
        {
            SqlConnection con = new SqlConnection(ConnectionInit.ConnectionString);
            cmd.Connection = con;
            cmd.CommandText = query;
            DataTable dt = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);
            return dt;
        }








        public static Status checkIfExists(string User_Email, string User_password)
        {
            string query = "if exists( select user_id from users_tbl where CONVERT(VARCHAR, user_email)=@email " +
                "and CONVERT(VARCHAR, user_passsword)=@password and user_is_active=1 ) " +
                "begin " +
                "select user_id,user_first_name,user_last_name,user_profile_photo, " +
                "user_location_swich,account_is_private from users_tbl where CONVERT(VARCHAR, user_email)=@email " +
                "and CONVERT(VARCHAR, user_passsword)=@password " +
                "end ";


            SqlCommand command = new SqlCommand();
            command.Parameters.AddWithValue("@email", User_Email);
            command.Parameters.AddWithValue("@password", User_password);



            return insertingToDBandRetunIdentity(query, command, LOGIN_CODE);
        }


        public static Status checkIfEmailExists(string User_Email)
        {
            string query = "select * from users_tbl where  CONVERT(VARCHAR, user_email) =" + "'" + User_Email + "'";
            return checkIfExistsInDB(query);
        }


        private static Setting initSettings(SqlDataReader reader, int type)
        {
            Setting setting = new Setting();

            setting.User_id = int.Parse(reader["user_id"].ToString());

            if (type == LOGIN_CODE)
            {
                setting.User_location_switch = reader.GetBoolean(4);
                setting.Account_is_private = reader.GetBoolean(5);
                setting.User_first_name = reader["user_first_name"].ToString();
                setting.User_last_name = reader["user_last_name"].ToString();
                setting.ProfilePic = reader["user_profile_photo"].ToString();
            }

            return setting;
        }






        private static Status insertingToDBandRetunIdentity(string query, SqlCommand cmd, int type)
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

                        Setting setting = initSettings(reader, type);
                        status.Json_data = setting.ToString();
                    }
                    status.State = 1;
                    status.Exception = "Done";
                }

                else
                {
                    status.State = 0;
                    status.Exception = "not found";
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








        //checks return 

        /// <summary>
        ///                                 0- not exists 
        ///                                 1- exists
        ///                                 -1 - Exception error
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>


        private static Status checkIfExistsInDB(string query)
        {
            SqlConnection con = new SqlConnection(ConnectionInit.ConnectionString);
            Status status = new Status();

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = query;

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    status.State = 1;
                    status.Exception = "Done";
                }
                else
                {
                    status.State = 0;

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