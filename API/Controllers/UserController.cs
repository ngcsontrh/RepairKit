using Data.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Entities;
using Shared.Filters;
using Shared.Models;
using System.IO;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IAddressUserRepository _addressUserRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserController(
            IUserRepository userRepository,
            IAddressUserRepository addressUserRepository,
            IUnitOfWork unitOfWork
            )
        {
            _userRepository = userRepository;
            _addressUserRepository = addressUserRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserAsync([FromForm] CreateUserRequest request)
        {
            var user = request.Adapt<User>();
            user.Password = PasswordHelper.HashPassword(request.Password!);

            if (request.AvatarFile != null)
            {
                string fileExtension = Path.GetExtension(request.AvatarFile.FileName);
                string fileName = $"{Guid.NewGuid().ToString("N")}{fileExtension}";

                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "avatars");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await request.AvatarFile.CopyToAsync(fileStream);
                }

                user.Avatar = $"/uploads/avatars/{fileName}";
            }

            await _unitOfWork.UserRepository.AddAsync(user, true);
            await _unitOfWork.CartRepository.AddAsync(new Cart
            {
                UserId = user.Id,
                Qty = 0
            }, true);
            return Created();
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserByIdAsync([FromRoute] Guid id)
        {
            var user = await _userRepository.GetDetailAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user.Adapt<UserDto>());
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetListUserAsync([FromQuery] UserFilter request)
        {
            var users = await _userRepository.GetListWithFilterAsync(request);
            return Ok(users.Adapt<List<UserDto>>());
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUserAsync(
            [FromRoute] Guid id,
            [FromForm] UpdateUserRequest request)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool isAdmin = User.IsInRole("Admin");
            if (!isAdmin && currentUserId != id.ToString())
            {
                return Forbid("You do not have permission to update this user.");
            }

            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            request.Adapt(user);

            if (request.AvatarFile != null)
            {
                string fileExtension = Path.GetExtension(request.AvatarFile.FileName);
                string fileName = $"{Guid.NewGuid().ToString("N")}{fileExtension}";

                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "avatars");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await request.AvatarFile.CopyToAsync(fileStream);
                }
                user.Avatar = $"/uploads/avatars/{fileName}";
            }

            await _userRepository.UpdateAsync(user, true);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUserAsync([FromRoute] Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }            
            await _userRepository.DeleteAsync(user, true);
            return NoContent();
        }        

        [HttpPatch("{id}/password")]
        [Authorize]
        public async Task<IActionResult> ChangePasswordAsync(
            [FromRoute] Guid id,
            [FromBody] ChangePasswordRequest request)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool isAdmin = User.IsInRole("Admin");
            if (!isAdmin && currentUserId != id.ToString())
            {
                return Forbid("You do not have permission to change this user's password.");
            }

            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            if (!PasswordHelper.VerifyPassword(request.OldPassword!, user.Password))
            {
                return BadRequest("Invalid email or password.");
            }
            user.Password = PasswordHelper.HashPassword(request.NewPassword!);
            await _userRepository.UpdateAsync(user, true);
            return NoContent();
        }

        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeStatusAsync(
            [FromRoute] Guid id,
            [FromBody] ChangeUserStatusRequest request)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            user.Status = request.Status;
            await _userRepository.UpdateAsync(user, true);
            return NoContent();
        }

        [HttpPatch("{id}/role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeRoleAsync(
            [FromRoute] Guid id,
            [FromBody] ChangeUserRoleRequest request)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            user.Role = request.Role;
            await _userRepository.UpdateAsync(user, true);
            return NoContent();
        }

        [HttpGet("{userId}/address")]
        [Authorize]
        public async Task<IActionResult> GetAddressAsync([FromRoute] Guid userId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool isAdmin = User.IsInRole("Admin");
            if (!isAdmin && currentUserId != userId.ToString())
            {
                return Forbid("You do not have permission to view this user's address.");
            }
            var user = await _userRepository.AnyAsync(x => x.Id == userId);
            if (!user)
            {
                return NotFound();
            }
            var addresses = await _addressUserRepository.GetByUserIdAsync(userId);
            return Ok(addresses.Adapt<List<AddressUserDto>>());
        }

        [HttpPost("{userId}/address")]
        [Authorize]
        public async Task<IActionResult> AddAddressAsync(
            [FromRoute] Guid userId,
            [FromBody] CreateAddressUserRequest request)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool isAdmin = User.IsInRole("Admin");
            if (!isAdmin && currentUserId != userId.ToString())
            {
                return Forbid("You do not have permission to add an address for this user.");
            }

            var user = await _userRepository.AnyAsync(x => x.Id == userId);
            if (!user)
            {
                return NotFound();
            }

            var address = request.Adapt<AddressUser>();
            address.UserId = userId;
            await _addressUserRepository.AddAsync(address, true);

            if (request.AddressMain == true)
            {
                await _addressUserRepository.UpdateMainAddressAsync(userId, address.Id);
            }            
            return Created();
        }

        [HttpPut("{userId}/address/{addressId}")]
        [Authorize]
        public async Task<IActionResult> UpdateAddressAsync(
            [FromRoute] Guid userId,
            [FromRoute] Guid addressId,
            [FromBody] UpdateAddressUserRequest request)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool isAdmin = User.IsInRole("Admin");
            if (!isAdmin && currentUserId != userId.ToString())
            {
                return Forbid("You do not have permission to update this user's address.");
            }

            var user = await _userRepository.AnyAsync(x => x.Id == userId);
            if (!user)
            {
                return NotFound();
            }
            var address = await _addressUserRepository.GetByIdAsync(addressId);
            if (address == null || address.UserId != userId)
            {
                return NotFound();
            }
            request.Adapt(address);
            await _addressUserRepository.UpdateAsync(address, true);
            return NoContent();
        }

        [HttpDelete("{userId}/address/{addressId}")]
        [Authorize]
        public async Task<IActionResult> DeleteAddressAsync(
            [FromRoute] Guid userId,
            [FromRoute] Guid addressId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool isAdmin = User.IsInRole("Admin");
            if (!isAdmin && currentUserId != userId.ToString())
            {
                return Forbid("You do not have permission to delete this user's address.");
            }
            var user = await _userRepository.AnyAsync(x => x.Id == userId);
            if (!user)
            {
                return NotFound();
            }
            var address = await _addressUserRepository.GetByIdAsync(addressId);
            if (address == null || address.UserId != userId)
            {
                return NotFound();
            }
            await _addressUserRepository.DeleteAsync(address, true);
            return NoContent();
        }
    }
}
