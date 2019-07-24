using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication14.Controllers.Methods
{
    public class HomeController : ApiController
    {

        //  ------------------------ > Creating tables  Methods <----------------------------//

        //users table
        [Route("api/Home/createUsersTable")]
        [HttpGet]
        public Status createUsersTable()
        {
            string query = "create table users_tbl(" +
                "user_id int primary key identity (1,1)," +
                "user_first_name text ," +
                "user_last_name text , " +
                "user_email text ," +
                "user_passsword text," +
                "user_birth_date date," +
                "user_gender int ," +
                "user_profile_photo text," +
                "user_posts_count int," +
                "user_photos_count int," +
                "user_connection_count int," +
                "user_reputation int ," +
                "user_current_location  geography," +
                "user_city text," +
                "user_country text ," +
                "user_job text , " +
                "user_location_swich bit, " +
                "account_is_private bit," +
                "user_is_online bit ," +
                "user_is_active bit," +
                "notification_count int," +
                "unread_messages_count int," +
                "connection_request_count int)";


            return ConnectionInit.excuteQuery(query);
        }



        //posts table
        [Route("api/Home/createPostsTable")]
        [HttpGet]
        public Status createPostsTable()
        {
            string query = "create table posts_tbl(" +
                "post_id int primary key identity(1,1)," +
                "user_id int references users_tbl(user_id)," +
                "post_text NTEXT," +
                "post_image_path text ," +
                "post_date_time datetime," +
                "post_type int, " +
                "post_comments_count int ," +
                "post_likes_count int ," +
                "post_dislike_count int ," +
                "post_reports_count int ," +
                "post_share_count int ," +
                "post_is_shared bit," +
                "original_post_id int references posts_tbl(post_id) )";


            return ConnectionInit.excuteQuery(query);
        }



        //likes table
        [Route("api/Home/createLikesTable")]
        [HttpGet]
        public Status createLikesTable()
        {
            string query = "create table likes_tbl(" +
                "like_id int primary key identity(1,1), " +
                "user_id int references users_tbl(user_id), " +
                "post_id int references posts_tbl(post_id)," +
                "like_datetime datetime)";

            return ConnectionInit.excuteQuery(query);
        }


        //likes triggers
        [Route("api/Home/createLikesTrigger")]
        [HttpGet]
        public Status createLikesTrigger()
        {

            string query = "create trigger OnLike " +
                "on likes_tbl " +
                "after insert " +
                "as " +
                "declare @post_id int " +
                "set @post_id=(select post_id from inserted) " +
                "update posts_tbl set post_likes_count=post_likes_count+1 where post_id=@post_id";

            return ConnectionInit.excuteQuery(query);

        }


        [Route("api/Home/createRemoveLikeTrigger")]
        [HttpGet]
        public Status createRemoveLikeTrigger()
        {

            string query = "create trigger OnRemoveLike " +
                "on likes_tbl " +
                "after delete " +
                "as " +
                "declare @post_id int " +
                "set @post_id=(select post_id from deleted) " +
                "update posts_tbl set post_likes_count=post_likes_count-1 where post_id=@post_id";

            return ConnectionInit.excuteQuery(query);

        }




        //dislikes table
        [Route("api/Home/createDisLikesTable")]
        [HttpGet]
        public Status createDisLikesTable()
        {
            string query = "create table dis_likes_tbl(" +
                "dis_like_id int primary key identity(1,1), " +
                "user_id int references users_tbl(user_id), " +
                "post_id int references posts_tbl(post_id)," +
                "dis_like_datetime datetime)";

            return ConnectionInit.excuteQuery(query);
        }


        //Dislikes triggers
        [Route("api/Home/createDisLikesTrigger")]
        [HttpGet]
        public Status createDisLikesTrigger()
        {

            string query = "create trigger OnDisLike " +
                "on dis_likes_tbl " +
                "after insert " +
                "as " +
                "declare @post_id int " +
                "set @post_id=(select post_id from inserted) " +
                "update posts_tbl set post_dislike_count=post_dislike_count+1 where post_id=@post_id";

            return ConnectionInit.excuteQuery(query);
        }



        [Route("api/Home/createRemoveDisLikeTrigger")]
        [HttpGet]
        public Status createRemoveDisLikeTrigger()
        {

            string query = "create trigger OnRemoveDisLike " +
                "on dis_likes_tbl " +
                "after delete " +
                "as " +
                "declare @post_id int " +
                "set @post_id=(select post_id from deleted) " +
                "update posts_tbl set post_dislike_count=post_dislike_count-1 where post_id=@post_id";

            return ConnectionInit.excuteQuery(query);

        }




        //connections table
        [Route("api/Home/createFriendsTable")]
        [HttpGet]
        public Status createFriendsTable()
        {
            string query = "create table friends_tbl(" +
                "request_id int primary key identity(1,1)," +
                "sender_id int references users_tbl(user_id)," +
                "receiver_id int references users_tbl(user_id)," +
                "date_time_sent datetime ," +
                "date_time_accepted datetime," +
                "is_accepted bit)";


            return ConnectionInit.excuteQuery(query);
        }



        //comments table
        [Route("api/Home/createCommentsTable")]
        [HttpGet]
        public Status createCommentsTable()
        {
            string query = "create table comments_tbl(" +
                "comment_id int primary key identity(1,1)," +
                "post_id int references posts_tbl(post_id)," +
                "user_id int references users_tbl(user_id)," +
                "comment_text NTEXT," +
                "comment_date_time datetime)";

            return ConnectionInit.excuteQuery(query);
        }




        //comments_tbl where comment_id=@@IDENTITY
        [Route("api/Home/createTriggerForAddingComment")]
        [HttpGet]
        public Status createTriggerForAddingComment()
        {
            string query = "create trigger triggerAddComment " +
                "on comments_tbl " +
                "after insert " +
                "as " +
                "declare @post_id int " +
                "set @post_id=(select post_id from inserted) " +
                "update posts_tbl set post_comments_count=post_comments_count+1 where post_id=@post_id ";

            return ConnectionInit.excuteQuery(query);
        }



        //chat table
        [Route("api/Home/createChatsTable")]
        [HttpGet]
        public Status createChatsTable()
        {
            string query = "create table chats_tbl(" +
                "chat_id int primary key identity(1,1)," +
                "chat_sender_id int references users_tbl(user_id)," +
                "chat_receiver_id int references users_tbl(user_id)," +
                "chat_datetime_created datetime ," +
                "chat_messages_count int ," +
                "chat_last_message_user_id int references users_tbl(user_id)," +
                "chat_last_message NTEXT, " +
                "chat_last_message_datetime datetime ," +
                "chat_removed_sender bit," +
                "chat_removed_receiver bit," +
                "sender_not_read int ," +
                "receiver_not_read int)";

            return ConnectionInit.excuteQuery(query);
        }




        //messages table
        [Route("api/Home/createMessagesTable")]
        [HttpGet]
        public Status createMessagesTable()
        {
            string query = "create table chat_messages_tbl(" +
                "message_id int primary key identity(1,1)," +
                "chat_id int references chats_tbl(chat_id)," +
                "message_sender_id int references users_tbl(user_id)," +
                "message_text NTEXT," +
                "message_datetime datetime)";


            return ConnectionInit.excuteQuery(query);
        }



        // chat_messages_tbl where message_id=@@IDENTITY
        [Route("api/Home/createTriggerOnSenddingMessage")]
        [HttpGet]
        public Status createTriggerOnSenddingMessage()
        {
            string query = "create trigger OnSenddingMessage " +
                "on chat_messages_tbl " +
                "after insert " +
                "as " +
                "declare @chat_id int,@message_sender_id int,@message_id int " +
                "set @message_id =(select message_id from inserted) " +
                "set @message_sender_id =(select message_sender_id from chat_messages_tbl where message_id=@message_id) " +
                "set @chat_id =(select chat_id from chat_messages_tbl where message_id=@message_id) " +
                "update chats_tbl set chat_messages_count=chat_messages_count+1 " +
                "where chat_id=@chat_id " +
                "" +
                "if(@message_sender_id = (select chat_sender_id from chats_tbl where chat_id=@chat_id)) " +
                "begin " +
                "update chats_tbl set receiver_not_read=receiver_not_read+1 " +
                "where chat_id=@chat_id " +
                "end " +
                "" +
                "else " +
                "begin " +
                "update chats_tbl set sender_not_read=sender_not_read+1 where chat_id=@chat_id " +
                "end " +
                "" +
                "update chats_tbl set chat_removed_sender=0 " +
                ", chat_removed_receiver=0 " +
                ", chat_last_message_datetime=(select chat_messages_tbl.message_datetime from chat_messages_tbl where message_id=@message_id) " +
                ",chat_last_message_user_id=@message_sender_id ,chat_last_message =(select message_text from chat_messages_tbl where message_id=@message_id) " +
                "where chat_id=@chat_id ";

            return ConnectionInit.excuteQuery(query);
        }



        //notification table
        [Route("api/Home/createNotificationTable")]
        [HttpGet]
        public Status createNotificationTable()
        {
            string query = "create table notification_tbl(notification_id int primary key identity(1,1)," +
                "user_id_sent_notification int references users_tbl(user_id)," +
                "user_id_received_notification int references users_tbl(user_id)," +
                "notification_type int references notifiaction_type_tbl(type_id)," +
                "notification_datetime datetime)";


            return ConnectionInit.excuteQuery(query);
        }



        //notification Type table
        [Route("api/Home/createNotificationTypeTable")]
        [HttpGet]
        public Status createNotificationTypeTable()
        {
            string query = "create table notifiaction_type_tbl(type_id int  primary key identity (1,1),type_name text)";
            return ConnectionInit.excuteQuery(query);
        }

    }
}
