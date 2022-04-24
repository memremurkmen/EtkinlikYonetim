using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace EtkinlikYonetim.Models
{
    public partial class EyEtkinlikKullaniciEslesme
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid EtkinlikKullaniciEslesmeId { get; set; }
        public Guid? EtkinlikId { get; set; }
        public Guid? KullaniciId { get; set; }
        public bool girisYapildi { get; set; }

        public virtual EyEtkinlik Etkinlik { get; set; }
        [UIHint("ClientTcNo")]
        public virtual EyKullanici Kullanici { get; set; }
    }
}
