using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Pusula.Training.HealthCare.Doctors;
using Pusula.Training.HealthCare.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;
using Volo.Abp;
using Pusula.Training.HealthCare.Employees;

namespace Pusula.Training.HealthCare.Controllers.Employees
{
    [RemoteService]
    [Area("app")]
    [ControllerName("Employees")]
    [Route("api/app/employees")]
    public class EmployeeController(IEmployeesAppService employeesAppService) : HealthCareController, IEmployeesAppService
    {
        [HttpGet]
        public virtual Task<PagedResultDto<EmployeeWithNavigationPropertiesDto>> GetListAsync(GetEmployeesInput input) => employeesAppService.GetListAsync(input);

        [HttpGet]
        [Route("with-navigation-properties/{id}")]
        public virtual Task<EmployeeWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id) => employeesAppService.GetWithNavigationPropertiesAsync(id);

        [HttpGet]
        [Route("{id}")]
        public virtual Task<EmployeeDto> GetAsync(Guid id) => employeesAppService.GetAsync(id);

        [HttpGet]
        [Route("title-lookup")]
        public virtual Task<PagedResultDto<LookupDto<Guid>>> GetTitleLookupAsync(LookupRequestDto input) => employeesAppService.GetTitleLookupAsync(input);

        [HttpGet]
        [Route("department-lookup")]
        public virtual Task<PagedResultDto<LookupDto<Guid>>> GetDepartmentLookupAsync(LookupRequestDto input) => employeesAppService.GetDepartmentLookupAsync(input);

        [HttpPost]
        public virtual Task<EmployeeDto> CreateAsync(EmployeeCreateDto input) => employeesAppService.CreateAsync(input);

        [HttpPut]
        public virtual Task<EmployeeDto> UpdateAsync(EmployeeUpdateDto input) => employeesAppService.UpdateAsync(input);

        [HttpPut("{id}/leave-days")]
        public virtual Task<EmployeeDto> UpdateLeaveDaysAsync(Guid id, int leaveDays) => employeesAppService.UpdateLeaveDaysAsync(id, leaveDays);

        [HttpPut("{id}/salary")]
        public virtual Task<EmployeeDto> UpdateSalaryAsync(Guid id, decimal salary) => employeesAppService.UpdateSalaryAsync(id, salary);

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id) => employeesAppService.DeleteAsync(id);

        [HttpGet]
        [Route("as-excel-file")]
        public virtual Task<IRemoteStreamContent> GetListAsExcelFileAsync(EmployeeExcelDownloadDto input) => employeesAppService.GetListAsExcelFileAsync(input);

        [HttpGet]
        [Route("download-token")]
        public virtual Task<DownloadTokenResultDto> GetDownloadTokenAsync() => employeesAppService.GetDownloadTokenAsync();

        [HttpDelete]
        [Route("")]
        public virtual Task DeleteByIdsAsync(List<Guid> employeeIds) => employeesAppService.DeleteByIdsAsync(employeeIds);

        [HttpDelete]
        [Route("all")]
        public virtual Task DeleteAllAsync(GetEmployeesInput input) => employeesAppService.DeleteAllAsync(input);
    }
}
