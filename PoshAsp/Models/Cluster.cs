using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Management.Automation;
using System.Collections.ObjectModel;

namespace PoshAsp.Models
{
    public class Cluster
    {
        private string _Name = null;
        private Collection<Computer> _Nodes = new Collection<Computer>();
        private Collection<ClusterGroup> _ClusterGroups = new Collection<ClusterGroup>();
        private PowerShell shell = PowerShell.Create();

        public Cluster(string ClusterName) {
            _Name = ClusterName;

            shell.Commands.AddScript("Import-Module FailoverClusters");
            shell.Commands.AddScript("Get-ClusterNode -Cluster " + ClusterName);

            foreach (PSObject result in shell.Invoke())
            {
                Computer Node = new Computer(result.Members["Name"].Value.ToString());
                _Nodes.Add(Node);
            }

            shell.Commands.Clear();
            shell.Commands.AddScript("Get-ClusterGroup -Cluster " + ClusterName);

            foreach (PSObject result in shell.Invoke())
            {
                ClusterGroup ClusterGroup = new ClusterGroup();
                ClusterGroup.Name = result.Members["Name"].Value.ToString();
                ClusterGroup.OwnerNode = _Nodes.First(Node => Node.Name == result.Members["OwnerNode"].Value.ToString());
                _ClusterGroups.Add(ClusterGroup);
            }
        }

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public Collection<Computer> Nodes
        {
            get { return _Nodes; }
            set { _Nodes = value; }
        }

        public Collection<ClusterGroup> ClusterGroups
        {
            get { return _ClusterGroups; }
            set { _ClusterGroups = value; }
        }
    }
}