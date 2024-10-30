using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pusula.Training.HealthCare.HRs
{
    public class HRExcelDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public EnumGender Gender { get; set; }
        public string Department { get; set; } = null!;
        public string Title { get; set; } = null!;
    }
}
