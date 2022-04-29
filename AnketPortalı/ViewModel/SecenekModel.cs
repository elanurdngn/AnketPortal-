using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnketPortalı.ViewModel
{
    public class SecenekModel
    {
        public int SecenekId { get; set; }
        public string SecenekMetni { get; set; }
        public Nullable<int> SecenekSoruId { get; set; }
    }
}