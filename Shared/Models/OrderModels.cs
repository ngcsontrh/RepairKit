using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class OrderDto
    {
        public Guid? Id { get; set; }
        public Guid? AddressId { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? RepairmanId { get; set; }
        public string? Status { get; set; }
        public DateTime? RepairDate { get; set; }
        public DateTime? RepairCompleted { get; set; }
        public int? Total { get; set; }
        public bool? PaymentStatus { get; set; }
        public DateTime? PaymentTern { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string? CustomerNote { get; set; }
        public int? RatingNumber { get; set; }
        public string? RatingDescription { get; set; }
        public DateTime? RatingTern { get; set; }
        public DateTime? RatingDate { get; set; }

        public AddressUserDto? Address { get; set; }
        public UserDto? Customer { get; set; }
        public UserDto? Repairman { get; set; }
        public List<OrderDetailDto>? OrderDetails { get; set; }
    }

    public class CreateOrderRequest
    {
        public Guid? AddressId { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? RepairmanId { get; set; }
        public string? CustomerNote { get; set; }
        public List<CreateOrderDetailRequest>? OrderDetails { get; set; }
    }
}
