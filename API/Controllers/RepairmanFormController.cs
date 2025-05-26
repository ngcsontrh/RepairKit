using Data.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Entities;
using Shared.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepairmanFormController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public RepairmanFormController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetListAsync(
            [FromQuery] int offset = 0,
            [FromQuery] int limit = 10
            )
        {
            var pageData = await _unitOfWork.RepairmanFormRepository.GetPageAsync(offset, limit);
            return Ok(new PageData<RepairmanFormDto>
            {
                Items = pageData.Item1.Adapt<List<RepairmanFormDto>>(),
                Total = pageData.Item2
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailAsync([FromRoute] Guid id)
        {
            var repairman = await _unitOfWork.RepairmanFormRepository.GetDetailAsync(id);
            if (repairman == null)
            {
                return NotFound();
            }
            return Ok(repairman);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] CreateRepairmanFormRequest request)
        {
            var isRepairmanExists = await _unitOfWork.RepairmanFormRepository.AnyAsync(x => x.UserId == request.UserId!.Value);
            if (isRepairmanExists)
            {
                return BadRequest("A repairman with this user ID already exists.");
            }
            var repairmanForm = request.Adapt<Shared.Entities.RepairmanForm>();
            var repairmanFormDetail = request.Adapt<Shared.Entities.RepairmanFormDetail>();
            repairmanFormDetail.RepairmanFormId = repairmanForm.Id;
            if (request.DegreeFile != null)
            {
                string fileExtension = Path.GetExtension(request.DegreeFile.FileName);
                string fileName = $"{Guid.NewGuid().ToString("N")}{fileExtension}";

                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "degrees");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await request.DegreeFile.CopyToAsync(fileStream);
                }

                repairmanFormDetail.Degree = $"/uploads/degrees/{fileName}";
            }
            
            await _unitOfWork.RepairmanFormRepository.AddAsync(repairmanForm);
            await _unitOfWork.RepairmanFormDetailRepository.AddAsync(repairmanFormDetail);
            await _unitOfWork.SaveChangesAsync();

            return Created();
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatusAsync([FromRoute] Guid id, [FromBody] UpdateRepairmanFormStatusRequest request)
        {
            var repairmanForm = await _unitOfWork.RepairmanFormRepository.GetByIdAsync(id);
            if (repairmanForm == null)
            {
                return NotFound();
            }
            repairmanForm.Status = request.Status;
            await _unitOfWork.RepairmanFormRepository.UpdateAsync(repairmanForm, true);
            return NoContent();
        }
    }
}
