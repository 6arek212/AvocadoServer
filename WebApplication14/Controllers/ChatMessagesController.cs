using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication14.Controllers.Methods;

namespace WebApplication14.Controllers
{
    public class ChatMessagesController : ApiController
    {



        [Route("api/ChatMessages/GettingUnreadedMessages")]
        [HttpGet]
        public Status GettingUnreadedMessages(int User_id, int Chat_id)
        {
            return ChatMethods.OnGettingUnreadedMessages(User_id, Chat_id);
        }


        [Route("api/ChatMessages/GettingUserChatsWithUpdatedValues")]
        [HttpGet]
        public Status GettingUserChatsWithUpdatedValues(int User_id)
        {
            return ChatMethods.OnGettingUserChatsWithUpdatedValues(User_id);

        }


        [Route("api/ChatMessages/deleteChat")]
        [HttpPost]
        public Status deleteChat(int chat_id, int user_id)
        {
            return ChatMethods.OnDeletingChat(chat_id,user_id);
        }


        [Route("api/ChatMessages/GettingChats")]
        [HttpGet]
        public Status GettingChats(int User_id)
        {
            return ChatMethods.OnGettingChats(User_id);
        }


        [Route("api/ChatMessages/SendingMessage")]
        [HttpGet]
        public Status SendingMessage(int Chat_id, int User_id, String Message, String Datetime)
        {
            return ChatMethods.OnSendingMessage(Chat_id, User_id, Message, Datetime);
        }



        [Route("api/ChatMessages/CheckingCreatingChat")]
        [HttpGet]
        public Status CheckingCreatingChat(int Sender_id, int Receiver_id, String Datetime, String Message)
        {
            return ChatMethods.OnCheckingCreatingChat(Sender_id, Receiver_id, Datetime, Message);
        }



        [Route("api/ChatMessages/GettingUserAndChats")]
        [HttpGet]
        public Status GettingUserAndChats(int User_id, String Username, int Offset)
        {
            if (Username == null)
                Username = "";
            return ChatMethods.OnGettingUserAndChats(User_id, Username, Offset);
        }


    }
}
