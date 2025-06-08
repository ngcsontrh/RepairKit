using FluentValidation;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Validators
{
    public class CreateOrderDetailRequestValidator : AbstractValidator<CreateOrderDetailRequest>
    {
        public CreateOrderDetailRequestValidator()
        {
            RuleFor(x => x.DeviceDetailId).NotNull().WithMessage("DeviceDetailId is required.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");            
            RuleFor(x => x.MinPrice).GreaterThan(0).WithMessage("MinPrice must be greater than 0.");
        }
    }
}
