using EruKampusSpor.Data;
using EruKampusSpor.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EruKampusSpor.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TesisController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public TesisController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        [Route("api/tesisler")]
        public IActionResult GetTesisler()
        {
            var tesisler = _context.Tesisler.ToList();  // Tüm Tesisleri getir
            if (tesisler == null || !tesisler.Any())
            {
                return NotFound("Hiçbir Tesis bulunamadı.");
            }

            return Ok(tesisler);  // Başarıyla bulunan Tesisleri döndür



        }


        [HttpPost]
        [Route("api/tesisler")]
        public IActionResult AddTesis([FromBody] Tesis tesis)
        {
            if (tesis == null)
            {
                return BadRequest("Tesis verisi eksik.");
            }

            // Tesis'i veritabanına ekle
            _context.Tesisler.Add(tesis);
            _context.SaveChanges();  // Değişiklikleri kaydet

            // Başarıyla eklenen Tesis'i döndür
            //return CreatedAtAction(nameof(GetTesisById), new { id = tesis.TesisId }, tesis);
            return Ok(tesis);
        }
    }
}
