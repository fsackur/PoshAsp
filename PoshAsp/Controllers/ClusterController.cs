using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PoshAsp.Models;

namespace PoshAsp.Controllers
{
    public class ClusterController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Details(string id)
        {
            return View(model:id);
        }

        [Authorize]
        public ActionResult History(string id)
        {
            LogEntries FilteredLog = new LogEntries("clusterlog");

            if(! String.IsNullOrEmpty(id) )
            {
                FilteredLog.RemoveAll(e => ((Cluster)e.Data["Before"]).Name.ToUpper() != id.ToUpper());
            }
            
            return View(FilteredLog);
        }
    }
}