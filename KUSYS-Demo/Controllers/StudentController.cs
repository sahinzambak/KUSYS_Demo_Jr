using KUSYS_Demo.Data;
using KUSYS_Demo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.Json.Serialization;




// AÇIKLAMA = VIEW'LAR İÇERİSİNDE RAZOR COMPENENTLER YERİNE JS ve JS Kütüphanesindeki JQuery Kodları kullanarak sayfaları interaktivitesini sağladım.
namespace KUSYS_Demo.Controllers
{
    public class StudentController : Controller
    {
        //DbContextimi constructor'da yaratıyorum
        private readonly KUSYSDbContext _dbContext;

        public StudentController(KUSYSDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Tüm Kursları Select'de göstermek için çağırıyorum
        public JsonResult GetCourseList()
        {
            var res = _dbContext.Courses.ToList();

            return Json(JsonConvert.SerializeObject(res));
        }

        //Tüm öğrencileri html tablosuna çağırmak için
        public JsonResult GetStudentList()
        {
            var res = _dbContext.Students.Include(x=>x.Course).ToList();

            return Json(JsonConvert.SerializeObject(res));
        }

        public JsonResult SaveNewStudent(Student p) 
        {
            //yeni öğrenciyi buradan ekliyorum, validation rulelar istenmediği için database'in istediği şekilde, sorun çıkmasın diye hata yakalama koydum
            try
            {
                _dbContext.Students.Add(p);
                _dbContext.SaveChanges();
                return Json(1);
            }
            catch (Exception)
            {
                return Json(0);
            }   
        }

        public JsonResult ShowStudent(int id) {
            //öğrenciyi ayrı bir modalda göstermek için
            try
            {
                var res = _dbContext.Students.Where(x => x.StudentId == id).FirstOrDefault();
                return Json(JsonConvert.SerializeObject(res));
            }
            catch (Exception)
            {
                return Json(0);
            }
        }

        public JsonResult EditStudent(Student p)
        {
            //modal'da karşımıza gelen öğrenci bilgilerini update etmek istersek
            try
            {
                _dbContext.Students.Update(p);
                _dbContext.SaveChanges();
                return Json(1);
            }
            catch (Exception)
            {
                return Json(0);
            }
        }

        public JsonResult DeleteStudent(int id)
        {
            //öğrenciyi Id'den çağırıp silmek için
            try
            {
                var res = _dbContext.Students.Where(x => x.StudentId == id).FirstOrDefault();
                _dbContext.Students.Remove(res);
                _dbContext.SaveChanges();
                return Json(1);
            }
            catch (Exception)
            {
                return Json(0);
            }
        }
    }
}
