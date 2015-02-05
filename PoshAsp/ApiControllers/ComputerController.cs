using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PoshAsp.Filters;
using PoshAsp.Models;

namespace PoshAsp.ApiControllers
{
    public class ComputerController : ApiController
    {
        [ApiAuth]
        [Authorize]
        public Computer Get(string id)
        {
            return new Computer(id);
        }
    }
}
