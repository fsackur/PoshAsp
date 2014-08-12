using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Management.Automation;

namespace PoshAsp.Controllers
{
    public class ClusterController : Controller
    {
        // GET: Cluster
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(string id)
        {
            return View(model:id);
        }
    }
}