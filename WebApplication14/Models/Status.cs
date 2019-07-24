using Json.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication14.Controllers
{
    public class Status
    {
        private int state;
        private string exception;
        private String json_data;

        public Status()
        {
        }

        public Status(int state, string exception, string json_data)
        {
            this.state = state;
            this.exception = exception;
            this.json_data = json_data;
        }

        public int State { get => state; set => state = value; }
        public string Exception { get => exception; set => exception = value; }
        public string Json_data { get => json_data; set => json_data = value; }

        public override string ToString()
        {
            return JsonNet.Serialize(this);
        }
    }
}