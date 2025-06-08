using FluentValidation;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Validators
{
    public class CreateRepairmanFormRequestValidator : AbstractValidator<CreateRepairmanFormRequest>
    {
        public CreateRepairmanFormRequestValidator()
        {
            RuleFor(x => x.UserId).NotNull().WithMessage("UserId is required.");
            RuleFor(x => x.Areas).NotEmpty().WithMessage("Areas cannot be empty.");
            RuleFor(x => x.ServiceDeviceId).NotNull().WithMessage("ServiceDeviceId is required.");
            RuleFor(x => x.YearsOfExperience).GreaterThanOrEqualTo(0).WithMessage("YearsOfExperience must be a non-negative integer.");
            RuleFor(x => x.Description).MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
        }
    }

    public class UpdateRepairmanFormStatusRequestValidator : AbstractValidator<UpdateRepairmanFormStatusRequest>
    {
        public UpdateRepairmanFormStatusRequestValidator()
        {
            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required.")
                .Must(status => Constraint.RepairmanFormStatuses.Contains(status!)).WithMessage("Status must be one of the following: " + string.Join(", ", Constraint.RepairmanFormStatuses));
        }
    }
}
