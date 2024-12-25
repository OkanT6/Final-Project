using System.Text.Json.Serialization;

namespace EruKampusSpor.Models
{
    public class Adres
    {
        public int Id { get; set; }
        public string AdresTanımı { get; set; }

        // Foreign Key
        public int KullanıcıId { get; set; }

        // Navigation Property

        //[JsonIgnore]
        public Kullanıcı Kullanıcı { get; set; }
    }
}
