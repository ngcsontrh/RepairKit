using FluentValidation;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Validators
{
    public class CreateNotificationRequestValidator : AbstractValidator<CreateNotificationRequest>
    {
        public CreateNotificationRequestValidator()
        {
            RuleFor(x => x.UserIds)
                .NotEmpty().WithMessage("User IDs are required.")
                .Must(userIds => userIds?.Count > 0).WithMessage("At least one user ID is required.");
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required.");
        }
    }

    public class MarkNotificationAsReadRequestValidator : AbstractValidator<MarkNotificationAsReadRequest>
    {
        public MarkNotificationAsReadRequestValidator()
        {
            RuleFor(x => x.NotificationIds)
                .NotEmpty().WithMessage("Notification IDs are required.")
                .Must(ids => ids?.Count > 0).WithMessage("At least one notification ID is required.");
        }
    }

    public class CreateOrderNotificationRequestValidator : AbstractValidator<CreateOrderNotificationRequest>
    {
        public CreateOrderNotificationRequestValidator()
        {
            Include(new CreateNotificationRequestValidator());
            RuleFor(x => x.OrderId).NotNull().WithMessage("Order ID is required.");
        }
    }

    public class CreateRepairmanFormNotificationRequestValidator : AbstractValidator<CreateRepairmanFormNotificationRequest>
    {
        public CreateRepairmanFormNotificationRequestValidator()
        {
            Include(new CreateNotificationRequestValidator());
            RuleFor(x => x.RepairmanFormId).NotNull().WithMessage("Repairman Form ID is required.");
        }
    }
}
