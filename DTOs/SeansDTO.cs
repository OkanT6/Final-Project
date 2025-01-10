using EruKampusSpor.Data;
using EruKampusSpor.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EruKampusSpor.DTOs
{
    public class SeansDTO
    {



        
        public int TesisId { get; set; } // Foreign Key
        public int SalonId { get; set; } // Foreign Key
        public int BransId { get; set; } // Foreign Key
        public TimeSpan SeansSaati { get; set; } // Saati tutar
        public DateTime Tarih { get; set; } // Tarihi tutar
                                            // public bool SeansRezerveEdildiMi { get; set; } // Durum bilgisi
                                            //public int? KullanıcıId { get; set; } // Nullable Foreign Key
        //public int SeansId { get; set; }
        public int Kontenjan { get; set; }

        public bool Dolu { get; set; }

        public SeansCinsiyet SeansCinsiyet { get; set; }
        public int RezerveEdenKisiSayisi { get; set; }
        
        public DateTime SeansBaslangicZamani => Tarih.Date + SeansSaati;

        //// Navigation Properties
        //[JsonIgnore]
        //public Tesis Tesis { get; set; }
        //[JsonIgnore]
        //public Salon Salon { get; set; }
        //[JsonIgnore]
        //public Brans Brans { get; set; }
        ////public Kullanıcı Kullanıcılar { get; set; }
        //[JsonIgnore]
        //public ICollection<Rezervasyon> Rezervasyonlar { get; set; }





    }
}
