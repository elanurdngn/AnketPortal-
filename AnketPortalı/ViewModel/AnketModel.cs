using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnketPortalı.ViewModel
{
    public class AnketModel
    {
        public int AnketId { get; set; }
        public string Baslik { get; set; }
        public int KategoriId { get; set; }
        public string KategoriAdi { get; set; }
        public int KullaniciId { get; set; }
        public string KullaniciAdi { get; set; }
        public Nullable<int> Okunma { get; set; }
        public List<SoruModel> sorubilgi { get; set; }
    }
}