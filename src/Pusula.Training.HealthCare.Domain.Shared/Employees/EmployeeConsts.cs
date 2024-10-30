using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pusula.Training.HealthCare.Employees
{
    public static class EmployeeConsts
    {
        private const string DefaultSorting = "{0}FirstName asc";

        public static string GetDefaultSorting(bool withEntityName) => string.Format(DefaultSorting, withEntityName ? "Employee." : string.Empty);

        public const int FirstNameMinLength = 2;
        public const int FirstNameMaxLength = 50;
        public const int LastNameMinLength = 2;
        public const int LastNameMaxLength = 50;
        public const int PhoneNumberMaxLength = 10;

        public const int LeaveDaysMin = 0;
        public const int LeaveDaysMax = 365;
        public const decimal SalaryMin = 0;
        public const decimal SalaryMax = 1000000;
    }
}
