using Json.Net;
using System;
using System.Collections.Generic;
using System.Data;
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

        public Message(DataRow dr)
        {
            int.TryParse(dr["message_id"].ToString(), out message_id);
            int.TryParse(dr["message_sender_id"].ToString(), out message_sender_id);
            this.Message_text = dr["message_text"].ToString();
            this.Message_datetime = DateTime.Parse(dr["message_datetime"].ToString());
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