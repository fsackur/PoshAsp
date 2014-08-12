using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoshAsp.Models
{
    public class ClusterGroup
    {
        public string Name { get; set; }
        public Computer OwnerNode { get; set; }
    }
}