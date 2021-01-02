using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
//using System.Text.Json.Serialization;

namespace Website.Models.Exception
{
    // able to return api response code to identify potential issues
    public class ApiException
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}

