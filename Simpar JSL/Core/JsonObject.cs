using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Simpar_JSL.Core
{
    public class JsonObject
    {
        public JsonObject(bool success, string message, string type, string data)
        {
            Success = success;
            Message = message;
            Type = type;
            Data = data;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public string Data { get; set; }
    }
}