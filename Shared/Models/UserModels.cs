using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class UserDto
    {
        public Guid? Id { get; set; }
        public string? FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Avatar { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Status { get; set; }
        public string? Role { get; set; }
        public int? Average { get; set; }
        public int? ReviewCount { get; set; }
        public string? Bio { get; set; }
        public string? WorkingStatus { get; set; }
    }

    public class AuthLoginRequest
    {
        public string? Phone { get; set; }
        public string? Password { get; set; }
    }

    public class AuthLoginResponse
    {
        public string? AccessToken { get; set; }
    }
   
    public class ChangePasswordRequest
    {
        public string? Email { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
    }

    public class ForgotPasswordRequest
    {
        public string? Email { get; set; }
    }

    public class CreateUserRequest
    {
        public string? FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public IFormFile? AvatarFile { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public int? Average { get; set; }
        public int? ReviewCount { get; set; }
        public string? Bio { get; set; }
        public string? WorkingStatus { get; set; }
    }

    public class UpdateUserRequest
    {
        public string? FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public IFormFile? AvatarFile { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public int? Average { get; set; }
        public int? ReviewCount { get; set; }
        public string? Bio { get; set; }
        public string? WorkingStatus { get; set; }
    }

    public class ChangeUserStatusRequest
    {
        public string? Status { get; set; }
    }
}
