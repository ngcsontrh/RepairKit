using Data.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Entities;
using Shared.Filters;
using Shared.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceDetailController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeviceDetailController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }        
        
        [HttpGet]
        public async Task<IActionResult> GetListAsync([FromQuery] DeviceDetailFilter filter)
        {
            var result = await _unitOfWork.DeviceDetailRepository.GetListWithFilterAsync(filter);
            return Ok(new PageData<DeviceDetailDto>
            {
                Items = result.Item1.Adapt<List<DeviceDetailDto>>(),
                Total = result.Item2
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
        {
            var deviceDetail = await _unitOfWork.DeviceDetailRepository.GetByIdAsync(id);
            if (deviceDetail == null)
            {
                return NotFound();
            }

            var result = deviceDetail.Adapt<DeviceDetailDto>();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateDeviceDetailRequest request)
        {

            var serviceDeviceExists = await _unitOfWork.ServiceDeviceRepository.AnyAsync(sd => sd.Id == request.ServiceDeviceId.Value);
            if (!serviceDeviceExists)
            {
                return BadRequest("ServiceDevice not found");
            }
            var deviceDetail = request.Adapt<DeviceDetail>();
            await _unitOfWork.DeviceDetailRepository.AddAsync(deviceDetail, true);

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateDeviceDetailRequest request)
        {
            var existingDeviceDetail = await _unitOfWork.DeviceDetailRepository.GetByIdAsync(id);
            if (existingDeviceDetail == null)
            {
                return NotFound();
            }

            request.Adapt(existingDeviceDetail);
            await _unitOfWork.DeviceDetailRepository.UpdateAsync(existingDeviceDetail, true);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            try
            {
                var deviceDetailExists = await _unitOfWork.DeviceDetailRepository.AnyAsync(dd => dd.Id == id);
                if (!deviceDetailExists)
                {
                    return NotFound();
                }

                await _unitOfWork.DeviceDetailRepository.ExecuteDeleteAsync(dd => dd.Id == id);
                return NoContent();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
