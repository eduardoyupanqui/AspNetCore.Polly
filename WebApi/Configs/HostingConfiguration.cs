using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Configs
{
    public class HostingConfiguration
    {
        public bool Error404 { get; set; }
        public bool Error400 { get; set; }
        public bool Error401 { get; set; }
        public bool Error403 { get; set; }
        public bool Error500 { get; set; }
    }
}
