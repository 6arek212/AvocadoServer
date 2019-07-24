using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Web;
using WebApplication14.Models;

namespace WebApplication14.Controllers
{
    public class ConnectionInit
    {

        public static String ConnectionString = @"Data Source=SQL5041.site4now.net;Initial Catalog=DB_A467A4_ahmadgabarin;User Id=DB_A467A4_ahmadgabarin_admin;Password=ahmadwar33;";






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
            SqlConnection con = new SqlConnection(ConnectionString);

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

        public static Status go_to_insertingToDB(String query,SqlCommand cmd)
        {
            return insertingToDB(query, cmd);
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
            SqlConnection con = new SqlConnection(ConnectionString);

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
            SqlConnection con = new SqlConnection(ConnectionString);
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
                        if (type == USER_INFO)
                        {
                          UserInfo user=initUserInfo(reader);
                            status.Json_data += user + ",";
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









        ///
        /// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private static int USER_INFO = 1;





        //      for creating  tables 
        public static Status excuteQuery(string query)
        {
            return insertingToDB(query, new SqlCommand());
        }




        private static UserInfo initUserInfo(SqlDataReader reader)
        {
            UserInfo userInfo = new UserInfo();
            userInfo.Name = reader["user_first_name"].ToString() + " " + reader["user_last_name"].ToString();
            userInfo.Profile_image = reader["user_first_name"].ToString();
            return userInfo;
        }


        public static Status getUserInfo(int user_id)
        {
            String query = "select user_first_name,user_last_name,user_profile_photo " +
                "from users_tbl " +
                "where user_id=@user_id ";

            SqlCommand command = new SqlCommand();
            command.Parameters.AddWithValue("@user_id", user_id);


            return universalRetriver(query, command, USER_INFO);
        }





        /// <summary>
        ///                     
        ///                         updating last name , gender, birthdate 
        ///                         returns
        ///                             1- task is done 
        ///                             0- my exception
        ///                             -1- System exception
        /// 
        /// </summary>
        /// <param name="User_id"></param>
        /// <param name="User_last_name"></param>
        /// <param name="User_gender"></param>
        /// <param name="User_birth_date"></param>
        /// <returns></returns>
        public static Status updateProfilePohotoGenderBirthDate(int User_id, String profile_photo_path, int User_gender, String User_birth_date, String user_country)
        {

            String query = "update users_tbl set " +
                "user_profile_photo=@user_profile_photo , " +
                "User_gender=@user_gender, " +
                "user_birth_date= @birth_date, " +
                "user_country=@user_country " +
                "where user_id=@user_id ";


            SqlCommand command = new SqlCommand();
            command.Parameters.AddWithValue("@user_profile_photo", profile_photo_path);
            command.Parameters.AddWithValue("@user_gender", User_gender);
            command.Parameters.AddWithValue("@birth_date", User_birth_date);
            command.Parameters.AddWithValue("@user_country", user_country);
            command.Parameters.AddWithValue("@user_id", User_id);


            return insertingToDB(query, command);
        }





       





        /// <summary>
        ///                     updating city, country 
        ///                     returns
        ///                              1- task is done 
        ///                              0- my exception
        ///                             -1- System exception
        /// </summary>
        /// <param name="User_id"></param>
        /// <param name="User_country"></param>
        /// <param name="User_city"></param>
        /// <returns></returns>
        public static Status updateCityCountry(int User_id, String User_country, String User_city)
        {
            String query = "update users_tbl set" +
               " user_city=@user_city ," +
                "user_country=@user_country " +
                " where user_id=@user_id";


            SqlCommand command = new SqlCommand();
            command.Parameters.AddWithValue("@user_city", User_city);
            command.Parameters.AddWithValue("@user_country", User_country);
            command.Parameters.AddWithValue("@user_id", User_id);


            return insertingToDB(query, command);
        }




        public static Status updateProfileImage(int User_id, String image)
        {
            String query = "update users_tbl set " +
               "user_profile_photo=@image " +
                "where user_id=@user_id ";


            SqlCommand command = new SqlCommand();
            command.Parameters.AddWithValue("@image", image);
            command.Parameters.AddWithValue("@user_id", User_id);


            return insertingToDB(query, command);
        }
    }
}