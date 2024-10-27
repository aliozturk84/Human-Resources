using Microsoft.Extensions.Logging;
using Pusula.Training.HealthCare.Doctors;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace Pusula.Training.HealthCare.Handlers
{
    public class DoctorTitleAndDepartmentEventHandler(ILogger<DoctorTitleAndDepartmentEventHandler> log) : IDistributedEventHandler<DoctorTitleAndDepartmentEto>, ITransientDependency
    {
        public Task HandleEventAsync(DoctorTitleAndDepartmentEto eventData)
        {
            log.LogInformation($" -----> HANDLER -> Doctor {eventData.Title} in {eventData.Department} department.");
            return Task.CompletedTask;
        }
    }
}
