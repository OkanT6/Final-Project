using EruKampusSpor.Models;

namespace EruKampusSpor.DTOs
{
    public class SeansDTO
    {
        public int TesisId { get; set; } // Foreign Key
        public int SalonId { get; set; } // Foreign Key
        public int BransId { get; set; } // Foreign Key
        public TimeSpan SeansSaati { get; set; } // Saati tutar
        public DateTime Tarih { get; set; } // Tarihi tutar
        public bool SeansRezerveEdildiMi { get; set; } // Durum bilgisi
        public int? KullanıcıId { get; set; } // Nullable Foreign Key

        
    }
}
