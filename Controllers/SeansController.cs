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
        // GET: api/seans
        [HttpGet]
        public ActionResult<IEnumerable<SeansDTO>> GetSeanslar()
        {
            var seanslar = _context.Seanslar
                .Include(s => s.Tesis)
                .Include(s => s.Salon)
                .Include(s => s.Brans)
                .ToList();



            return Ok(seanslar);
        }

        [HttpPost]
        public IActionResult seansEkle(SeansDTO Yeniseans)
        {
            if (Yeniseans == null)
                return BadRequest();

           



            Seans seans = new Seans
            {
                TesisId = Yeniseans.TesisId,
                SalonId = Yeniseans.SalonId,
                BransId = Yeniseans.BransId,
                Tarih = Yeniseans.Tarih,
                SeansSaati = Yeniseans.SeansSaati,
                Kontenjan = Yeniseans.Kontenjan,
                SeansCinsiyet=Yeniseans.SeansCinsiyet


            };
            // Geçerli mi kontrolü(seans zaten var mı ?)
            if (SeansVarlıkKontrol(seans))
            {
                return BadRequest("Bu seans zaten mevcut.");
            }



            _context.Seanslar.Add(seans);
            _context.SaveChanges();



            return Ok(seans);



        }






        //// POST: api/seans
        //[HttpPost]
        //public ActionResult<SeansDTO> PostSeans(SeansDTO seansDto)
        //{
        //    // DTO'dan gelen veriyi Seans modeline dönüştür
        //    var seans = new Seans
        //    {
        //        TesisId = seansDto.TesisId,
        //        SalonId = seansDto.SalonId,
        //        BransId = seansDto.BransId,
        //        SeansSaati = seansDto.SeansSaati,
        //        Tarih = seansDto.Tarih,


        //    };

        //    // Geçerli mi kontrolü (seans zaten var mı?)
        //    if (SeansVarlıkKontrol(seans))
        //    {
        //        return BadRequest("Bu seans zaten mevcut.");
        //    }

        //    // Seans ekleme
        //    _context.Seanslar.Add(seans);
        //    _context.SaveChanges();

        //    // Yeni eklenen seansı DTO olarak dön
        //    var createdSeansDto = new SeansDTO
        //    {
        //        TesisId = seans.TesisId,
        //        SalonId = seans.SalonId,
        //        BransId = seans.BransId,
        //        SeansSaati = seans.SeansSaati,
        //        Tarih = seans.Tarih,

        //    };

        //    return CreatedAtAction(nameof(GetSeanslar),
        //        new { tesisId = seans.TesisId, salonId = seans.SalonId, bransId = seans.BransId, seansSaati = seans.SeansSaati, tarih = seans.Tarih },
        //        createdSeansDto);
        //}


        ////// PUT: api/seans/choose/{tesisId}/{salonId}/{bransId}/{seansSaati}/{tarih}
        ////[HttpPut("choose/{tesisId}/{salonId}/{bransId}/{seansSaati}/{tarih}")]
        ////public IActionResult PutSeansSec(int tesisId, int salonId, int bransId, TimeSpan seansSaati, DateTime tarih, [FromBody] int kullanıcıId)
        ////{
        ////    var seans = _context.Seanslar
        ////        .FirstOrDefault(s => s.TesisId == tesisId && s.SalonId == salonId && s.BransId == bransId && s.SeansSaati == seansSaati && s.Tarih == tarih);

        ////    if (seans == null)
        ////    {
        ////        return NotFound("Seans bulunamadı.");
        ////    }

        ////    // Seans zaten bir kullanıcı tarafından seçilmişse başka bir kullanıcı bu seans'ı alamaz




        ////    // Seansı rezerve et


        ////    _context.Entry(seans).State = EntityState.Modified;
        ////    _context.SaveChanges();

        ////    // Güncellenmiş seansı DTO olarak döndür
        ////    var seansDto = new SeansDTO
        ////    {
        ////        TesisId = seans.TesisId,
        ////        SalonId = seans.SalonId,
        ////        BransId = seans.BransId,
        ////        SeansSaati = seans.SeansSaati,
        ////        Tarih = seans.Tarih,

        ////    };

        ////    return Ok(seansDto);
        ////}

        //// Seans var mı kontrolü (composite key kontrolü)
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
