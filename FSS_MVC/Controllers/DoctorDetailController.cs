using FSS_MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FSS_MVC.Controllers
{
    public class DoctorDetailController : Controller
    {
        private readonly HttpClient _httpClient;
        public DoctorDetailController(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("FSS_API");
        }

        [Authorize(Roles = "Doctor, Admin")]
        public async Task<IActionResult> Index()
        {
            var booking = await _httpClient.GetFromJsonAsync<List<DoctorDetail>>(
                "api/DoctorDetailController");

            return View(booking);
        }
        [Authorize(Roles = "Doctor, Admin")]
        public async Task<IActionResult> Detail(int id)
        {
            var response = await _httpClient.GetAsync($"api/DoctorDetail/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return NotFound();
            if (!response.IsSuccessStatusCode)
                return NotFound();

            return View(await response.Content.ReadFromJsonAsync<DoctorDetail>());

        }
        [Authorize(Roles = "Doctor, Admin")]
        public async Task<IActionResult> Create()
        {
            var appoSch = await _httpClient.GetFromJsonAsync<List<AppointmentSch>>("api/Appointment");

            ViewBag.appoSch = new SelectList(
                appoSch,
                "AppoSchId",
                "AppoSchIsDate",
                "AppoSchStime"
                );

            return View();
        }
        [Authorize(Roles = "Doctor, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DoctorDetail docDetail)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PostAsJsonAsync("api/DoctorDetail", docDetail);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return View(docDetail);
            }
            return BadRequest();
        }
        [Authorize(Roles = "Doctor, Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"api/AppointmentSch/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return NotFound();
            if (!response.IsSuccessStatusCode)
                return NotFound();

            var bookingSch = await response.Content.ReadFromJsonAsync<DoctorDetail>();

            if (bookingSch == null)
                return NotFound();

            //---------------------
            var appoSch = await _httpClient.GetFromJsonAsync<List<AppointmentSch>>("api/Appointment");

            ViewBag.appoSch = new SelectList(
                appoSch,
                "DocSchIsDate",
                "DocSchStime",
                bookingSch.DoctorId
                );
            //----------------------

            return View(bookingSch);

        }
        [Authorize(Roles = "Doctor, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DoctorDetail doctorDetail)
        {
            if (id != doctorDetail.DoctorId)
                return BadRequest();

            var response = await _httpClient.PutAsJsonAsync($"api/DoctorDetail/{id}", doctorDetail);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            return View(doctorDetail);

        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {

            var response = await _httpClient.GetAsync($"api/DoctorDetail/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return NotFound();
            if (!response.IsSuccessStatusCode)
                return NotFound();

            var booking = (await response.Content.ReadFromJsonAsync<DoctorDetail>());
            return View(booking);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(DoctorDetail doctorDetail)
        {
            var response = await _httpClient.DeleteAsync($"api/DoctorDetail/{doctorDetail.DoctorId}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(doctorDetail);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var response = await _httpClient.GetAsync($"api/PatientDetail/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return NotFound();
            if (!response.IsSuccessStatusCode)
                return NotFound();

            var booking = (await response.Content.ReadFromJsonAsync<PatientDetail>());
            return View(booking);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(PatientDetail patientDetail)
        {
            var response = await _httpClient.DeleteAsync($"api/PatientDetail/{patientDetail.PatientId}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(patientDetail);
        }

    }
}

