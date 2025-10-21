using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wisegar.Toolkit.Models.MSGraph
{
    public class MSGraphSettings
    {
        public const string SectionName = "MSGraph";
        public string Principal { get; set; } = string.Empty;
        public string TenantId { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
    }
}
