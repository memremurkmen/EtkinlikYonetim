using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace EtkinlikYonetim.Models
{
    public partial class EyKullanici
    {
        public EyKullanici()
        {
            EyEtkinlikKullaniciEslesmes = new HashSet<EyEtkinlikKullaniciEslesme>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ScaffoldColumn(false)]
        public Guid KullaniciId { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Lütfen bir ad giriniz.")]
        public string Ad { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "Lütfen bir soyad giriniz.")]
        public string Soyad { get; set; }
        [RegularExpression("([0-9]+)", ErrorMessage = "Telefon numaranız harf içeremez!")]
        [StringLength(10, ErrorMessage = "Telefon No 10 hane olacak şekilde giriniz.", MinimumLength = 10)]
        [Required(ErrorMessage = "Lütfen bir telefon no giriniz")]
        public string TelefonNo { get; set; }

        [RegularExpression("([0-9]+)", ErrorMessage = "TC numaranız harf içeremez!")]
        [StringLength(11, ErrorMessage = "TC No 11 hane olmalıdır", MinimumLength = 11)]
        [Required(ErrorMessage = "Lütfen bir tc no giriniz")]
        public string TcNo { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "Lütfen bir şifre giriniz")]
        public string Sifre { get; set; }
        [Required(ErrorMessage = "Lütfen bir yetki seçiniz")]
        public string Yetki { get; set; }

        public virtual ICollection<EyEtkinlikKullaniciEslesme> EyEtkinlikKullaniciEslesmes { get; set; }
    }
}
