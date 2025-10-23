using Microsoft.AspNetCore.Mvc;
using AgendaPro.Domain.Services.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using AgendaPro.Infrastucture.Data.Context;

namespace AgendaPro.Api.Controllers
{
    [ApiController]
    [Route("api/services")]
    public class ServicesController : ControllerBase
    {

        private readonly AgendaProDbContext _context;

        public ServicesController(AgendaProDbContext context)
        {

            _context = context;

        }


        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {

            var getListOfServices = _context.Services.ToList();

            return Ok(getListOfServices);

        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {

            var getServiceById = _context.Services.SingleOrDefault(j => j.Id == id);

            if (getServiceById == null)
            {
                return NotFound();
            }

            return Ok(getServiceById);

        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(ServiceModel serviceModel)
        {
            
            _context.Services.Add(serviceModel);

            _context.SaveChanges();

            return CreatedAtAction(nameof(GetByIdAsync), new { id = serviceModel.Id }, serviceModel);

        }


        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAsync(Guid id, ServiceModel sm)
        {

            var serviceToUpdate = _context.Services.SingleOrDefault(j => j.Id == id);

            if (serviceToUpdate == null)
            {
                return NotFound();
            }

            sm.UpdateService(sm.Nome, sm.DuracaoMin, sm.Preco, sm.Descricao, sm.CategoriaId, sm.IntervaloMin);
            
            _context.Update(serviceToUpdate);

            _context.SaveChanges();

            return NoContent();

        }


        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {

            var serviceToDelete = _context.Services.SingleOrDefault(j => j.Id == id);

            if (serviceToDelete == null)
            {
                return NotFound();
            }

            _context.Remove(serviceToDelete);
            
            _context.SaveChanges();

            return NoContent();

        }
    }
}
