using Json.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication14.Models
{
    public class Chat
    {


        private String user_first_name;
        private String user_last_name;
        private String user_profile_photo;
        private int chat_id;
        private int chat_sender_id;
        private int chat_receiver_id;
        private DateTime chat_datetime_created;
        private int chat_messages_count;
        private int chat_last_message_user_id;
        private String chat_last_message;
        private DateTime chat_last_message_datetime;
        private int sender_not_read;
        private int receiver_not_read;


        public Chat()
        {

        }

        public Chat(string user_first_name, string user_last_name,
            string user_profile_photo, int chat_id, int chat_sender_id,
            int chat_receiver_id, DateTime chat_datetime_created, int chat_messages_count,
            int chat_last_message_user_id, string chat_last_message, DateTime chat_last_message_datetime,
            int sender_not_read, int receiver_not_read)
        {
            this.User_first_name = user_first_name;
            this.User_last_name = user_last_name;
            this.User_profile_photo = user_profile_photo;
            this.chat_id = chat_id;
            this.chat_sender_id = chat_sender_id;
            this.chat_receiver_id = chat_receiver_id;
            this.chat_datetime_created = chat_datetime_created;
            this.chat_messages_count = chat_messages_count;
            this.chat_last_message_user_id = chat_last_message_user_id;
            this.chat_last_message = chat_last_message;
            this.chat_last_message_datetime = chat_last_message_datetime;
            this.sender_not_read = sender_not_read;
            this.receiver_not_read = receiver_not_read;
        }

        public int Chat_id { get => chat_id; set => chat_id = value; }
        public int Chat_sender_id { get => chat_sender_id; set => chat_sender_id = value; }
        public int Chat_receiver_id { get => chat_receiver_id; set => chat_receiver_id = value; }
        public DateTime Chat_datetime_created { get => chat_datetime_created; set => chat_datetime_created = value; }
        public int Chat_messages_count { get => chat_messages_count; set => chat_messages_count = value; }
        public int Chat_last_message_user_id { get => chat_last_message_user_id; set => chat_last_message_user_id = value; }
        public string Chat_last_message { get => chat_last_message; set => chat_last_message = value; }
        public DateTime Chat_last_message_datetime { get => chat_last_message_datetime; set => chat_last_message_datetime = value; }
        public int Sender_not_read { get => sender_not_read; set => sender_not_read = value; }
        public int Receiver_not_read { get => receiver_not_read; set => receiver_not_read = value; }
        public string User_first_name { get => user_first_name; set => user_first_name = value; }
        public string User_last_name { get => user_last_name; set => user_last_name = value; }
        public string User_profile_photo { get => user_profile_photo; set => user_profile_photo = value; }

        public override string ToString()
        {
            return JsonNet.Serialize(this);
        }
    }
}