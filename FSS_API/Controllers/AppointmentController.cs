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

            [Authorize(Roles = "Admin,Doctor")]
            [HttpGet]
            public async Task<IActionResult> Index()
            {
                var doctorSchedule = await _context.TblAppointmentSchedule.ToListAsync();

                return Ok(doctorSchedule);
            }

            //--Kailash 9/11/2026--//
            [Authorize(Roles = "Admin,Doctor,Patient")]
            [HttpGet("{appoScheId}")]
            public async Task<ActionResult<TblAppointmentSchedule>> GetAppointmentSchedule(int appoScheId)
            {
                var docsched = await _context.TblAppointmentSchedule.FirstOrDefaultAsync(p => p.DocScheId == appoScheId);

                if (docsched == null)
                {
                    return NotFound("Doctor Shedule not found");
                }

                return docsched;
            }

            //---PUT---/
            [Authorize(Roles = "Admin,Doctor")]
            [HttpPut("{appoScheId}")]
            public async Task<IActionResult> PutDocSchedule(int? appoScheId, TblAppointmentSchedule docSchedule)
            {
                if (appoScheId != docSchedule.DocScheId)
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
                return Ok($"Doctor Shedule {appoScheId} has been successfully updated");
            }

            //--POST--//
            [Authorize(Roles = "Admin,Doctor")]
            [HttpPost]
            public async Task<ActionResult<TblAppointmentSchedule>> PostDocShedule(TblAppointmentSchedule appoScheId)
            {
                _context.TblAppointmentSchedule.Add(appoScheId);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetDoctorSchedule", new { appoScheId = docSchedule.DocScheId }, appoScheId);
            }

            //--DELETE--//
            [Authorize(Roles = "Admin,Doctor")]
            [HttpDelete("{appoScheId}")]
            public async Task<IActionResult> DeleteDocShedule(int? appoScheId)
            {
                var docsched = await _context.TblAppointmentSchedule.FindAsync(appoScheId);
                if (docsched == null)
                {
                    return NotFound();
                }

                _context.TblAppointmentSchedule.Remove(appoScheId);
                await _context.SaveChangesAsync();

                return Ok($"Doctor Schedule has been successfully removed");
            }

            private bool DocSheduleExists(int? appoScheId)
            {
                return _context.TblAppointmentSchedule.Any(e => e.appoScheId == appoScheId);
            }

        }
    }