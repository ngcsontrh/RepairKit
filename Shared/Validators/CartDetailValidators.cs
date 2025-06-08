using FluentValidation;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Validators
{
    public class AddToCartRequestValidator : AbstractValidator<AddToCartRequest>
    {
        public AddToCartRequestValidator()
        {
            RuleFor(x => x.ServiceDeviceId).NotNull().WithMessage("ServiceDeviceId is required.");
            RuleFor(x => x.Qty).GreaterThan(0).WithMessage("Qty must be greater than 0.");
        }
    }

    public class UpdateCartDetailRequestValidator : AbstractValidator<UpdateCartDetailRequest>
    {
        public UpdateCartDetailRequestValidator()
        {
            RuleFor(x => x.Qty).GreaterThan(0).WithMessage("Qty must be greater than 0.");
        }
    }
}
