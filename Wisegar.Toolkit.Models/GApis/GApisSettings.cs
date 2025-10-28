using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wisegar.Toolkit.Models.GApis
{
    public class GApisSettings
    {
        public const string SectionName = "GApis";

        public string JsonPath { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }
}
