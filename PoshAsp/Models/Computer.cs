using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Management.Automation;
using System.Collections.ObjectModel;

namespace PoshAsp.Models
{
    public class Computer
    {
        private string _Name = null;
        private Collection<IpAddress> _IpAddresses = new Collection<IpAddress>();
        private PowerShell shell = PowerShell.Create();

        public Computer(string ComputerName) {
            _Name = ComputerName;

            shell.Commands.AddScript("Invoke-Command -ComputerName " + ComputerName + " -ScriptBlock { Get-NetIpAddress | Where { $_.PrefixOrigin -ne \"WellKnown\" } }");

            foreach (PSObject result in shell.Invoke())
            {
                IpAddress IpAddress = new IpAddress();
                IpAddress.Address = result.Members["IPAddress"].Value.ToString();
                IpAddress.PrefixLength = (byte)result.Members["PrefixLength"].Value;

                if (result.Members["IPAddress"].Value.ToString() == "2")
                {
                    IpAddress.Type = "Ipv4";
                }
                else
                {
                    IpAddress.Type = "Ipv6";
                }

                _IpAddresses.Add(IpAddress);
            }
        }

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public Collection<IpAddress> IpAddresses
        {
            get { return _IpAddresses; }
            set { _IpAddresses = value; }
        }
    }
}