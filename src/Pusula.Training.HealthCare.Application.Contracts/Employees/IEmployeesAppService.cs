using Pusula.Training.HealthCare.Doctors;
using Pusula.Training.HealthCare.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;

namespace Pusula.Training.HealthCare.Employees
{
    public interface IEmployeesAppService : IApplicationService
    {
        Task<PagedResultDto<EmployeeWithNavigationPropertiesDto>> GetListAsync(GetEmployeesInput input);

        Task<EmployeeWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id);

        Task<EmployeeDto> GetAsync(Guid id);

        Task<PagedResultDto<LookupDto<Guid>>> GetTitleLookupAsync(LookupRequestDto input);

        Task<PagedResultDto<LookupDto<Guid>>> GetDepartmentLookupAsync(LookupRequestDto input);

        Task DeleteAsync(Guid id);

        Task<EmployeeDto> CreateAsync(EmployeeCreateDto input);

        Task<EmployeeDto> UpdateAsync(EmployeeUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(EmployeeExcelDownloadDto input);
        Task DeleteByIdsAsync(List<Guid> employeeIds);

        Task DeleteAllAsync(GetEmployeesInput input);
        Task<EmployeeDto> UpdateLeaveDaysAsync(Guid id, int leaveDays);
        Task<EmployeeDto> UpdateSalaryAsync(Guid id, decimal salary);
        Task<Pusula.Training.HealthCare.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();
    }
}
