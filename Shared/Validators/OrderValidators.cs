using FluentValidation;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Validators
{
    public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderRequestValidator()
        {
            RuleFor(x => x.AddressId).NotNull().WithMessage("AddressId is required.");
            RuleFor(x => x.CustomerId).NotNull().WithMessage("CustomerId is required.");
            RuleFor(x => x.OrderDetails).NotEmpty().WithMessage("OrderDetails cannot be empty.");
            RuleForEach(x => x.OrderDetails).SetValidator(new CreateOrderDetailRequestValidator());
        }
    }
}
