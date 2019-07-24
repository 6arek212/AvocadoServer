using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication14.Controllers.Methods
{
    public class BiographyMethod
    {

        public static Status update_user_bio(String userid,String bio)
        {
            String query = "update users_tbl set user_bio=@bio where user_id=@userid";
            SqlCommand command = new SqlCommand();

            //parameters
            command.Parameters.AddWithValue("@bio", bio);
            command.Parameters.AddWithValue("@userid", userid);

          return   ConnectionInit.go_to_insertingToDB(query, command); 
        }
    }
}