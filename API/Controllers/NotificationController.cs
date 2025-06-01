using Data.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Entities;
using Shared.Filters;
using Shared.Models;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public NotificationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserNotificationsAsync(            
            [FromQuery] UserNotificationFilter filter
            )
        {
            if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            {
                return Unauthorized("User ID not found or invalid.");
            }
            var notifications = await _unitOfWork.UserNotificationRepository.GetNotificationPageByFilterAsync(userId, filter);
            return Ok(new PageData<UserNotificationDto>
            {
                Items = notifications.Item1.Adapt<List<UserNotificationDto>>(),
                Total = notifications.Item2
            });
        }

        [HttpPost("read")]
        [Authorize]
        public async Task<IActionResult> MarkNotificationAsReadAsync([FromBody] MarkNotificationAsReadRequest request)
        {
            if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            {
                return Unauthorized("User ID not found or invalid.");
            }
            await _unitOfWork.UserNotificationRepository.MarkNotificationAsReadAsync(userId, request.NotificationIds!);
            return NoContent();
        }

        [HttpPost("system")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateSystemNotificationAsync([FromBody] CreateNotificationRequest request)
        {
            var notification = new Shared.Entities.Notification
            {
                Description = request.Description,
                Title = request.Title,
                Type = NotificationType.System.ToString(),
            };
            var userNotification = request.UserIds!.Select(userId => new UserNotification
            {
                UserId = userId,
                NotificationId = notification.Id
            });

            await _unitOfWork.NotificationRepository.AddAsync(notification);
            await _unitOfWork.UserNotificationRepository.AddAsync(userNotification);
            await _unitOfWork.SaveChangesAsync();

            return Created();
        }

        [HttpPost("order")]
        [Authorize(Roles = "Admin,Repairman")]
        public async Task<IActionResult> CreateOrderNotificationAsync(
            [FromBody] CreateOrderNotificationRequest request)
        {
            var notification = request.Adapt<Notification>();
            notification.Type = NotificationType.Order.ToString();
            var userNotification = request.UserIds!.Select(userId => new UserNotification
            {
                UserId = userId,
                NotificationId = notification.Id
            });

            await _unitOfWork.NotificationRepository.AddAsync(notification);
            await _unitOfWork.UserNotificationRepository.AddAsync(userNotification);
            await _unitOfWork.SaveChangesAsync();

            return Created();
        }

        [HttpPost("register")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRepairmanFormNotificationAsync(
            [FromBody] CreateRepairmanFormNotificationRequest request)
        {
            var notification = request.Adapt<Notification>();
            notification.Type = NotificationType.Register.ToString();
            var userNotification = request.UserIds!.Select(userId => new UserNotification
            {
                UserId = userId,
                NotificationId = notification.Id
            });

            await _unitOfWork.NotificationRepository.AddAsync(notification);
            await _unitOfWork.UserNotificationRepository.AddAsync(userNotification);
            await _unitOfWork.SaveChangesAsync();

            return Created();
        }
    }
}
