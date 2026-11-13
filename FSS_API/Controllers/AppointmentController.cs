using FSS_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FSS_API.Controllers
{
    
        [ApiController]
        [Route("api/[controller]")]
        public class AppointmentController : Controller
        {
            private readonly DBS_Context _context;

            public AppointmentController(DBS_Context context)
            {
                _context = context;
            }

            [Authorize(Roles = "Admin, Doctor")]
            [HttpGet]
            public async Task<IActionResult> Index()
            {
                var doctorSchedule = await _context.TblAppointmentSchedules.ToListAsync();

                return Ok(doctorSchedule);
            }

            //--Kailash 9/11/2026--//
            [Authorize(Roles = "Admin,Doctor,Patient")]
            [HttpGet("{appoScheId}")]
            public async Task<ActionResult<TblAppointmentSchedules>> GetAppointmentSchedule(int appoScheId)
            {
                var docsche = await _context.TblAppointmentSchedules.FirstOrDefaultAsync(p => p.AppoScheId == appoScheId);

                if (docsche == null)
                {
                    return NotFound("Doctor Shedule not found");
                }

                return docsche;
            }

            //---PUT---/
            [Authorize(Roles = "Admin,Doctor")]
            [HttpPut("{appoScheId}")]
            public async Task<IActionResult> PutDocSchedule(int? appoScheId, TblAppointmentSchedules docSchedule)
            {
                if (appoScheId != docSchedule.AppoScheId)
                {
                    return BadRequest();
                }

                _context.Entry(docSchedule).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocSheduleExists(appoScheId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Ok($"Doctor Schedule {appoScheId} has been successfully updated");
            }

            //--POST--//
            [Authorize(Roles = "Admin,Doctor")]
            [HttpPost]
            public async Task<ActionResult<TblAppointmentSchedules>> PostDocShedule(TblAppointmentSchedules appoScheId)
            {
                _context.TblAppointmentSchedules.Add(appoScheId);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetDoctorSchedule", new { appoScheId = appoScheId.AppoScheId }, appoScheId);
            }

            //--DELETE--//
            [Authorize(Roles = "Admin,Doctor")]
            [HttpDelete("{appoScheId}")]
            public async Task<IActionResult> DeleteDocShedule(int? appoScheId)
            {
                var appoSched = await _context.TblAppointmentSchedules.FindAsync(appoScheId);
                if (appoSched == null)
                {
                    return NotFound();
                }

                _context.TblAppointmentSchedules.Remove(appoSched);
                await _context.SaveChangesAsync();

                return Ok($"Doctor Schedule has been successfully removed");
            }

            private bool DocSheduleExists(int? appoScheId)
            {
                return _context.TblAppointmentSchedules.Any(e => e.AppoScheId == appoScheId);
            }

        }
    }