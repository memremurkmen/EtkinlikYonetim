using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EtkinlikYonetim.Models
{
    public class EtkinlikDuzenleModel
    {
        public List<EyEtkinlikKullaniciEslesme> activitiesUsers { get; set; }
        public EyEtkinlik activity { get; set; }
        public List<EyKullanici> users { get; set; }
    }
}
