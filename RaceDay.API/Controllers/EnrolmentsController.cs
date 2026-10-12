using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrolmentsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public EnrolmentsController(RaceDayDbContext context)
        {
            _context = context;
        }

        // GET: api/Enrolments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Enrolment>>> GetEnrolments()
        {
            return await _context.Enrolments
                .Include(e => e.User)
                .Include(e => e.Event)
                .ToListAsync();
        }

        // GET: api/Enrolments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Enrolment>> GetEnrolment(int id)
        {
            var enrolment = await _context.Enrolments
                .Include(e => e.User)
                .Include(e => e.Event)
                .FirstOrDefaultAsync(e => e.EnrolmentId == id);

            if (enrolment == null)
            {
                return NotFound();
            }

            return enrolment;
        }

        // POST: api/Enrolments
        [HttpPost]
        public async Task<ActionResult<Enrolment>> PostEnrolment(Enrolment enrolment)
        {
            _context.Enrolments.Add(enrolment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEnrolment), new { id = enrolment.EnrolmentId }, enrolment);
        }
    }
}