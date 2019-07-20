using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity_example.Utilities
{
    public static class SeedRoles
    {
        public enum Roles
        {
            Admin,
            Manager,
            Member
        }
        public static string AdminRole = "Admin";
        public static string MemberRole = "Member";
        public static string ManagerRole = "Manager";
        
    }
}
