using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.ObjectModel;

namespace PoshAsp.Models
{
    public class ClusterGroup
    {
        public string Name { get; set; }
        public Computer OwnerNode { get; set; }
        public bool Online { get; set; }
        public Collection<Computer> PossibleOwners { get; set; }
        public Collection<Computer> PreferredOwners { get; set; }
    }
}