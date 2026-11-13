//using FSS_MVC.Data;
//using Microsoft.AspNetCore.Mvc;
//using System.Text.Json;

//namespace FSS_MVC.Models
//{
//        public class ManageUserController : Controller
//        {
//            private readonly ApplicationDbContext _context;
//            private readonly IHttpClientFactory _httpClientFactory;
//            public ManageUserController(ApplicationDbContext context, IHttpClientFactory httpClientFactory)
//            {
//                _context = context;
//                _httpClientFactory = httpClientFactory;
//            }
//            //ctor for constructor 

//            public IActionResult Index()
//            {
//                string sql = @"SELECT       
//                c.CourseCode,       
//                c.CourseTitle,       
//                c.Credits,       
//                l.FirstName + ' ' + l.LastName AS LecturerName   
//                FROM dbo.Courses c   
//                INNER JOIN dbo.Lecturers l       
//                ON c.LecturerId = l.LecturerId   
//                ORDER BY c.CourseCode";

//                var results = _context.Database.SqlQueryRaw<CourseLecturerVM>(sql).ToList();

//                return View(results);
//            }

//            public IActionResult CourseCount()
//            {
//                int count = _context.Courses.Count();

//                return Json(new { count = count });
//            }

//            public async Task<IActionResult> BookSuggestion()
//            {
//                try
//                {
//                    var client = _httpClientFactory.CreateClient();

//                    string url = "https://openlibrary.org/search.json?title=programming&field=title&limit=1";
//                    string json = await client.GetStringAsync(url);

//                    using var document = JsonDocument.Parse(json);

//                    var books = document.RootElement.GetProperty("docs");

//                    if (books.GetArrayLength() == 0)
//                    {
//                        return Json(new { title = "No book found" });
//                    }

//                    var title = books[0].GetProperty("title").GetString();

//                    return Json(new { title = title });

//                }
//                catch (Exception ex)
//                {
//                    return StatusCode(503, new { Message = "Book service is unavailable" });
//                }
//            }
//        }
//    }
