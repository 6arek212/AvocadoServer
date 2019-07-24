using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication14.Controllers.Methods;

namespace WebApplication14.Controllers
{
    public class LoginRegisterInfoController : ApiController
    {
        



        //  ------------------------ > Login & Register Methods <----------------------------//

        [Route("api/LoginRegisterInfo/registerNewUser")]
        [HttpGet]
        public Status registerNewUser(string User_first_name,String user_last_name, string User_Email, string User_password,String register_datetime)
        {
            Status status = new Status();
            status =LoginRegisterMethods.checkIfEmailExists(User_Email);

            if (status.State == 1)
            {
                status.State = 0;
                status.Exception = "Email already been used  " + User_Email;
                return status;
            }
            else if (status.State == -1)
            {
                status.Exception = "check your email and pasword";
                return status;
            }
            else
            {
                return LoginRegisterMethods.OnInsertingNewUser(User_first_name,user_last_name, User_Email, User_password, register_datetime);
            }
        }





        [Route("api/LoginRegisterInfo/checkIfEmailExists")]
        [HttpGet]
        public Status checkIfEmailExists(string User_Email)
        {
            return LoginRegisterMethods.checkIfEmailExists(User_Email);
        }





        [Route("api/LoginRegisterInfo/loginWithEmailAndPassword")]
        [HttpGet]
        public Status loginWithEmailAndPassword(string User_Email, string User_password)
        {
            return LoginRegisterMethods.checkIfExists(User_Email, User_password);
        }





        //setting ///////////////////


        [Route("api/LoginRegisterInfo/userOnline")]
        [HttpPost]
        public Status userOnline(int user_id,bool state)
        {
            return LoginRegisterMethods.userOnline(user_id, state);
        }




        [Route("api/LoginRegisterInfo/locationSettings")]
        [HttpPost]
        public Status settings(int user_id, bool state,int type)
        {
            return LoginRegisterMethods.settings(user_id,state,type);
        }



        [Route("api/LoginRegisterInfo/locationSettings")]
        [HttpPost]
        public Status getSettings(int user_id)
        {
            return LoginRegisterMethods.getSettings(user_id);
        }



        [Route("api/LoginRegisterInfo/deleteAccount")]
        [HttpPost]
        public Status deleteAccount(int user_id,String password)
        {
            return LoginRegisterMethods.deleteAccount(user_id, password);
        }




        //  ------------------------ > Edit User info Methods <----------------------------//


        [Route("api/LoginRegisterInfo/updateProfilePhotoGenderBirthDate")]
        [HttpGet]
        public Status updateProfilePohotoGenderBirthDate(int User_id,String image_path, int User_gender, String User_birth_date ,String user_country)
        {
            return ConnectionInit.updateProfilePohotoGenderBirthDate(User_id, image_path, User_gender, User_birth_date, user_country);
        }


        [Route("api/LoginRegisterInfo/updateCityCountry")]
        [HttpGet]
        public Status updateCityCountry(int User_id, String User_country, String User_city)
        {
            return ConnectionInit.updateCityCountry(User_id, User_country, User_city);
        }



        [Route("api/LoginRegisterInfo/updateProfilePhoto")]
        [HttpGet]
        public Status updateProfilePhoto(int User_id, String image_path)
        {
            return ConnectionInit.updateProfileImage(User_id, image_path);
        }




        [Route("api/LoginRegisterInfo/getUserInfo")]
        [HttpGet]
        public Status getUserInfo(int user_id)
        {
            return ConnectionInit.getUserInfo(user_id);
        }

    }
}
