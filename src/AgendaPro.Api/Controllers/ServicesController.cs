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
        private readonly ServiceUseCase _serviceUseCase;

        public ServicesController( ServiceUseCase serviceUseCase)
        {
            _serviceUseCase = serviceUseCase;

        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {

            var getListOfServices = await _serviceUseCase.GetAllAsync();

            return Ok(getListOfServices);

        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {

            var getServiceById = await _serviceUseCase.GetByIdAsync(id);

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
        public async Task<IActionResult> UpdateAsync(Guid id, ServiceDTO serviceDTO)
        {
            await _serviceUseCase.UpdateAsync(id, serviceDTO);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {

            await _serviceUseCase.DeleteAsync(id);

            return NoContent();

        }
    }
}
