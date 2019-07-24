using Json.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication14.Models
{
    public class Message
    {

        private int message_id;
        private int message_sender_id;
        private String message_text;
        private DateTime message_datetime;


        public Message()
        {

        }

        public Message(int message_id, int message_sender_id, string message_text, DateTime message_datetime)
        {
            this.Message_id = message_id;
            this.Message_sender_id = message_sender_id;
            this.Message_text = message_text;
            this.Message_datetime = message_datetime;
        }

        public int Message_id { get => message_id; set => message_id = value; }
        public int Message_sender_id { get => message_sender_id; set => message_sender_id = value; }
        public string Message_text { get => message_text; set => message_text = value; }
        public DateTime Message_datetime { get => message_datetime; set => message_datetime = value; }





        public override string ToString()
        {
            return JsonNet.Serialize(this);
        }

    }
}