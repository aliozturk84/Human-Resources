using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pusula.Training.HealthCare.HRs
{
    public class HRUpdateDto
    {
        [Required]
        public Guid Id { get; set; } = default!;
        [Required]
        [StringLength(HRConsts.FirstNameMaxLength, MinimumLength = HRConsts.FirstNameMinLength)]
        public string FirstName { get; set; } = null!;
        [Required]
        [StringLength(HRConsts.LastNameMaxLength, MinimumLength = HRConsts.LastNameMinLength)]
        public string LastName { get; set; } = null!;
        [Required]
        [StringLength(HRConsts.PhoneNumberMaxLength)]
        public string PhoneNumber { get; set; } = null!;
        [Required]
        public DateTime BirthDate { get; set; }
        public EnumGender Gender { get; set; }

        public Guid DepartmentId { get; set; }
        public Guid TitleId { get; set; }
    }
}
