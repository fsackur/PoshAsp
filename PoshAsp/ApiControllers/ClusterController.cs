using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PoshAsp.Models;

namespace PoshAsp.ApiControllers
{
    public class ClusterController : ApiController
    {
        [Authorize] public Cluster Get(string id)
        {
            return new Cluster(id);
        }

        public HttpResponseMessage Put(string id, Cluster DesiredState)
        {
            Cluster CurrentState = new Cluster(id);
            PowerShell shell = PowerShell.Create();

            shell.AddScript("Import-Module FailoverClusters");

            foreach (ClusterGroup DesiredClusterGroup in DesiredState.ClusterGroups)
            {
                if (DesiredClusterGroup.OwnerNode.Name != CurrentState.ClusterGroups.First(CurrentClusterGroup => CurrentClusterGroup.Name == DesiredClusterGroup.Name).OwnerNode.Name)
                {
                    shell.AddScript("Move-ClusterGroup -Name \"" + DesiredClusterGroup.Name + "\" -Node \"" + DesiredClusterGroup.OwnerNode.Name + "\" -Cluster \"" + id + "\"");
                }
            }

            shell.Invoke();
            return Request.CreateResponse(HttpStatusCode.OK, Get(DesiredState.Name));
        }
    }
}
