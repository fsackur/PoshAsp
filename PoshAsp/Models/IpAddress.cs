using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoshAsp.Models
{
    public class IpAddress
    {
        private string _Type = null;
        public string Address { get; set; }
        public byte PrefixLength { get; set; }

        public string Type
        {
            get
            {
                return _Type;
            }

            set
            {
                if (value != "Ipv4" && value != "Ipv6")
                {
                    throw new ArgumentOutOfRangeException();
                }
                else
                {
                    _Type = value;
                }
            }
        }
    }
}