using Microsoft.AspNetCore.Mvc;
using AgendaPro.Domain.Services.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using AgendaPro.Infrastucture.Data.Context;
using AgendaPro.Application.Services.DTOs;
using AgendaPro.Application.Services.UseCases;
using AgendaPro.Api.Wrappers;
using Microsoft.Build.Tasks;
using AgendaPro.Domain.Shared;
using AgendaPro.Api.Extensions;

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

            var result = await _serviceUseCase.GetAllAsync();

            return result.ToActionResult();

        }


        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {

            var result = await _serviceUseCase.GetByIdAsync(id);

            return result.ToActionResult();

        }


        [HttpPost]
        public async Task<IActionResult> PostAsync(ServiceDTO serviceDTO)
        {

            var result = await _serviceUseCase.CreateAsync(serviceDTO);

            return result.ToActionResult();


        }


        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAsync(Guid id, ServiceDTO serviceDTO)
        {
        
            var result = await _serviceUseCase.UpdateAsync(id, serviceDTO);

            return result.ToActionResult();
        
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {

            var result = await _serviceUseCase.DeleteAsync(id);

            return result.ToActionResult();

        }

        [HttpGet("filter-by-name")]
        public async Task<IActionResult> FilterByNameLike([FromQuery] string name)
        {
            var result = await _serviceUseCase.FilterByNameLike(name);
            return result.ToActionResult();
        }

        [HttpGet("filter-by-description")]
        public async Task<IActionResult> FilterByDescriptionLike([FromQuery] string description)
        {
            var result = await _serviceUseCase.FilterByDescriptionLike(description);
            return result.ToActionResult();
        }
    }
}
