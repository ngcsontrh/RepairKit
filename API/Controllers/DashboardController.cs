using Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("statistics")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetStatisticsAsync()
        {
            var totalUserCount = await _unitOfWork.UserRepository.GetCountAsync();
            var (todayOrderRevenue, thisWeekOrderRevenue, thisMonthorderRevenue) = await _unitOfWork.OrderRepository.GetRevenueAsync();
            var (todayNewUserCount, thisWeekNewUserCount, thisMonthNewUserCount) = await _unitOfWork.UserRepository.GetNewUserCountAsync();
            var (todayNewOrderCount, thisWeekNewOrderCount, thisMonthNewOrderCount) = await _unitOfWork.OrderRepository.GetNewOrderCountAsync();
            var (inProgressOrderStatusCount, completedOrderStatusCount, canceledOrderStatusCount) = await _unitOfWork.OrderRepository.GetOrderStatusCountAsync();
            
            return Ok(new
            {
                TotalUserCount = totalUserCount,
                TodayOrderRevenue = todayOrderRevenue,
                ThisWeekOrderRevenue = thisWeekOrderRevenue,
                ThisMonthOrderRevenue = thisMonthorderRevenue,
                TodayNewUserCount = todayNewUserCount,
                ThisWeekNewUserCount = thisWeekNewUserCount,
                ThisMonthNewUserCount = thisMonthNewUserCount,
                TodayNewOrderCount = todayNewOrderCount,
                ThisWeekNewOrderCount = thisWeekNewOrderCount,
                ThisMonthNewOrderCount = thisMonthNewOrderCount,
                InProgressOrderStatusCount = inProgressOrderStatusCount,
                CompletedOrderStatusCount = completedOrderStatusCount,
                CanceledOrderStatusCount = canceledOrderStatusCount
            });
        }
    }
}
