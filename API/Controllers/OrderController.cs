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

        /// <summary>
        /// Retrieves a paginated list of orders based on the provided filter.
        /// </summary>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetListAsync([FromQuery] OrderFilter filter)
        {
            var orders = await _unitOfWork.OrderRepository.GetPageByFilterAsync(filter);
            return Ok(new PageData<OrderDto>
            {
                Items = orders.Item1.Adapt<List<OrderDto>>(),
                Total = orders.Item2
            });
        }

        /// <summary>
        /// Retrieves the details of a specific order by its ID.
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
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

        /// <summary>
        /// Creates a new order with the provided details and files.
        /// </summary>
        [HttpPost]
        [Authorize]
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

        /// <summary>
        /// Rates a specific order by its ID.
        /// </summary>
        [HttpPatch("{id}/rate")]
        [Authorize]
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

        /// <summary>
        /// Updates the repair information for a specific order. Admin and Repairman roles only.
        /// </summary>
        [HttpPatch("{id}/repair")]
        [Authorize(Roles = "Admin,Repairman")]
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

        /// <summary>
        /// Updates the payment information for a specific order. Admin and Repairman roles only.
        /// </summary>
        [HttpPost("{id}/payment")]
        [Authorize(Roles = "Admin,Repairman")]
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

        /// <summary>
        /// Cancels a specific order if it is still pending.
        /// </summary>
        [HttpPost("{id}/cancel-order")]
        public async Task<IActionResult> CancelOrderAsync([FromRoute] Guid id)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound("Order not found.");
            }
            if (order.Status != OrderStatus.Pending.ToString())
            {
                return BadRequest("Only pending orders can be canceled.");
            }
            order.Status = OrderStatus.Canceled.ToString();
            await _unitOfWork.OrderRepository.SaveChangesAsync();
            return NoContent();
        }
    }
}
