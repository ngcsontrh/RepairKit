using FluentValidation;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Validators
{
    public class CreateServiceDeviceRequestValidator : AbstractValidator<CreateServiceDeviceRequest>
    {
        public CreateServiceDeviceRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
        }
    }

    public class UpdateServiceDeviceRequestValidator : AbstractValidator<UpdateServiceDeviceRequest>
    {
        public UpdateServiceDeviceRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
        }
    }
}
