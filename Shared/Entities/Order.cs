﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.CreateVersion7();
        public Guid AddressId { get; set; }
        public Guid CustomerId { get; set; }
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
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        public AddressUser? Address { get; set; }
        public User? Customer { get; set; }
        public User? Repairman { get; set; }
        public List<OrderDetail>? OrderDetails { get; set; }
    }
}