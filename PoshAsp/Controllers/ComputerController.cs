using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PoshAsp.Models;

namespace PoshAsp.Controllers
{
    public class ComputerController : Controller
    {
        //
        // GET: /Computer/
        public ActionResult Index()
        {
            return View();
        }
    }
}