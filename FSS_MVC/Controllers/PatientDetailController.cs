using FSS_MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using FSS_MVC.Models;

namespace FSS_MVC.Controllers
{
    public class PatientDetailController : Controller
    {
        private readonly HttpClient _httpClient;
        public PatientDetailController(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("FSS_API");
        }
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> Index()
        {
            var booking = await _httpClient.GetFromJsonAsync<List<PatientDetail>>(
                "api/PatientDetailController/{id}");

            return View(booking);
        }
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"api/PatientDetails/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return NotFound();
            if (!response.IsSuccessStatusCode)
                return NotFound();

            return View(await response.Content.ReadFromJsonAsync<PatientDetail>());

        }
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> Create()
        {
            var appoSch = await _httpClient.GetFromJsonAsync<List<PatientDetail>>("api/PatientDetail/");

            ViewBag.appoSch = new SelectList(
                appoSch,
                "DocScheId",
                "DocSchIsDate",
                "DocSchStime"
                );

            return View();
        }
        [Authorize(Roles = "Patient")]
        [HttpPost]
        public async Task<IActionResult> Create(PatientDetail booking)
        {
            var response = await _httpClient.PostAsJsonAsync("api/PatientDetail", booking);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(booking);

        }
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"api/PatientDetail/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return NotFound();
            if (!response.IsSuccessStatusCode)
                return NotFound();
            var patient = await response.Content.ReadFromJsonAsync<PatientDetail>();

            if (patient == null)
                return NotFound();

            //---------------------
            var booking = await _httpClient.GetFromJsonAsync<List<PatientDetail>>("api/PatientDetail");

            ViewBag.Booking = new SelectList(
                booking,
                "BookingId",
                "BookingDate",
                patient.PatientId
                );

            return View(patient);

        }
        [Authorize(Roles = "Patient")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PatientDetail patientDetail)
        {
            if (id != patientDetail.PatientId)
                return BadRequest();

            var response = await _httpClient.PutAsJsonAsync($"api/PatientDetail/{id}", patientDetail);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            return View(patientDetail);

        }

    }

}
