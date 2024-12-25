namespace EruKampusSpor.Models
{
    
        public class Seans
        {
            public int TesisId { get; set; } // Foreign Key
            public int SalonId { get; set; } // Foreign Key
            public int BransId { get; set; } // Foreign Key
            public TimeSpan SeansSaati { get; set; } // Saati tutar
            public DateTime Tarih { get; set; } // Tarihi tutar
            public bool SeansRezerveEdildiMi { get; set; } // Durum bilgisi
            public int? KullanıcıId { get; set; } // Nullable Foreign Key

            // Navigation Properties
            public Tesis Tesis { get; set; }
            public Salon Salon { get; set; }
            public Brans Brans { get; set; }
            public Kullanıcı? Kullanıcı { get; set; }
        }
    
}
