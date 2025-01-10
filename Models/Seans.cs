using EruKampusSpor.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EruKampusSpor.Models
{
    
        public class Seans
        {

        public Seans()
        {
            Rezervasyonlar = new HashSet<Rezervasyon>();
        }
        public int TesisId { get; set; } // Foreign Key
            public int SalonId { get; set; } // Foreign Key
            public int BransId { get; set; } // Foreign Key
            public TimeSpan SeansSaati { get; set; } // Saati tutar
            public DateTime Tarih { get; set; } // Tarihi tutar
           // public bool SeansRezerveEdildiMi { get; set; } // Durum bilgisi
            //public int? KullanıcıId { get; set; } // Nullable Foreign Key
            public int SeansId {  get; set; } //Alternate Key
            public int Kontenjan {  get; set; }

            [NotMapped] // Bu property EF Core tarafından veritabanına kaydedilmeyecek
            public DateTime SeansBaslangicZamani => Tarih.Date + SeansSaati;
        public bool Dolu {  get; set; }

            public SeansCinsiyet SeansCinsiyet { get; set; }

            public int RezerveEdenKisiSayisi {  get; set; }

        // Navigation Properties
        [JsonIgnore]
        public Tesis Tesis { get; set; }
        [JsonIgnore]
        public Salon Salon { get; set; }
        [JsonIgnore]
        public Brans Brans { get; set; }
        //public Kullanıcı Kullanıcılar { get; set; }
        [JsonIgnore]
            public ICollection<Rezervasyon> Rezervasyonlar { get; set; }


    }
    
}
