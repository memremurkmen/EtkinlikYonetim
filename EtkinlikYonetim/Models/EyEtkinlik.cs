using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace EtkinlikYonetim.Models
{
    public partial class EyEtkinlik
    {
        public EyEtkinlik()
        {
            EyEtkinlikKullaniciEslesmes = new HashSet<EyEtkinlikKullaniciEslesme>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ScaffoldColumn(false)]
        public Guid EtkinlikId { get; set; }

        [StringLength(200, ErrorMessage = "Etkinlik adı 200 harfi geçemez")]
        [Required(ErrorMessage = "Lütfen bir ad giriniz")]
        public string EtkinlikAdi { get; set; }
        [Required(ErrorMessage = "Lütfen max katılımcı sayısı giriniz")]
        public int? MaxKatilimciSayisi { get; set; }
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Lütfen bir başlangıç tarihi giriniz")]
        public DateTime? BaslangicTarihi { get; set; }
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Lütfen bir bitiş tarihi giriniz")]
        public DateTime? BitisTarihi { get; set; }
        public DateTime? updateDate { get; set; }
        public DateTime? deleteDate { get; set; }
        public bool isDeleted { get; set; }

        public virtual ICollection<EyEtkinlikKullaniciEslesme> EyEtkinlikKullaniciEslesmes { get; set; }
    }
}
