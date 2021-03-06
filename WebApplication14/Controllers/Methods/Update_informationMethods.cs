﻿using Json.Net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
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

        public static Status update_emailaddress(String userid, String emailaddress,String password)
        {
            
            String query = "" +
                "if(CONVERT(VARCHAR, (select user_passsword from users_tbl where user_id = @userid))=@password) " +
                "begin " +
                "if not exists(select * from users_tbl where  CONVERT(VARCHAR, user_email)=@emailaddress) " +
                "begin " +
                "update users_tbl set user_email=@emailaddress where user_id=@userid " +
                "end " +
                "else " +
                "begin " +
                "select 'E-mail already exists' as 'data' " +
                "end " +
                "end " +
                "" +
                "else " +
                "begin " +
                "select 'incorrect password' as 'data' " +
                "end ";

            SqlCommand command = new SqlCommand();

            //parameters
            command.Parameters.AddWithValue("@emailaddress", emailaddress);
            command.Parameters.AddWithValue("@userid", userid);
            command.Parameters.AddWithValue("@password", password);

            DataTable dt = getDataTable(query, command);
            Status st = new Status();
            if (dt.Rows.Count > 0)
            {
                st.State = 0;
                st.Exception = dt.Rows[0]["data"].ToString();
            }
            else
            {
                st.State = 1;
            }
            return st;
        }



        public static Status update_phonenumber(String userid, String phonenumber)
        { 
            String query = "if not exists(select 1 from users_tbl where  " +
                "CONVERT(Nvarchar,user_phonenumber)=@phonenumber) " +
                "begin " +
                "update users_tbl set user_phonenumber=@phonenumber where user_id=@user_id " +
                "select 1 " +
                "end ";
            SqlCommand command = new SqlCommand();

            //parameters
            command.Parameters.AddWithValue("@phonenumber", phonenumber);
            command.Parameters.AddWithValue("@user_id", userid);

            DataTable dt = getDataTable(query, command);

            Status st = new Status();

            if (dt.Rows.Count > 0)
            {
                st.State = 1;
                st.Exception = "Done";
            }
            else
            {
                st.State = 0;
                st.Exception = "Number already exists";
            }

            return st;
        }




        public static Status updateBirthDate(int user_id,String birthDate)
        {
            String query = "update users_tbl set  user_birth_date=@user_birth_date where user_id=@user_id; " +
                "select 1";
            SqlCommand command = new SqlCommand();

            //parameters
            command.Parameters.AddWithValue("@user_birth_date", birthDate);
            command.Parameters.AddWithValue("@user_id", user_id);

            DataTable dt = getDataTable(query, command);

            Status st = new Status();

            if (dt.Rows.Count > 0)
            {
                st.State = 1;
                st.Exception = "Done";
            }
          

            return st;
        }



        public static Status updateContry(int user_id, String country)
        {
            String query = "update users_tbl set  user_country=@country where user_id=@user_id; " +
                "select 1";
            SqlCommand command = new SqlCommand();

            //parameters
            command.Parameters.AddWithValue("@country", country);
            command.Parameters.AddWithValue("@user_id", user_id);

            DataTable dt = getDataTable(query, command);

            Status st = new Status();

            if (dt.Rows.Count > 0)
            {
                st.State = 1;
                st.Exception = "Done";
            }


            return st;
        }



        public static Status updateGender(int user_id, int gender)
        {
            String query = "update users_tbl set user_gender=@gender where user_id=@user_id; " +
                "select 1";
            SqlCommand command = new SqlCommand();

            //parameters
            command.Parameters.AddWithValue("@gender", gender);
            command.Parameters.AddWithValue("@user_id", user_id);

            DataTable dt = getDataTable(query, command);

            Status st = new Status();

            if (dt.Rows.Count > 0)
            {
                st.State = 1;
                st.Exception = "Done";
            }


            return st;
        }




        public static Status update_password(String userid, String current_password, String new_password)
        {
            String query = "" +
                "if(CONVERT(VARCHAR, (select user_passsword from users_tbl where user_id = @userid))=@current_password) " +
                "begin " +
                "update users_tbl set user_passsword=@new_password where user_id=@userid " +
                "end " +
                "" +
                "else " +
                "begin " +
                "select 'incorrect password' " +
                "end ";
            SqlCommand command = new SqlCommand();


            //parameters
            command.Parameters.AddWithValue("@new_password", new_password);
            command.Parameters.AddWithValue("@userid", userid);
            command.Parameters.AddWithValue("@current_password", current_password);

            DataTable dt = getDataTable(query, command);
            Status st = new Status();

            if (dt.Rows.Count > 0)
            {
                st.State = 0;
                st.Exception = "passwornd is incorrect";

            }
            else
            {
                st.State = 1;
                st.Exception = "Done";
            }

            return st;
        }






        public static Status getUserInformation(int user_id)
        {
            String query = "select user_id,user_first_name, " +
                "user_last_name,user_email,user_birth_date, " +
                "user_gender,user_profile_photo,user_country, " +
                "user_bio,user_phonenumber from users_tbl where user_id=@user_id and user_is_active=1 ";
               
            SqlCommand command = new SqlCommand();

            //parameters
            command.Parameters.AddWithValue("@user_id", user_id);

            DataTable dt = getDataTable(query, command);

            Status st = new Status();

            if (dt.Rows.Count > 0)
            {
                st.State = 1;
                st.Exception = "Done";

                string JSONresult;
                JSONresult = JsonConvert.SerializeObject(dt);
                st.Json_data = JSONresult;

            }

            return st;
        }









        private static DataTable getDataTable(string query, SqlCommand cmd)
        {
            SqlConnection con = new SqlConnection(ConnectionInit.ConnectionString);
            DataTable dt = new DataTable();
            cmd.Connection = con;
            cmd.CommandText = query;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);
            return dt;
        }

    }
}