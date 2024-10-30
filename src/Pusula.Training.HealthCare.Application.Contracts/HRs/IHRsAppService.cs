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

namespace Pusula.Training.HealthCare.HRs
{
    public interface IHRsAppService : IApplicationService
    {
        Task<PagedResultDto<HRWithNavigationPropertiesDto>> GetListAsync(GetHRsInput input);

        Task<HRWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id);

        Task<HRDto> GetAsync(Guid id);

        Task<PagedResultDto<LookupDto<Guid>>> GetTitleLookupAsync(LookupRequestDto input);

        Task<PagedResultDto<LookupDto<Guid>>> GetDepartmentLookupAsync(LookupRequestDto input);

        Task DeleteAsync(Guid id);

        Task<HRDto> CreateAsync(HRCreateDto input);

        Task<HRDto> UpdateAsync(HRUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(HRExcelDownloadDto input);
        Task DeleteByIdsAsync(List<Guid> hrIds);

        Task DeleteAllAsync(GetHRsInput input);
        Task<Pusula.Training.HealthCare.Shared.DownloadTokenResultDto> GetDownloadTokenAsync();
    }
}
