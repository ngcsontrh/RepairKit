using FluentValidation;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Validators
{
    public class DeviceDetailRequestValidator : AbstractValidator<CreateDeviceDetailRequest>
    {
        public DeviceDetailRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.MinPrice).GreaterThan(0).WithMessage("MinPrice must be greater than 0.");
        }
    }

    public class UpdateDeviceDetailRequestValidator : AbstractValidator<UpdateDeviceDetailRequest>
    {
        public UpdateDeviceDetailRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.MinPrice).GreaterThan(0).WithMessage("MinPrice must be greater than 0.");
        }
    }
}
