using EruKampusSpor.Data;
using EruKampusSpor.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EruKampusSpor.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class KullanıcıDetayController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public KullanıcıDetayController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPut()]
        public IActionResult UpdateKullanıcıDetay([FromBody] KullanıcıDetay yeniKullanıcıDetay) 
        {
            var kullaniciId = yeniKullanıcıDetay.Id;

            var kullanıcı =  _context.Kullanıcılar.Find(kullaniciId); 
            if (kullanıcı == null) 
            {
                return NotFound($"ID'si {kullaniciId} olan kullanıcı bulunamadı.");
            }

            // Kullanıcıya ait ilk adresi al
            var mevcutKullanıcıDetay = _context.KullanıcıDetayları
                .FirstOrDefault(kd => kd.Id == kullaniciId);

            if (mevcutKullanıcıDetay == null)
            {
                return NotFound($"ID'si {kullaniciId} olan kullanıcıya ait adres bulunamadı.");
            }
            mevcutKullanıcıDetay.Adres = yeniKullanıcıDetay.Adres;
            mevcutKullanıcıDetay.EMail =yeniKullanıcıDetay.EMail;
            mevcutKullanıcıDetay.Telefon =yeniKullanıcıDetay.Telefon;

            _context.SaveChanges();

            return Ok(mevcutKullanıcıDetay);


        }

        [HttpGet()]
        public IActionResult GetKullanıcıDetay(int ID)
        {
            

            var kullanıcıDetayı = _context.KullanıcıDetayları.Find(ID);
            if (kullanıcıDetayı == null)
                return NotFound("Böyle bir kullanıcı yoktur");
            return Ok(kullanıcıDetayı);
        }

        [HttpGet()]
        public IActionResult GetAllKullanıcıDetayları()
        {
           var kullanıcıDetayları= _context.KullanıcıDetayları;
            if (kullanıcıDetayları == null)
                return Ok("Hiç bir kullanıcı sistemde kayıtlı olmadığı için detay bilgisi yoktur");
            return Ok(kullanıcıDetayları);
        }
    }
}
