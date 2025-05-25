using Data.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Entities;
using Shared.Filters;
using Shared.Models;
using System.IO;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IAddressUserRepository _addressUserRepository;

        public UserController(
            IUserRepository userRepository,
            IAddressUserRepository addressUserRepository
            )
        {
            _userRepository = userRepository;
            _addressUserRepository = addressUserRepository;
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

            await _userRepository.AddAsync(user, true);
            return Created();
        }

        [HttpGet("{id}")]
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
        public async Task<IActionResult> GetListUserAsync([FromQuery] UserFilter request)
        {
            var users = await _userRepository.GetListWithFilterAsync(request);
            return Ok(users.Adapt<List<UserDto>>());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserAsync(
            [FromRoute] Guid id,
            [FromForm] UpdateUserRequest request)
        {
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
        public async Task<IActionResult> ChangePasswordAsync(
            [FromRoute] Guid id,
            [FromBody] ChangePasswordRequest request)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            if (user.Email != request.Email || PasswordHelper.VerifyPassword(request.OldPassword!, user.Password))
            {
                return BadRequest("Invalid email or password.");
            }
            user.Password = PasswordHelper.HashPassword(request.NewPassword!);
            await _userRepository.UpdateAsync(user, true);
            return NoContent();
        }

        [HttpPatch("{id}/status")]
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

        [HttpPost("{userId}/address")]
        public async Task<IActionResult> AddAddressAsync(
            [FromRoute] Guid userId,
            [FromBody] CreateAddressUserRequest request)
        {
            var user = await _userRepository.AnyAsync(x => x.Id == userId);
            if (!user)
            {
                return NotFound();
            }
            var address = request.Adapt<AddressUser>();
            address.UserId = userId;
            await _addressUserRepository.AddAsync(address, true);
            return Created();
        }

        [HttpPut("{userId}/address/{addressId}")]
        public async Task<IActionResult> UpdateAddressAsync(
            [FromRoute] Guid userId,
            [FromRoute] Guid addressId,
            [FromBody] UpdateAddressUserRequest request)
        {
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
        public async Task<IActionResult> DeleteAddressAsync(
            [FromRoute] Guid userId,
            [FromRoute] Guid addressId)
        {
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
