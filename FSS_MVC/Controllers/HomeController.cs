using FSS_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FSS_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        //public async Task<IActionResult> Index(decimal amount = 100)
        //{
        //    //call pub API
        //    string url = "https://api.frankfurter.dev/v2/rate/nzd/aud";
        //    var client = _httpClientFactory.CreateClient();

        //    try
        //    {
        //        //try call api and convet json to the model
        //        var reponse = await client.GetFromJsonAsync<ExchangeRateModel>(url);
        //        if (reponse == null)
        //        {
        //            return NotFound();
        //        }
        //        reponse.Amount = amount;

        //        //send the responce to the view 
        //        return View(reponse);

        //    }
        //    catch (HttpRequestException)
        //    {
        //        return NotFound();
        //    }
        //}


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
