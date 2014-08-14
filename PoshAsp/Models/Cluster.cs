using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Management.Automation;
using System.Collections;
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
                ClusterGroup.Online = result.Members["State"].Value.ToString() == "Online";
                _ClusterGroups.Add(ClusterGroup);
            }

            foreach (ClusterGroup ClusterGroup in _ClusterGroups)
            {
                Collection<Computer> PossibleOwners = new Collection<Computer>();

                if (ClusterGroup.Name == "Available Storage" || ClusterGroup.Name == "Cluster Group")
                {
                    PossibleOwners = _Nodes;
                }
                else
                {
                    Dictionary<string,int> PossibleOwnerNodes = new Dictionary<string,int>();

                    shell.Commands.Clear();
                    shell.Commands.AddScript("Get-ClusterGroup \"" + ClusterGroup.Name + "\" -Cluster \"" + ClusterName + "\" | Get-ClusterResource");

                    int ResourceCount = shell.Invoke().Count;
                    
                    shell.Commands.Clear();
                    shell.Commands.AddScript("Get-ClusterGroup \"" + ClusterGroup.Name + "\" -Cluster \"" + ClusterName + "\" | Get-ClusterResource | Get-ClusterOwnerNode | ForEach { $_.OwnerNodes }");

                    foreach (PSObject result in shell.Invoke())
                    {
                        string PossibleOwnerNode = result.Members["Name"].Value.ToString().ToLower();

                        if (PossibleOwnerNodes.ContainsKey(PossibleOwnerNode))
                        {
                            PossibleOwnerNodes[PossibleOwnerNode] += 1;
                        }
                        else
                        {
                            PossibleOwnerNodes.Add(PossibleOwnerNode, 1);
                        }
                    }

                    foreach (Computer Node in _Nodes)
                    {
                        if (PossibleOwnerNodes.ContainsKey(Node.Name.ToLower()) && PossibleOwnerNodes[Node.Name.ToLower()] == ResourceCount)
                        {
                            PossibleOwners.Add(Node);
                        }
                    }
                }

                shell.Commands.Clear();
                shell.Commands.AddScript("Get-ClusterGroup \"" + ClusterGroup.Name + "\" -Cluster \"" + ClusterName + "\" | Get-ClusterOwnerNode | ForEach { $_.OwnerNodes }");

                _ClusterGroups.First(SelectedClusterGroup => SelectedClusterGroup.Name == ClusterGroup.Name).PossibleOwners = PossibleOwners;

                foreach (PSObject result in shell.Invoke())
                {
                    if (ClusterGroup.PreferredOwners == null)
                    {
                        ClusterGroup.PreferredOwners = new Collection<Computer>();
                    }

                    ClusterGroup.PreferredOwners.Add(_Nodes.First(SelectedNode => SelectedNode.Name.ToLower() == result.Members["Name"].Value.ToString().ToLower()));
                }
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