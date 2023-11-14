using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReniAPI.Errors
{
    public class ApiException : ApiResponse
    {
        public ApiException(int statusCode, string message =null, string _details = null) : base
        (statusCode, message)
        {
            Details = _details;
        }
        
        public string Details {get; set;}
    }
}