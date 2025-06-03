using Data.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Entities;
using Shared.Filters;
using Shared.Models;
using Shared.Utils;
using System.Security.Claims;

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
        public async Task<IActionResult> GetListAsync([FromQuery] OrderFilter filter)
        {
            var orders = await _unitOfWork.OrderRepository.GetPageByFilterAsync(filter);
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
            var order = request.Adapt<Order>();
            order.Status = OrderStatus.Pending.ToString();
            order.OrderDetails = new List<OrderDetail>();

            foreach (var detail in request.OrderDetails)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = order.Id,
                    DeviceDetailId = detail.DeviceDetailId.Value,
                    Description = detail.Description,
                    MinPrice = detail.MinPrice
                };

                if (detail.ImageFile != null)
                {
                    string? fileExtension = null;
                    if (detail.ImageFile.StartsWith("data:image/"))
                    {
                        var mimeType = detail.ImageFile.Split(';')[0].Replace("data:image/", "");
                        fileExtension = $".{mimeType}";
                    }

                    if (string.IsNullOrEmpty(fileExtension))
                    {
                        continue;
                    }

                    string fileName = $"{Guid.NewGuid().ToString("N")}{fileExtension}";
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "orders", "images");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string filePath = Path.Combine(uploadsFolder, fileName);

                    if (FileHelper.SaveBase64File(detail.ImageFile, filePath))
                    {
                        orderDetail.Image = $"/uploads/orders/images/{fileName}";
                    }
                }
                if (detail.VideoFile != null)
                {
                    string? fileExtension = null;
                    if (detail.VideoFile.StartsWith("data:video/"))
                    {
                        var mimeType = detail.VideoFile.Split(';')[0].Replace("data:video/", "");
                        fileExtension = $".{mimeType}";
                    }

                    if (string.IsNullOrEmpty(fileExtension))
                    {
                        continue;
                    }

                    string fileName = $"{Guid.NewGuid().ToString("N")}{fileExtension}";
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "orders", "videos");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string filePath = Path.Combine(uploadsFolder, fileName);

                    if (FileHelper.SaveBase64File(detail.VideoFile, filePath))
                    {
                        orderDetail.Video = $"/uploads/orders/videos/{fileName}";
                    }
                }

                order.OrderDetails.Add(orderDetail);
            }

            await _unitOfWork.OrderRepository.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return Created();
        }

        [HttpPatch("{id}/rate")]
        public async Task<IActionResult> RateOrderAsync([FromRoute] Guid id, [FromBody] RateOrderRequest request)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound("Order not found.");
            }
            
            request.Adapt(order);
            await _unitOfWork.OrderRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id}/repair")]
        public async Task<IActionResult> RepairOrderAsync([FromRoute] Guid id, [FromBody] RepairOrderRequest request)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound("Order not found.");
            }
            if (order.Status == OrderStatus.Completed.ToString())
            {
                return BadRequest("Cannot repair completed order");
            }

            request.Adapt(order);
            await _unitOfWork.OrderRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("{id}/payment")]
        public async Task<IActionResult> PaymentOrderAsync([FromRoute] Guid id, [FromBody] PaymentOrderRequest request)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound("Order not found.");
            }
            
            request.Adapt(order);
            await _unitOfWork.OrderRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
