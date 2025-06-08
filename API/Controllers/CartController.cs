using Data.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Entities;
using Shared.Models;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Retrieves paginated cart details for the authenticated user.
        /// </summary>
        [HttpGet("details")]
        [Authorize]
        public async Task<IActionResult> GetCartDetailsAsync([FromQuery] int offset = 0, [FromQuery] int limit = 10)
        {
            if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            {
                return Unauthorized("User ID not found or invalid.");
            }
            var pageData = await _unitOfWork.CartDetailRepository.GetPageAsync(offset, limit, x => x.Cart!.UserId == userId);
            if (pageData.Item1 == null || !pageData.Item1.Any())
            {
                return NotFound("Cart details not found for the user.");
            }
            return Ok(new PageData<CartDetailDto>
            {
                Items = pageData.Item1.Adapt<List<CartDetailDto>>(),
                Total = pageData.Item2
            });
        }

        /// <summary>
        /// Adds a service device to the user's cart or updates the quantity if it already exists.
        /// </summary>
        [HttpPost("details")]
        [Authorize]
        public async Task<IActionResult> AddToCartAsync(AddToCartRequest request)
        {
            if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            {
                return Unauthorized("User ID not found or invalid.");
            }
            var isDeviceExists = await _unitOfWork.ServiceDeviceRepository.AnyAsync(x => x.Id == request.ServiceDeviceId!.Value);
            if (!isDeviceExists)
            {
                return BadRequest("Service device does not exist.");
            }
            var cart = await _unitOfWork.CartRepository.GetCartByUserIdAsync(userId);            
            if (cart!.CartDetails!.Any(x => x.ServiceDeviceId == request.ServiceDeviceId))
            {
                var existingDetail = cart.CartDetails!.FirstOrDefault(x => x.ServiceDeviceId == request.ServiceDeviceId);
                existingDetail!.Qty += request.Qty;
                await _unitOfWork.CartRepository.SaveChangesAsync();
            }
            else
            {
                cart.Qty += 1;
                var cartDetail = new CartDetail
                {
                    ServiceDeviceId = request.ServiceDeviceId!.Value,
                    Qty = request.Qty,
                    CartId = cart.Id
                };
                await _unitOfWork.CartDetailRepository.AddAsync(cartDetail);
                await _unitOfWork.SaveChangesAsync();
            }

            return Created();
        }

        /// <summary>
        /// Updates the quantity of a specific cart detail.
        /// </summary>
        [HttpPatch("details/{cartDetailId}")]
        [Authorize]
        public async Task<IActionResult> UpdateCartDetailAsync(Guid cartDetailId, [FromBody] UpdateCartDetailRequest request)
        {
            if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            {
                return Unauthorized("User ID not found or invalid.");
            }
            var cart = await _unitOfWork.CartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                return NotFound("Cart not found for the user.");
            }
            var cartDetail = cart.CartDetails!.FirstOrDefault(x => x.Id == cartDetailId);
            if (cartDetail == null)
            {
                return NotFound("Cart detail not found.");
            }
            cartDetail.Qty = request.Qty;
            await _unitOfWork.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Removes a specific cart detail from the user's cart.
        /// </summary>
        [HttpDelete("details/{cartDetailId}")]
        [Authorize]
        public async Task<IActionResult> RemoveCartDetailAsync(Guid cartDetailId)
        {
            if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            {
                return Unauthorized("User ID not found or invalid.");
            }
            var cart = await _unitOfWork.CartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                return NotFound("Cart not found for the user.");
            }
            var cartDetail = cart.CartDetails!.FirstOrDefault(x => x.Id == cartDetailId);
            cart.Qty -= 1;
            await _unitOfWork.CartDetailRepository.DeleteAsync(cartDetail!);
            await _unitOfWork.SaveChangesAsync();
            return NoContent();
        }
    }
}
