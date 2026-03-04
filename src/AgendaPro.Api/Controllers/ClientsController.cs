using AgendaPro.Application.Clients.UseCases;
using AgendaPro.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using AgendaPro.Api.Extensions;
using AgendaPro.Application.Clients.DTOs;


namespace AgendaPro.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {

        private readonly ClientUseCase _clientUseCase;

        public ClientsController(ClientUseCase clientUseCase)
        {

            _clientUseCase = clientUseCase;

        }


        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {

            var result = await _clientUseCase.GetAllAsync();

            return result.ToActionResult();

        }


        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {

            var result = await _clientUseCase.GetByIdAsync(id);
            
            return result.ToActionResult();
        
        }


        [HttpPost]
        public async Task<IActionResult> PostAsync(ClientDTO clientDTO)
        {

            var result = await _clientUseCase.CreateAsync(clientDTO);
        
            return result.ToActionResult();
        
        }


        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAsync(Guid id, ClientDTO clientDTO)
        {

            var result = await _clientUseCase.UpdateAsync(id, clientDTO);

            return result.ToActionResult();
        
        }


        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {

            var result = await _clientUseCase.DeleteAsync(id);
        
            return result.ToActionResult();
        
        }

        [HttpGet("filter-by-name")]
        public async Task<IActionResult>FilterByNameLike([FromQuery]string name)
        {

            var result = await _clientUseCase.FilterByNameLike(name);

            return result.ToActionResult();

        }

        [HttpGet("filter-by-email")]
        public async Task<IActionResult> FilterByEmailLike([FromQuery] string email)
        {

            var result = await _clientUseCase.FilterByEmailLike(email);
            
            return result.ToActionResult();
        
        }

    }
}
