using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnketPortalı.ViewModel
{
    public class SoruModel
    {
        public int SoruId { get; set; }
        public string SoruMetni { get; set; }
        public Nullable<int> SoruAnketId { get; set; }
        public List<SecenekModel> secenekbilgi { get; set; }


    }
}