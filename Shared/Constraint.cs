using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public static class Constraint
    {
        public static List<string> UserGenders { get; } = Enum.GetNames<UserGender>().ToList();
        public static List<string> UserRoles { get; } = Enum.GetNames<UserRole>().ToList();
        public static List<string> UserStatuses { get; } = Enum.GetNames<UserStatus>().ToList();
        public static List<string> UserWorkingStatuses { get; } = Enum.GetNames<UserWorkingStatus>().ToList();
    }
}
