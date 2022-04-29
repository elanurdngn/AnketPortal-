using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AnketPortalı.Models;
using AnketPortalı.ViewModel;

namespace AnketPortalı.Controllers
{
    public class ServisController : ApiController
    {
        DBEntities db = new DBEntities();
        SonucModel sonuc = new SonucModel();


        #region Kullanici
        [HttpGet]
        [Route("api/KullaniciListe")]
        public List<KullaniciModel> KullaniciListe()
        {
            List<KullaniciModel> liste = db.Kullanici.Select(x => new KullaniciModel()
            {
                KullaniciId = x.KullaniciId,
                AdSoyad = x.AdSoyad,
                KullaniciAdi = x.KullaniciAdi,
                Sifre = x.Sifre,
                Email = x.Email,
                KayitTarihi = x.KayitTarihi,
                KullaniciYetki = x.KullaniciYetki
            }).ToList();
            return liste;
        }
        [HttpGet]
        [Route("api/KullaniciById/{KullaniciId}")]
        public KullaniciModel KullaniciUyeById(int KullaniciId)
        {
            KullaniciModel kayit = db.Kullanici.Where(s => s.KullaniciId == KullaniciId).Select(x => new KullaniciModel()
            {
                KullaniciId = x.KullaniciId,
                AdSoyad = x.AdSoyad,
                KullaniciAdi = x.KullaniciAdi,
                Sifre = x.Sifre,
                Email = x.Email,
                KayitTarihi = x.KayitTarihi,
                KullaniciYetki = x.KullaniciYetki
            }).SingleOrDefault();
            return kayit;
        }
        [HttpPost]
        [Route("api/KullaniciEkle")]
        public SonucModel KullaniciEkle(KullaniciModel model)
        {
            if (db.Kullanici.Count(s => s.KullaniciAdi == model.KullaniciAdi || s.Email == model.Email) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen Kullanıcı Adı veya E-Posta Adresi Kayıtlıdır!";
                return sonuc;
            }
            Kullanici yeni = new Kullanici();
            yeni.AdSoyad = model.AdSoyad;
            yeni.KullaniciAdi = model.KullaniciAdi;
            yeni.Sifre = model.Sifre;
            yeni.Email = model.Email;
            yeni.KayitTarihi = model.KayitTarihi;
            yeni.KullaniciYetki = model.KullaniciYetki;
            db.Kullanici.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kullanici Eklendi";
            return sonuc;
        }
        [HttpPut]
        [Route("api/KullaniciDuzenle")]
        public SonucModel KullaniciDuzenle(KullaniciModel model)
        {
            Kullanici kayit = db.Kullanici.Where(s => s.KullaniciId == model.KullaniciId).SingleOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı";
                return sonuc;
            }
            kayit.AdSoyad = model.AdSoyad;
            kayit.KullaniciAdi = model.KullaniciAdi;
            kayit.Sifre = model.Sifre;
            kayit.Email = model.Email;
            kayit.KayitTarihi = model.KayitTarihi;
            kayit.KullaniciYetki = model.KullaniciYetki;
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kullanici Düzenlendi";
            return sonuc;
        }
        [HttpDelete]
        [Route("api/KullaniciSil/{KullaniciId}")]
        public SonucModel KullaniciSil(int KullaniciId)
        {
            Kullanici kayit = db.Kullanici.Where(s => s.KullaniciId == KullaniciId).SingleOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı";
                return sonuc;
            }
            if (db.Anket.Count(s => s.KullaniciId == KullaniciId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Üzerinde Anket Kaydı Olan Kullanıcı Silinemez!";
                return sonuc;
            }
            db.Kullanici.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kullanici Silindi";
            return sonuc;
        }
        #endregion

        #region Kategori
        [HttpGet]
        [Route("api/KategoriListe")]
        public List<KategoriModel> KategoriListe()
        {
            List<KategoriModel> liste = db.Kategori.Select(x => new KategoriModel()
            {
                KategoriId = x.KategoriId,
                KategoriAdi = x.KategoriAdi,
                KategoriAnketSay = x.Anket.Count()
            }).ToList();
            return liste;
        }
        [HttpGet]
        [Route("api/KategoriById/{KategoriId}")]
        public KategoriModel KategoriById(int KategoriId)
        {
            KategoriModel kayit = db.Kategori.Where(s => s.KategoriId == KategoriId).Select(x => new KategoriModel()
            {
                KategoriId = x.KategoriId,
                KategoriAdi = x.KategoriAdi,
                KategoriAnketSay = x.Anket.Count()
            }).FirstOrDefault();
            return kayit;
        }
        [HttpPost]
        [Route("api/KategoriEkle")]
        public SonucModel KategoriEkle(KategoriModel model)
        {
            if (db.Kategori.Count(s => s.KategoriAdi == model.KategoriAdi) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen Kategori Adı Kayıtlıdır!";
                return sonuc;
            }
            Kategori yeni = new Kategori();
            yeni.KategoriAdi = model.KategoriAdi;
            db.Kategori.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kategori Eklendi";
            return sonuc;
        }
        [HttpPut]
        [Route("api/KategoriDuzenle")]
        public SonucModel KategoriDuzenle(KategoriModel model)
        {
            Kategori kayit = db.Kategori.Where(s => s.KategoriId == model.KategoriId).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }
            kayit.KategoriAdi = model.KategoriAdi;
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kategori Düzenlendi";
            return sonuc;
        }
        [HttpDelete]
        [Route("api/KategoriSil/{KategoriId}")]
        public SonucModel KategoriSil(int KategoriId)
        {
            Kategori kayit = db.Kategori.Where(s => s.KategoriId == KategoriId).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }
            if (db.Anket.Count(s => s.KategoriId == KategoriId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Üzerinde Anket Kaydı Olan Kategori Silinemez!";
                return sonuc;
            }
            db.Kategori.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kategori Silindi";
            return sonuc;
        }
        #endregion

        #region Anket

        [HttpGet]
        [Route("api/AnketListe")]
        public List<AnketModel> AnketListe()
        {
            List<AnketModel> liste = db.Anket.Select(x => new AnketModel()
            {
                AnketId = x.AnketId,
                Baslik = x.Baslik,
                KategoriId = x.KategoriId,
                KategoriAdi = x.Kategori.KategoriAdi,
                KullaniciId = x.KullaniciId,
                KullaniciAdi = x.Kullanici.KullaniciAdi,
                Okunma = x.Okunma

            }).ToList();
            return liste;
        }
        [HttpGet]
        [Route("api/AnketListeByKatId/{kategoriId}")]
        public List<AnketModel> AnketListeByKatId(int KategoriId)
        {
            List<AnketModel> liste = db.Anket.Where(s => s.KategoriId == KategoriId).Select(x =>
           new AnketModel()
           {
               AnketId = x.AnketId,
               Baslik = x.Baslik,
               KategoriId = x.KategoriId,
               KategoriAdi = x.Kategori.KategoriAdi,
               KullaniciId = x.KullaniciId,
               KullaniciAdi = x.Kullanici.KullaniciAdi,
               Okunma = x.Okunma
           }).ToList();
            return liste;
        }
        [HttpGet]
        [Route("api/AnketById/{AnketId}")]
        public AnketModel AnketById(int AnketId)
        {
            AnketModel kayit = db.Anket.Where(s => s.AnketId == AnketId).Select(x => new AnketModel()
            {
                AnketId = x.AnketId,
                Baslik = x.Baslik,
                KategoriId = x.KategoriId,
                KategoriAdi = x.Kategori.KategoriAdi,
                KullaniciId = x.KullaniciId,
                KullaniciAdi = x.Kullanici.KullaniciAdi,
                Okunma = x.Okunma
            }).FirstOrDefault();
            return kayit;
        }

        #region Soru
        [HttpPost]
        [Route("api/SoruEkle")]
        public SonucModel SoruEkle(SoruModel model)
        {
            if (db.Soru.Count(s => s.SoruMetni == model.SoruMetni) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen Soru Kayıtlıdır!";
                return sonuc;
            }

            Soru yeni = new Soru();
            yeni.SoruMetni = model.SoruMetni;
            yeni.SoruAnketId = model.SoruAnketId;

            db.Soru.Add(yeni);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Soru Eklendi";
            return sonuc;
        }
        [HttpPut]
        [Route("api/SoruDuzenle")]
        public SonucModel SoruDuzenle(SoruModel model)
        {
            Soru kayit = db.Soru.Where(s => s.SoruId == model.SoruId).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }
            kayit.SoruMetni = model.SoruMetni;
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Soru Düzenlendi";
            return sonuc;
        }
        [HttpDelete]
        [Route("api/SoruSil/{SoruId}")]
        public SonucModel SoruSil(int SoruId)
        {
            Soru kayit = db.Soru.Where(s => s.SoruId == SoruId).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }
            db.Soru.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Soru Silindi";
            return sonuc;
        }
        [HttpGet]
        [Route("api/SoruByAnketId/{anketId}")]
        public List<SoruModel> SoruByanketId(int anketId)
        {
            List<SoruModel> kayit = db.Soru.Where(s => s.SoruAnketId == anketId).Select(x => new SoruModel()
            {
                SoruId = x.SoruId,
                SoruMetni = x.SoruMetni,
                SoruAnketId = x.SoruAnketId
        }).ToList();
            return kayit;
        }
        #endregion

        [HttpPost]
        [Route("api/AnketEkle")]
        public SonucModel AnketEkle(AnketModel model)
        {
            if (db.Anket.Count(s => s.Baslik == model.Baslik) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen Anket Başlığı Kayıtlıdır!";
                return sonuc;
            }

            Anket yeni = new Anket();
            yeni.Baslik = model.Baslik;
            yeni.KategoriId = model.KategoriId;
            yeni.KullaniciId = model.KullaniciId;
            yeni.Okunma = model.Okunma;

            db.Anket.Add(yeni);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Anket Eklendi";
            return sonuc;
        }
        [HttpGet]
        [Route("api/AnketSoruListe/{anketId}")]
        public List<AnketModel> AnketSoruListe(int anketId)
        {
            List<AnketModel> liste = db.Anket.Where(s => s.AnketId == anketId).Select(x => new AnketModel()
            {
                AnketId = x.AnketId,
                Baslik = x.Baslik,
                KategoriId = x.KategoriId,
                KategoriAdi = x.Kategori.KategoriAdi,
                KullaniciId = x.KullaniciId,
                KullaniciAdi = x.Kullanici.KullaniciAdi,
                Okunma = x.Okunma
            }).ToList();
            foreach (var Anket in liste)
            {
                Anket.sorubilgi = SoruByanketId(Anket.AnketId);
            }
            return liste;
        }
        [HttpGet] 
        [Route("api/SoruSecenekListe/{soruId}")] 
        public List<SoruModel> SorusecenekListe(int soruId) 
        { 
            List<SoruModel> liste = db.Soru.Where(s => s.SoruId == soruId).Select(x => new SoruModel()
            { 
                SoruId = x.SoruId,
                SoruMetni = x.SoruMetni,
                SoruAnketId = x.SoruAnketId, }).ToList(); 
            foreach (var Soru in liste) { 
                Soru.secenekbilgi = secenekBysoruId(Soru.SoruId); 
            } 
            return liste; 
        }

        [HttpPost]
        [Route("api/SoruSecenekEkle")]
        public SonucModel SecenekEkle(SecenekModel model)
        {
            Secenek yeni = new Secenek();
            yeni.SecenekMetni = model.SecenekMetni;
            yeni.SecenekSoruId = model.SecenekSoruId;


            db.Secenek.Add(yeni);
            db.SaveChanges();

            sonuc.islem = true;
            sonuc.mesaj = "Secenek Eklendi";
            return sonuc;
        }
        [HttpGet]
        [Route("api/SecenekBySoruId/{soruId}")]
        public List<SecenekModel> secenekBysoruId(int soruId)
        {
           List< SecenekModel> kayit = db.Secenek.Where(s => s.SecenekSoruId == soruId).Select(x => new SecenekModel()
            {
                SecenekId = x.SecenekId,
                SecenekMetni = x.SecenekMetni,
                SecenekSoruId = x.SecenekSoruId
            }).ToList();
            return kayit;
        }
        [HttpPut]
        [Route("api/AnketDuzenle")]
        public SonucModel AnketDuzenle(AnketModel model)
        {
            Anket kayit = db.Anket.Where(s => s.AnketId == model.AnketId).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }
            kayit.Baslik = model.Baslik;
            kayit.KategoriId = model.KategoriId;
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Anket Düzenlendi";
            return sonuc;
        }
        [HttpDelete]
        [Route("api/AnketSil/{AnketId}")]
        public SonucModel AnketSil(int AnketId)
        {
            Anket kayit = db.Anket.Where(s => s.AnketId == AnketId).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }
            if (db.Soru.Count(s => s.SoruAnketId == AnketId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Üzerinde Soru Kaydı Olan Anket Silinemez!";
                return sonuc;
            }
            db.Anket.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Anket Silindi";
            return sonuc;
        }

        #endregion
    }
}
