using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication14.Controllers.Methods
{
    public class Update_informationMethods
    {

        public static Status update_fist_last_name(String userid, String firstname, String lastname)
        {
            String query = "update users_tbl set user_first_name=@first_name,user_last_name=@last_name where user_id=@userid";
            SqlCommand command = new SqlCommand();

            //parameters
            command.Parameters.AddWithValue("@first_name", firstname);
            command.Parameters.AddWithValue("@last_name", lastname);
            command.Parameters.AddWithValue("@userid", userid);

            return ConnectionInit.go_to_insertingToDB(query, command);
        }

        public static Status update_emailaddress(String userid, String emailaddress)
        {
            String query = "update users_tbl set user_email=@emailaddress where user_id=@userid";
            SqlCommand command = new SqlCommand();

            //parameters
            command.Parameters.AddWithValue("@emailaddress", emailaddress);
            command.Parameters.AddWithValue("@userid", int.Parse(userid));

            return ConnectionInit.go_to_insertingToDB(query, command);
        }

        public static Status update_phonenumber(String userid, String phonenumber)
        {
            String query = "update users_tbl set user_phonenumber=@phonenumber where user_id=@userid";
            SqlCommand command = new SqlCommand();

            //parameters
            command.Parameters.AddWithValue("@phonenumber", phonenumber);
            command.Parameters.AddWithValue("@userid", userid);

            return ConnectionInit.go_to_insertingToDB(query, command);

        }
    }
}