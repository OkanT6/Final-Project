using EruKampusSpor.Data;
using EruKampusSpor.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EruKampusSpor.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RezervasyonController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        public RezervasyonController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllRezervasyonlar()
        {
            var rezervasyonlar = _context.Rezervasyonlar.ToList();

            if (rezervasyonlar == null)
                return NotFound("Hiç bir rezervasyon yok");
            return Ok(rezervasyonlar);
        }


        [HttpPost]
        public IActionResult rezervasyonYap([FromQuery] int KullanıcıId, [FromQuery] int SeansId)
        {

            var kullanıcı = _context.Kullanıcılar.Include(k => k.KullanıcıDetay).FirstOrDefault(k => k.KullanıcıId == KullanıcıId);
                

            var seans=_context.Seanslar.FirstOrDefault(s=>s.SeansId==SeansId);
            
            if (kullanıcı == null)
                return BadRequest("Böyle bir kullanıcı yoktur");
            if (seans == null)
                return BadRequest("Böyle bir seans yoktur");

            
            if (seans.SeansCinsiyet != SeansCinsiyet.Karma)
            { 
                if(kullanıcı.KullanıcıDetay.Cinsiyet.ToString()!=seans.SeansCinsiyet.ToString())
                {
                    if (kullanıcı.KullanıcıDetay.Cinsiyet.ToString() == "Erkek")
                        return BadRequest("Bu seans sadece kadınlara özeldir");
                    else return BadRequest("Bu seans sadece erkeklere özeldir");
                }

            }



            if (seans.Dolu == true)
                return BadRequest("Seans Doludur");

            
            var rezerveEdilmisAynıSeans=_context.Rezervasyonlar.FirstOrDefault(r => r.SeansId == SeansId && r.KullanıcıId == KullanıcıId);

            if (rezerveEdilmisAynıSeans != null)
                return BadRequest("Aynı seansı zaten rezerve etmişsin.");

            var yeniRezervasyon = new Rezervasyon { 
            KullanıcıId= KullanıcıId,
                SeansId=SeansId
            };
            seans.RezerveEdenKisiSayisi++;
            if (seans.Kontenjan==seans.RezerveEdenKisiSayisi)
                seans.Dolu = true;

            _context.Rezervasyonlar.Add(yeniRezervasyon);
            _context.SaveChanges();

            
            var rezervasyonBilgileri=_context.Rezervasyonlar.Where(r=>r.KullanıcıId==KullanıcıId).Include(r=>r.Seans).ToList();
            return Ok(yeniRezervasyon);
            
            
            //// Parametreleri kullanarak işlemler yapabilirsiniz.
            //return Ok(new { Id = id, Name = name });
        }
        [HttpPut]
        public IActionResult RezervasyonIptal([FromQuery] int KullanıcıId, [FromQuery] int SeansId)
        {
            var kullanıcı = _context.Kullanıcılar.Include(k => k.KullanıcıDetay).FirstOrDefault(k => k.KullanıcıId == KullanıcıId);


            var seans = _context.Seanslar.FirstOrDefault(s => s.SeansId == SeansId);

            var rezervasyon=_context.Rezervasyonlar.Find(new { KullanıcıId , SeansId });

            if (kullanıcı == null)
                return BadRequest("Böyle bir kullanıcı yoktur");
            if (seans == null)
                return BadRequest("Böyle bir seans yoktur");

            if (rezervasyon == null)
                return BadRequest("Böyle bir rezervasyon yoktur");
            if (!IptalEdilebilirMi(seans))
                return BadRequest("Başlangıç saatine 24 saatten az kalan seansların rezervasyonları iptal edilemez.");

            rezervasyon.IptalEdildi = true;
            seans.RezerveEdenKisiSayisi--;
            if (seans.Dolu == true)
                seans.Dolu =false;

            var rezervasyonBilgileri = _context.Rezervasyonlar.Where(r => r.KullanıcıId == KullanıcıId).Include(r => r.Seans).ToList();
            return Ok(rezervasyon);





        }

        public bool IptalEdilebilirMi(Seans seans)
        {
            return DateTime.UtcNow < seans.SeansBaslangicZamani.AddHours(-24);
        }


    }
}
