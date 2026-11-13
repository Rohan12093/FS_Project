using FSS_MVC.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FSS_MVC.Models
{
    public class AjaxController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        public AjaxController(ApplicationDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }
        //public IActionResult Index()
        //{
        //    string sql = @"SELECT       
        //        c.CourseCode,       
        //        c.CourseTitle,       
        //        c.Credits,       
        //        l.FirstName + ' ' + l.LastName AS LecturerName   
        //        FROM dbo.Courses c   
        //        INNER JOIN dbo.Lecturers l       
        //        ON c.LecturerId = l.LecturerId   
        //        ORDER BY c.CourseCode";

        //    var results = _context.Database.SqlQueryRaw<CourseLecturerVM>(sql).ToList();

        //    return View(results);
        //}

        //public IActionResult CourseCount()
        //{
        //    int count = _context.Courses.Count();

        //    return Json(new { count = count });
        //}
    }
}
