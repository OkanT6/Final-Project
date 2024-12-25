namespace EruKampusSpor.DTOs
{
    public class AdresDTO
    {
        public int Id { get; set; }
        public string AdresTanımı { get; set; }

        // Foreign Key
        public int KullanıcıId { get; set; }
    }
}
