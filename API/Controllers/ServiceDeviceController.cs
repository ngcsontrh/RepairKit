using Data.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Shared.Entities;
using Shared.Filters;
using Shared.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceDeviceController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceDeviceController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetListAsync([FromQuery] ServiceDeviceFilter filter)
        {
            var serviceDevices = await _unitOfWork.ServiceDeviceRepository.GetListWithFilterAsync(filter);
            return Ok(new PageData<ServiceDeviceDto>
            {
                Items = serviceDevices.ServiceDevices.Adapt<List<ServiceDeviceDto>>(),
                Total = serviceDevices.TotalCount
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
        {
            var serviceDevice = await _unitOfWork.ServiceDeviceRepository.GetByIdAsync(id);
            if (serviceDevice == null)
            {
                return NotFound();
            }
            var result = serviceDevice.Adapt<ServiceDeviceDto>();
            return Ok(result);
        }        
        
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateServiceDeviceRequest request)
        {
            if (!request.ServiceId.HasValue)
            {
                return BadRequest("ServiceId is required");
            }

            var serviceDevice = request.Adapt<ServiceDevice>();
            
            await _unitOfWork.ServiceDeviceRepository.AddAsync(serviceDevice, true);
            
            return Created();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateServiceDeviceRequest request)
        {
            var existingServiceDevice = await _unitOfWork.ServiceDeviceRepository.GetByIdAsync(id);
            if (existingServiceDevice == null)
            {
                return NotFound();
            }
            
            request.Adapt(existingServiceDevice);
            await _unitOfWork.ServiceDeviceRepository.UpdateAsync(existingServiceDevice, true);
            
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            try
            {
                var isServiceDeviceExists = await _unitOfWork.ServiceDeviceRepository.AnyAsync(x => x.Id == id);
                if (!isServiceDeviceExists)
                {
                    return NotFound();
                }
                
                var deviceDetailIds = await _unitOfWork.DeviceDetailRepository.GetIdsAsync(dd => dd.ServiceDeviceId == id);
                
                await _unitOfWork.BeginTransactionAsync();
                
                if (deviceDetailIds.Any())
                {
                    await _unitOfWork.DeviceDetailRepository.ExecuteDeleteAsync(dd => deviceDetailIds.Contains(dd.Id));
                }
                
                await _unitOfWork.ServiceDeviceRepository.ExecuteDeleteAsync(sd => sd.Id == id);
                
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
