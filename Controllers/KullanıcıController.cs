using EruKampusSpor.Data;
using EruKampusSpor.DTOs;
using EruKampusSpor.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EruKampusSpor.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class KullanıcıController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public KullanıcıController(ApplicationDbContext context)
        {
            _context = context;
        }




        [HttpGet]
        public IActionResult GetAllUser()
        {
            try
            {
                // Kullanıcılar listesini veritabanından çekiyoruz.
                var users = _context.Kullanıcılar.ToList(); //Select * from Users


                // Liste boşsa 404 döndür
                if (users == null || !users.Any())
                {
                    return NotFound("Kullanıcı bulunamadı.");
                }

                //İleride Kullanıcı DTO'ları gönder.

                // Başarılı durum koduyla listeyi döndür
                return Ok(users);
            }
            catch (Exception ex)
            {
                // Hata durumunda 500 döndür
                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }


        [HttpPost]
        public IActionResult CreateUser([FromBody] KullanıcıDTO kullanıcıDTO)
        {
            //if (newUser == null)
            //    return BadRequest();
            //if (!ModelState.IsValid) 
            //    return BadRequest(ModelState);
            //bool IsTcMatching=false;
            //foreach (var user in ApplicationDbContext.bilgiIslem)
            //{
            //    if(user.TC==newUser.UserTC)
            //        IsTcMatching=true;
            //}
            //if (!IsTcMatching)
            //    return NotFound("You are not related to Erciyes University");

            if (kullanıcıDTO == null)
                return BadRequest();
            if (kullanıcıDTO.TC.Length != 11)
                return BadRequest();
            bool IsTcMatching = false;
            string matchingName;

            var appKullanıcısıTcleri = _context.Kullanıcılar.Select(k => k.TC).ToList();
            foreach(var appKullanıcısıTc in appKullanıcısıTcleri)
            {
                if(kullanıcıDTO.TC== appKullanıcısıTc)
                {
                    return BadRequest("Kullanıcı zaten kayıtlı");
                }
            }


            foreach (var eruMember in ApplicationDbContext.bilgiIslem)
            {
                if (eruMember.TC == kullanıcıDTO.TC)
                {
                    IsTcMatching = true;
                    matchingName = eruMember.name;
                    break;
                }
            }
            if (!IsTcMatching)
                return NotFound("You are not related to Erciyes University");


            var target = ApplicationDbContext.bilgiIslem.FirstOrDefault(x => x.TC == kullanıcıDTO.TC); //LINQ Kullanımı
            Kullanıcı kullanıcı = new() { TC = target.TC, Name = target.name, };
            kullanıcı.Adres = new()
            {
                AdresTanımı = "Default Kayseri Adresi"
            };

            _context.Kullanıcılar.Add(kullanıcı);
            _context.SaveChanges();



            return Ok(kullanıcı);









            //_context.Users.Add(newUser);
            //_context.SaveChanges();
            //return Ok(newUser);



        }

        [HttpDelete]
        public IActionResult DeleteUser([FromBody] int id)
        {
            try
            {
                // Kullanıcıyı id ile bul
                var user = _context.Kullanıcılar.FirstOrDefault(u => u.KullanıcıId == id);

                // Eğer kullanıcı yoksa, 404 döndür
                if (user == null)
                {
                    return NotFound($"Id'si {id} olan kullanıcı bulunamadı.");
                }

                // Kullanıcıyı veritabanından sil
                _context.Kullanıcılar.Remove(user);
                _context.SaveChanges();

                // Başarılı durum kodu ile mesaj döndür
                return Ok($"Id'si {id} olan kullanıcı başarıyla silindi.");
            }
            catch (Exception ex)
            {
                // Hata durumunda 500 döndür
                return StatusCode(500, $"Bir hata oluştu: {ex.Message}");
            }
        }

    }


}
