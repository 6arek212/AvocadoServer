using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebApplication14.Models;

namespace WebApplication14.Controllers.Methods
{
    public class LocatioMethods
    {



        public static Status OngettingNearByUsers(int user_id, float latitude, float longitude, int distance,String text_cmp, String datetime,int offset)
        {
            String query = "DECLARE @g1 geography = 'POINT(' + cast(@latitude as nvarchar) + ' ' + cast(@longitude as nvarchar) + ')' " +
                "" +
                "SELECT " +
                "users_tbl.user_id, " +
                "users_tbl.user_first_name," +
                "users_tbl.user_last_name," +
                "users_tbl.user_profile_photo , " +
                "users_tbl.user_city ," +
                "users_tbl.user_country ," +
                "friends_tbl.is_accepted ," +
                "friends_tbl.request_id, " +
                "friends_tbl.sender_id," +
                "cast(user_current_location.STDistance(@g1) /1000 as numeric(10, 1)) as 'Distance' " +
                "" +
                "FROM users_tbl " +
                "left join friends_tbl on (friends_tbl.sender_id=users_tbl.user_id or friends_tbl.receiver_id=users_tbl.user_id) " +
                "and (friends_tbl.sender_id=@user_id or friends_tbl.receiver_id=@user_id) " +
                "WHERE " +
                "user_current_location is not NULL and user_id != @user_id " +
                "and " +
                "(CONVERT(VARCHAR,users_tbl.user_first_name)+' '+CONVERT(VARCHAR,users_tbl.user_last_name)) like @text_cmp+'%'  " +
                "and " +
                "users_tbl.register_datetime <= @datetime " +
                "and " +
                "users_tbl.user_is_active=1 " +
                "and " +
                "user_current_location.STDistance( 'POINT(' + cast(@latitude as nvarchar) + ' ' + cast(@longitude as nvarchar) + ')')/1000 < @distance  " +
                "ORDER BY user_current_location.STDistance( 'POINT(' + cast(@latitude as nvarchar) + ' ' + cast(@longitude as nvarchar) + ')') ASC " +
                "offset (@offset) rows " +
                "fetch next 20 rows only";


            SqlCommand command = new SqlCommand();
            command.Parameters.AddWithValue("@user_id", user_id);
            command.Parameters.AddWithValue("@latitude", latitude);
            command.Parameters.AddWithValue("@longitude", longitude);
            command.Parameters.AddWithValue("@distance ", distance);
            command.Parameters.AddWithValue("@datetime ", datetime);
            command.Parameters.AddWithValue("@offset ", offset);
            command.Parameters.AddWithValue("@text_cmp ", text_cmp);



            return universalRetriver(query, command, 1);
        }








        public static Status OnUpdatingUserLocation(int user_id, double latitude, double longitude)
        {
            String query = "update users_tbl set user_current_location='POINT("+latitude+" "+longitude+")' where user_id=@user_id";

            SqlCommand command = new SqlCommand();
            command.Parameters.AddWithValue("@user_id", user_id);
         

            return insertingToDB(query, command);
        }











        private static UserSearch initUserSearch(SqlDataReader reader)
        {
            UserSearch usersearch = new UserSearch();
            usersearch.User_id = reader.GetInt32(0);
            usersearch.User_first_name = reader.GetString(1);
            usersearch.User_last_name = reader.GetString(2);
            usersearch.User_profile_photo = reader.GetString(3);
            usersearch.User_city = reader.GetString(4);
            usersearch.User_country = reader.GetString(5);



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


            usersearch.Distance = float.Parse(reader["Distance"].ToString());


            return usersearch;

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
                        UserSearch userSearch = initUserSearch(reader);
                        status.Json_data += userSearch.ToString() + ",";
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




    }
}