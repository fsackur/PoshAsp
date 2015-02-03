using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PoshAsp.Models;

namespace PoshAsp.ApiControllers
{
    public class AuthController : ApiController
    {
        public bool Get(string AuthToken)
        {
            return true;
        }

        public HttpResponseMessage Post(string Username, string Password)
        {
            try
            {
                AuthToken Token = new AuthToken(Username, Password);
                return Request.CreateResponse(HttpStatusCode.OK, Token);
            }
            catch(Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden, e.Message);
            }
        }
    }
}