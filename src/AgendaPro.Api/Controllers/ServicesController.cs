using Microsoft.AspNetCore.Mvc;
using AgendaPro.Domain.Services.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using AgendaPro.Infrastucture.Data.Context;
using AgendaPro.Application.Services.DTOs;
using AgendaPro.Application.Services.UseCases;
using AgendaPro.Api.Wrappers;

namespace AgendaPro.Api.Controllers
{
    [ApiController]
    [Route("api/services")]
    public class ServicesController : ControllerBase
    {

        private readonly AgendaProDbContext _context;
        private readonly ServiceUseCase _serviceUseCase;

        public ServicesController(AgendaProDbContext context, ServiceUseCase serviceUseCase)
        {

            _context = context;
            _serviceUseCase = serviceUseCase;

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
        public async Task<IActionResult> PostAsync(ServiceDTO serviceDTO)
        {
            var result = await _serviceUseCase.CreateAsync(serviceDTO);
            var response = new ApiResponse<ServiceDTO>(result);
            return Ok(response);
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
