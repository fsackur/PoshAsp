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
        public HttpResponseMessage Get(string id)
        {
            try
            {
                AuthToken Token = new AuthToken(id);
                return Request.CreateResponse(HttpStatusCode.OK, Token);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, e.Message);
            }
        }

        public HttpResponseMessage Post([FromBody] User Credentials)
        {
            try
            {
                AuthToken Token = new AuthToken(Credentials.Username, Credentials.Password);
                return Request.CreateResponse(HttpStatusCode.OK, Token);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, e.Message);
            }
        }
    }
}