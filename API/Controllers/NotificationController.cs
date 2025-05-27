using Data.Interfaces;
using Mapster;
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

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserNotificationsAsync(            
            [FromRoute] Guid userId,
            [FromQuery] UserNotificationFilter filter
            )
        {
            var notifications = await _unitOfWork.UserNotificationRepository.GetNotificationPageByFilterAsync(userId, filter);
            return Ok(new PageData<UserNotificationDto>
            {
                Items = notifications.Item1.Adapt<List<UserNotificationDto>>(),
                Total = notifications.Item2
            });
        }

        [HttpPost("user/{userId}/read")]
        public async Task<IActionResult> MarkNotificationAsReadAsync([FromRoute] Guid userId, [FromBody] MarkNotificationAsReadRequest request)
        {
            await _unitOfWork.UserNotificationRepository.MarkNotificationAsReadAsync(userId, request.NotificationIds!);
            return NoContent();
        }

        [HttpPost("system")]
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
