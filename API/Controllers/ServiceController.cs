using Data.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Shared.Entities;
using Shared.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetListAsync(
            [FromQuery] int offset = 0,
            [FromQuery] int limit = 10
            )
        {
            var services = await _unitOfWork.ServiceRepository.GetPageAsync(offset, limit);
            return Ok(new PageData<ServiceDto>
            {
                Items = services.Item1.Adapt<List<ServiceDto>>(),
                Total = services.Item2
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
        {
            var service = await _unitOfWork.ServiceRepository.GetDetailAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            var result = service.Adapt<ServiceDto>();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateServiceRequest request)
        {
            var service = request.Adapt<Shared.Entities.Service>();
            await _unitOfWork.ServiceRepository.AddAsync(service, true);
            return Created();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateServiceRequest request)
        {
            var existingService = await _unitOfWork.ServiceRepository.GetByIdAsync(id);
            if (existingService == null)
            {
                return NotFound();
            }
            var service = request.Adapt(existingService);
            await _unitOfWork.ServiceRepository.UpdateAsync(service, true);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            try
            {
                var isServiceExists = await _unitOfWork.ServiceRepository.AnyAsync(x => x.Id == id);
                if (!isServiceExists)
                {
                    return NotFound();
                }
                var serviceDeviceIds = await _unitOfWork.ServiceDeviceRepository.GetIdsAsync(sd => sd.ServiceId == id);
                var serviceDetailIds = await _unitOfWork.DeviceDetailRepository.GetIdsAsync(sd => serviceDeviceIds.Contains(sd.ServiceDeviceId));

                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.DeviceDetailRepository.ExecuteDeleteAsync(sd => serviceDetailIds.Contains(sd.Id));
                await _unitOfWork.ServiceDeviceRepository.ExecuteDeleteAsync(sd => sd.ServiceId == id);
                await _unitOfWork.ServiceRepository.ExecuteDeleteAsync(sd => sd.Id == id);
                await _unitOfWork.CommitTransactionAsync();

                return NoContent();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }       
    }
}
