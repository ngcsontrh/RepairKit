using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public enum UserGender
    {
        Male,
        Female,
        Other
    }

    public enum UserStatus
    {
        Active,
        Inactive,
        Disabled
    }

    public enum UserRole
    {
        Customer,
        Repairman,
        Admin
    }

    public enum UserWorkingStatus
    {
        Online,
        Offline,
    }
}
