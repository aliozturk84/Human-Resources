using JetBrains.Annotations;
using Pusula.Training.HealthCare.Doctors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp;

namespace Pusula.Training.HealthCare.HRs
{
    public class HR : FullAuditedAggregateRoot<Guid>
    {
        [NotNull]
        public virtual string FirstName { get; set; }
        [NotNull]
        public virtual string LastName { get; set; }
        [NotNull]
        public virtual string PhoneNumber { get; set; }
        [NotNull]
        public virtual DateTime BirthDate { get; set; }
        [CanBeNull]
        public virtual EnumGender Gender { get; set; }

        public virtual Guid DepartmentId { get; set; }
        public virtual Guid TitleId { get; set; }

        protected HR()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            PhoneNumber = string.Empty;
            BirthDate = DateTime.Now;
        }

        public HR(Guid id, Guid titleId, Guid departmentId, string firstName, string lastName, string phoneNumber, DateTime birthDate, EnumGender gender = default)
        {
            Check.NotNullOrWhiteSpace(firstName, nameof(firstName), HRConsts.FirstNameMaxLength, HRConsts.FirstNameMinLength);
            Check.NotNullOrWhiteSpace(lastName, nameof(lastName), HRConsts.LastNameMaxLength, HRConsts.LastNameMinLength);
            Check.NotNullOrWhiteSpace(phoneNumber, nameof(phoneNumber), HRConsts.PhoneNumberMaxLength);
            Check.NotNull(birthDate, nameof(birthDate));
            Check.Range((int)gender, nameof(gender), 1, 2);
            Check.NotNullOrWhiteSpace(departmentId.ToString(), nameof(departmentId));
            Check.NotNullOrWhiteSpace(titleId.ToString(), nameof(titleId));


            Id = id;
            TitleId = titleId;
            DepartmentId = departmentId;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            BirthDate = birthDate;
            Gender = gender;
        }
    }
}
