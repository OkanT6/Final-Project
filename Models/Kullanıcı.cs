using System.Text.Json.Serialization;

namespace EruKampusSpor.Models
{
    public class Kullanıcı
    {
        public Kullanıcı()
        {
            Seanslar = new HashSet<Seans>();
        }

        public int KullanıcıId { get; set; }
        public string TC { get; set; } 
        public string Name { get; set; }


        [JsonIgnore]
        public Adres Adres { get; set; }

        [JsonIgnore]
        public ICollection<Seans> Seanslar { get; set; } // Seanslarla ilişki
    }
}
