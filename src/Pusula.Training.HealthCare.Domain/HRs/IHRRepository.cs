using Pusula.Training.HealthCare.Doctors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Pusula.Training.HealthCare.HRs
{
    public interface IHRRepository : IRepository<HR, Guid>
    {
        Task DeleteAllAsync(
            string? filterText = null,
            string? firstName = null,
            string? lastName = null,
            string? phoneNumber = null,
            DateTime? birthDateMin = null,
            DateTime? birthDateMax = null,
            EnumGender? gender = null,
            Guid? departmentId = null,
            Guid? titleId = null,
            CancellationToken cancellationToken = default);

        Task<HRWithNavigationProperties> GetWithNavigationPropertiesAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<List<HRWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string? filterText = null,
            string? firstName = null,
            string? lastName = null,
            string? phoneNumber = null,
            DateTime? birthDateMin = null,
            DateTime? birthDateMax = null,
            EnumGender? gender = null,
            Guid? departmentId = null,
            Guid? titleId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default);


        Task<List<HR>> GetListAsync(
            string? filterText = null,
            string? firstName = null,
            string? lastName = null,
            string? phoneNumber = null,
            DateTime? birthDateMin = null,
            DateTime? birthDateMax = null,
            EnumGender? gender = null,
            Guid? departmentId = null,
            Guid? titleId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default);

        Task<long> GetCountAsync(
            string? filterText = null,
            string? firstName = null,
            string? lastName = null,
            string? phoneNumber = null,
            DateTime? birthDateMin = null,
            DateTime? birthDateMax = null,
            EnumGender? gender = null,
            Guid? departmentId = null,
            Guid? titleId = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default);
    }
}
