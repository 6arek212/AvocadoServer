using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication14.Controllers.Methods;

namespace WebApplication14.Controllers
{
    public class ConnectionsController : ApiController
    {
        [Route("api/Connections/getInCommeingConnectionsRequest")]
        [HttpGet]
        public Status getInCommeingConnectionsRequest(int User_id, int offset,String datetime)
        {
            return ConnectionMethods.onGettingIncommingConnectionsRequest(User_id, offset, datetime);
        }


        [Route("api/Connections/searchUsersByName")]
        [HttpGet]
        public Status searchUsersByName(int User_id, String Text_cmp,String datetime , int offset)
        {
            if (Text_cmp == null)
                return ConnectionMethods.onSearchingUserByName(User_id, "", datetime, offset);
            return ConnectionMethods.onSearchingUserByName(User_id, Text_cmp, datetime, offset);
        }



        [Route("api/Connections/deleteFriendRequest")]
        [HttpGet]
        public Status deleteFriendRequest(int Request_id,int user_id)
        {
            return ConnectionMethods.onDeleteFriendRequest(Request_id,user_id);
        }



        [Route("api/Connections/deleteFriend")]
        [HttpPost]
        public Status deleteFriend(int Request_id,int u1,int u2)
        {
            return ConnectionMethods.onDeleteFriend(Request_id,u1,u2);
        }




        [Route("api/Connections/sendFriendRequest")]
        [HttpGet]
        public Status sendFriendRequest(int Sender_id, int Receiver_id, String Date_time_sent)
        {
            return ConnectionMethods.onSendingNewFriemdRequest(Sender_id, Receiver_id, Date_time_sent);
        }


        [Route("api/Connections/accepteFriendRequest")]
        [HttpGet]
        public Status accepteFriendRequest(int Request_id, String Date_time_accepted)
        {
            return ConnectionMethods.onAcceptingFriendRequest(Request_id, Date_time_accepted);
        }


        [Route("api/Connections/getFriends")]
        [HttpGet]
        public Status getFriends(int user_id,int offset,String datetime)
        {
            return ConnectionMethods.getFriends(user_id, offset,datetime);
        }

    }
}
