using EruKampusSpor.Data;
using EruKampusSpor.DTOs;
using EruKampusSpor.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EruKampusSpor.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SeansController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SeansController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: api/seans
        [HttpGet]
        public ActionResult<IEnumerable<SeansDTO>> GetSeanslar()
        {
            var seanslar = _context.Seanslar
                .Include(s => s.Tesis)
                .Include(s => s.Salon)
                .Include(s => s.Brans)
                .Include(s => s.Kullanıcı)
                .ToList();

            var seansDTOs = seanslar.Select(s => new SeansDTO
            {
                TesisId = s.TesisId,
                SalonId = s.SalonId,
                BransId = s.BransId,
                SeansSaati = s.SeansSaati,
                Tarih = s.Tarih,
                SeansRezerveEdildiMi = s.SeansRezerveEdildiMi,
                KullanıcıId = s.KullanıcıId
            }).ToList();

            return Ok(seansDTOs);
        }

        // POST: api/seans
        [HttpPost]
        public ActionResult<SeansDTO> PostSeans(SeansDTO seansDto)
        {
            // DTO'dan gelen veriyi Seans modeline dönüştür
            var seans = new Seans
            {
                TesisId = seansDto.TesisId,
                SalonId = seansDto.SalonId,
                BransId = seansDto.BransId,
                SeansSaati = seansDto.SeansSaati,
                Tarih = seansDto.Tarih,
                
                
            };

            // Geçerli mi kontrolü (seans zaten var mı?)
            if (SeansVarlıkKontrol(seans))
            {
                return BadRequest("Bu seans zaten mevcut.");
            }

            // Seans ekleme
            _context.Seanslar.Add(seans);
            _context.SaveChanges();

            // Yeni eklenen seansı DTO olarak dön
            var createdSeansDto = new SeansDTO
            {
                TesisId = seans.TesisId,
                SalonId = seans.SalonId,
                BransId = seans.BransId,
                SeansSaati = seans.SeansSaati,
                Tarih = seans.Tarih,
                SeansRezerveEdildiMi = seans.SeansRezerveEdildiMi, //True= evet rezerve edildi. //false = hayır edilmedi demektir.
                KullanıcıId = seans.KullanıcıId
            };

            return CreatedAtAction(nameof(GetSeanslar),
                new { tesisId = seans.TesisId, salonId = seans.SalonId, bransId = seans.BransId, seansSaati = seans.SeansSaati, tarih = seans.Tarih },
                createdSeansDto);
        }


        // PUT: api/seans/choose/{tesisId}/{salonId}/{bransId}/{seansSaati}/{tarih}
        [HttpPut("choose/{tesisId}/{salonId}/{bransId}/{seansSaati}/{tarih}")]
        public IActionResult PutSeansSec(int tesisId, int salonId, int bransId, TimeSpan seansSaati, DateTime tarih, [FromBody] int kullanıcıId)
        {
            var seans = _context.Seanslar
                .FirstOrDefault(s => s.TesisId == tesisId && s.SalonId == salonId && s.BransId == bransId && s.SeansSaati == seansSaati && s.Tarih == tarih);

            if (seans == null)
            {
                return NotFound("Seans bulunamadı.");
            }

            // Seans zaten bir kullanıcı tarafından seçilmişse başka bir kullanıcı bu seans'ı alamaz
            if (seans.KullanıcıId.HasValue)
            {
                return BadRequest("Bu seans zaten rezerve edilmiş.");
            }



            // Seansı rezerve et
            seans.KullanıcıId = kullanıcıId;
            seans.SeansRezerveEdildiMi = true; // Seans rezerve edildi = true

            _context.Entry(seans).State = EntityState.Modified;
            _context.SaveChanges();

            // Güncellenmiş seansı DTO olarak döndür
            var seansDto = new SeansDTO
            {
                TesisId = seans.TesisId,
                SalonId = seans.SalonId,
                BransId = seans.BransId,
                SeansSaati = seans.SeansSaati,
                Tarih = seans.Tarih,
                SeansRezerveEdildiMi = seans.SeansRezerveEdildiMi,
                KullanıcıId = seans.KullanıcıId
            };

            return Ok(seansDto);
        }

        // Seans var mı kontrolü (composite key kontrolü)
        private bool SeansVarlıkKontrol(Seans seans)
        {
            return _context.Seanslar.Any(s =>
                s.TesisId == seans.TesisId &&
                s.SalonId == seans.SalonId &&
                s.BransId == seans.BransId &&
                s.SeansSaati == seans.SeansSaati &&
                s.Tarih == seans.Tarih);
        }
    }
}
