using EruKampusSpor.Data;
using EruKampusSpor.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EruKampusSpor.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public AdresController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPut("{kullaniciId}")]
        public IActionResult UpdateAdres(int kullaniciId,string yeniAdressTanımı)
        {
            var kullanıcı =  _context.Kullanıcılar.Find(kullaniciId);
            if (kullanıcı == null)
            {
                return NotFound($"ID'si {kullaniciId} olan kullanıcı bulunamadı.");
            }

            // Kullanıcıya ait ilk adresi al
            var mevcutAdres =  _context.Adresler
                .FirstOrDefault(a => a.KullanıcıId == kullaniciId);

            if (mevcutAdres == null)
            {
                return NotFound($"ID'si {kullaniciId} olan kullanıcıya ait adres bulunamadı.");
            }
            mevcutAdres.AdresTanımı = yeniAdressTanımı;
            _context.SaveChanges();

            return Ok(mevcutAdres);


        }
    }
}
