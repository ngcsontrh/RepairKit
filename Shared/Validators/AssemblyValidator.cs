using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Validators
{
    /// <summary>
    /// Class representing the base assembly for validation purposes.
    /// </summary>
    internal class BaseAssembly { }

    /// <summary>
    /// Provides validation logic for instances of the <see cref="BaseAssembly"/> class.
    /// </summary>
    /// <remarks>This class is intended to define validation rules for <see cref="BaseAssembly"/> objects. It
    /// inherits from <see cref="AbstractValidator{T}"/> and should be extended to include specific validation
    /// rules.</remarks>
    internal class BaseAssemblyValidator : AbstractValidator<BaseAssembly>
    {
        public BaseAssemblyValidator()
        {
        }
    }
}
