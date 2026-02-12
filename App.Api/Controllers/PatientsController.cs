using App.Api.Data;
using App.Api.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace App.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly VeterinaryContext Context; //Constructor Injection
        public PatientsController(VeterinaryContext _dbContext)
        {
            Context = _dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetPatients()
        {
            var patients = await Context.Patients.ToListAsync();
            return Ok(patients);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatientById([FromRoute] int id)
        { 
            var patient = await Context.Patients.FindAsync(id);
            if (patient is null)
            {
                return NotFound();
            }
            return Ok(patient);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePatient([FromBody] Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Context.Patients.Add(patient);
            await Context.SaveChangesAsync();
            return Ok(new { message = "Patient created" });
        }

        [HttpPut("{id}")]      
        public async Task<IActionResult> PatientUpdate([FromRoute] int id, [FromBody] UpdatePatientDto requestDto)
        {
         
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
          
            var existingPatient = await Context.Patients.FindAsync(id);
           
            if (existingPatient is null)
            {
                return NotFound($"Guncellemek Istenen {id} numarali Hayvan Bulunamadi");
            }
                       
            existingPatient.AnimalName = requestDto.AnimalName;
            existingPatient.TreatmentDescription = requestDto.TreatmentDescription;
            
            await Context.SaveChangesAsync();

            return Ok("Hayvan bilgisi başarıyla güncellendi.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> PatientDelete([FromRoute]int id)
        {
            var exist = await Context.Patients.FindAsync(id);
            if (exist is null)
            {
                return NotFound($"Silinmek istenen {id} numaralı Hayvan bulunamadı.");
            }
            Context.Patients.Remove(exist);
            await Context.SaveChangesAsync();
            return Ok($"ID {id} olan Hayvan silindi.");

        }
        
    }
}
