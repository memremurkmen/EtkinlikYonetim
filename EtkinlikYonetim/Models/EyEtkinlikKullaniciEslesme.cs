using System;
using System.Collections.Generic;

#nullable disable

namespace EtkinlikYonetim.Models
{
    public partial class EyEtkinlikKullaniciEslesme
    {
        public int EtkinlikKullaniciEslesmeId { get; set; }
        public int? EtkinlikId { get; set; }
        public int? KullaniciId { get; set; }
        public bool girisYapildi { get; set; }

        public virtual EyEtkinlik Etkinlik { get; set; }
        public virtual EyKullanici Kullanici { get; set; }
    }
}
