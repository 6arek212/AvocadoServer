using Json.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication14.Models
{
    public class UserInfo
    {
        String name;
        String profile_image;

        public string Name { get => name; set => name = value; }
        public string Profile_image { get => profile_image; set => profile_image = value; }

        public override string ToString()
        {
            return JsonNet.Serialize(this);
        }
    }
}