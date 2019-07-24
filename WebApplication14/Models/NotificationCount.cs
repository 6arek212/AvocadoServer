using Json.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication14.Models
{
    public class NotificationCount
    {
        private int notification_count;
        private int unread_messages_count;

        public NotificationCount(int notification_count, int unread_messages_count)
        {
            this.Notification_count = notification_count;
            this.Unread_messages_count = unread_messages_count;
        }

        public int Notification_count { get => notification_count; set => notification_count = value; }
        public int Unread_messages_count { get => unread_messages_count; set => unread_messages_count = value; }

        public override string ToString()
        {
            return JsonNet.Serialize(this);
        }
    }
}