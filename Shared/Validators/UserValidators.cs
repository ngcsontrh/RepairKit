using FluentValidation;
using Shared.Models;

namespace Shared.Validations
{
    public class AuthLoginRequestValidator : AbstractValidator<AuthLoginRequest>
    {
        public AuthLoginRequestValidator()
        {
            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Số điện thoại không được để trống.")
                .Length(10).WithMessage("Số điện thoại phải gồm 10 chữ số.")
                .Matches(@"^\d{10}$").WithMessage("Số điện thoại chỉ được chứa chữ số.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Mật khẩu không được để trống.");
        }
    }

    public class ChangePasswordValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email không được để trống.")
                .EmailAddress().WithMessage("Định dạng email không hợp lệ.");

            RuleFor(x => x.OldPassword)
                .NotEmpty().WithMessage("Vui lòng nhập mật khẩu cũ.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Vui lòng nhập mật khẩu mới.")
                .MinimumLength(6).WithMessage("Mật khẩu mới phải có ít nhất 6 ký tự.");
        }
    }

    public class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequest>
    {
        public ForgotPasswordRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email không được để trống.")
                .EmailAddress().WithMessage("Định dạng email không hợp lệ.");
        }
    }

    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Họ tên không được để trống.");

            RuleFor(x => x.DateOfBirth)
                .LessThan(DateTime.Now).WithMessage("Ngày sinh phải nhỏ hơn ngày hiện tại.")
                .When(x => x.DateOfBirth.HasValue);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email không được để trống.")
                .EmailAddress().WithMessage("Định dạng email không hợp lệ.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Số điện thoại không được để trống.")
                .Length(10).WithMessage("Số điện thoại phải gồm 10 chữ số.")
                .Matches(@"^\d{10}$").WithMessage("Số điện thoại chỉ được chứa chữ số.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Mật khẩu không được để trống.")
                .MinimumLength(6).WithMessage("Mật khẩu phải có ít nhất 6 ký tự.");

            RuleFor(x => x.Gender)
                .Must(g => Constraint.UserGenders.Contains(g))
                .WithMessage("Giới tính phải là Male, Female hoặc Other");
        }
    }

    public class UpdateCustomerRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateCustomerRequestValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Họ tên không được để trống.");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Định dạng email không hợp lệ.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Mật khẩu không được để trống.")
                .MinimumLength(6).WithMessage("Mật khẩu phải có ít nhất 6 ký tự.");

            RuleFor(x => x.Gender)
                .Must(g => Constraint.UserGenders.Contains(g))
                .WithMessage("Giới tính phải là Male, Female hoặc Other");
        }
    }

    public class ChangeUserStatusRequestValidator : AbstractValidator<ChangeUserStatusRequest>
    {
        public ChangeUserStatusRequestValidator()
        {
            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Trạng thái người dùng không được để trống.")
                .Must(s => Constraint.UserStatuses.Contains(s))
                .WithMessage("Trạng thái người dùng phải là Active, Inactive hoặc Disabled.");
        }
    }
}
