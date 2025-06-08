using FluentValidation;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Validators
{
    public class CreateServiceValidator : AbstractValidator<CreateServiceRequest>
    {
        public CreateServiceValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
        }
    }

    public class UpdateServiceValidator : AbstractValidator<UpdateServiceRequest>
    {
        public UpdateServiceValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
        }
    }
}
