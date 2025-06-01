using Data.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetListAsync(
            [FromQuery] int offset = 0,
            [FromQuery] int limit = 10
            )
        {
            var orders = await _unitOfWork.OrderRepository.GetPageAsync(offset, limit);
            return Ok(new PageData<OrderDto>
            {
                Items = orders.Item1.Adapt<List<OrderDto>>(),
                Total = orders.Item2
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailAsync([FromRoute] Guid id)
        {
            var order = await _unitOfWork.OrderRepository.GetDetailAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            var result = order.Adapt<OrderDto>();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateOrderRequest request)
        {
            var order = request.Adapt<Shared.Entities.Order>();
            await _unitOfWork.OrderRepository.AddAsync(order, true);
            return Created();
        }
    }
}
