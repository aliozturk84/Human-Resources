using Pusula.Training.HealthCare.Doctors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;
using Volo.Abp;

namespace Pusula.Training.HealthCare.HRs
{
    public class HRManager(IHRRepository hrRepository) : DomainService
    {
        public virtual async Task<HR> CreateAsync(
            Guid titleId, Guid departmentId, string firstName, string lastName, string phoneNumber, DateTime birthDate, EnumGender gender)
        {
            Check.NotNullOrWhiteSpace(firstName, nameof(firstName), HRConsts.FirstNameMaxLength, HRConsts.FirstNameMinLength);
            Check.NotNullOrWhiteSpace(lastName, nameof(lastName), HRConsts.LastNameMaxLength, HRConsts.LastNameMinLength);
            Check.NotNullOrWhiteSpace(phoneNumber, nameof(phoneNumber), HRConsts.PhoneNumberMaxLength);
            Check.NotNull(birthDate, nameof(birthDate));
            Check.Range((int)gender, nameof(gender), 1, 2);
            Check.NotNullOrWhiteSpace(departmentId.ToString(), nameof(departmentId));
            Check.NotNullOrWhiteSpace(titleId.ToString(), nameof(titleId));
            var hr = new HR(
                GuidGenerator.Create(), titleId, departmentId, firstName, lastName, phoneNumber, birthDate, gender
                );

            return await hrRepository.InsertAsync(hr);
        }

        public virtual async Task<HR> UpdateAsync(
            Guid id, Guid titleId, Guid departmentId, string firstName, string lastName, string phoneNumber, DateTime birthDate, EnumGender gender)

        {
            Check.NotNullOrWhiteSpace(firstName, nameof(firstName), HRConsts.FirstNameMaxLength, HRConsts.FirstNameMinLength);
            Check.NotNullOrWhiteSpace(lastName, nameof(lastName), HRConsts.LastNameMaxLength, HRConsts.LastNameMinLength);
            Check.NotNullOrWhiteSpace(phoneNumber, nameof(phoneNumber), HRConsts.PhoneNumberMaxLength);
            Check.NotNull(birthDate, nameof(birthDate));
            Check.Range((int)gender, nameof(gender), 1, 2);
            Check.NotNullOrWhiteSpace(departmentId.ToString(), nameof(departmentId));
            Check.NotNullOrWhiteSpace(titleId.ToString(), nameof(titleId));

            var hr = await hrRepository.GetAsync(id);

            hr.TitleId = titleId;
            hr.DepartmentId = departmentId;
            hr.FirstName = firstName;
            hr.LastName = lastName;
            hr.PhoneNumber = phoneNumber;
            hr.BirthDate = birthDate;
            hr.Gender = gender;

            return await hrRepository.UpdateAsync(hr);
        }

    }
}
