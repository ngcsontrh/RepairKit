using FluentValidation;
using Shared.Models;

namespace Shared.Validations
{
    public class AuthLoginRequestValidator : AbstractValidator<AuthLoginRequest>
    {
        public AuthLoginRequestValidator()
        {
            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone number is required.")
                .Length(10).WithMessage("Phone number must be exactly 10 digits.")
                .Matches(@"^\d{10}$").WithMessage("Phone number must contain only digits.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }

    public class AuthRefreshTokenRequestValidator : AbstractValidator<AuthRefreshTokenRequest>
    {
        public AuthRefreshTokenRequestValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("RefreshToken is required.");
        }
    }

    public class SendPasswordResetCodeRequestValidator : AbstractValidator<SendPasswordResetCodeRequest>
    {
        public SendPasswordResetCodeRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
        }
    }

    public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Verification code is required.");
            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .MinimumLength(6).WithMessage("New password must be at least 6 characters long.");
        }
    }

    public class ChangePasswordValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.OldPassword)
                .NotEmpty().WithMessage("Old password is required.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is required.")
                .MinimumLength(6).WithMessage("New password must be at least 6 characters long.");
        }
    }

    public class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequest>
    {
        public ForgotPasswordRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");
        }
    }

    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.");

            RuleFor(x => x.DateOfBirth)
                .LessThan(DateTime.Now).WithMessage("Date of birth must be in the past.")
                .When(x => x.DateOfBirth.HasValue);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone number is required.")
                .Length(10).WithMessage("Phone number must be exactly 10 digits.")
                .Matches(@"^\d{10}$").WithMessage("Phone number must contain only digits.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

            RuleFor(x => x.Gender)
                .Must(g => Constraint.UserGenders.Contains(g))
                .WithMessage("Gender must be Male, Female, or Other.");
        }
    }

    public class UpdateCustomerRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateCustomerRequestValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

            RuleFor(x => x.Gender)
                .Must(g => Constraint.UserGenders.Contains(g))
                .WithMessage("Gender must be Male, Female, or Other.");
        }
    }

    public class ChangeUserStatusRequestValidator : AbstractValidator<ChangeUserStatusRequest>
    {
        public ChangeUserStatusRequestValidator()
        {
            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("User status is required.")
                .Must(s => Constraint.UserStatuses.Contains(s))
                .WithMessage("User status must be Active, Inactive, or Disabled.");
        }
    }
}
