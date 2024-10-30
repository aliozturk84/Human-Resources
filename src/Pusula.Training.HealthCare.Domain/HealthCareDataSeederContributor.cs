using Pusula.Training.HealthCare.Departments;
using Pusula.Training.HealthCare.Employees;
using Pusula.Training.HealthCare.HRs;
using Pusula.Training.HealthCare.Patients;
using Pusula.Training.HealthCare.Protocols;
using Pusula.Training.HealthCare.Titles;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace Pusula.Training.HealthCare
{
    public class HealthCareDataSeederContributor(IDepartmentRepository departmentRepository,
        IPatientRepository patientRepository,
        IEmployeeRepository employeeRepository,
        IProtocolRepository protocolRepository,
        ITitleRepository titleRepository,
        IHRRepository hrRepository,
        IGuidGenerator guidGenerator) : IDataSeedContributor, ITransientDependency
    {
        public async Task SeedAsync(DataSeedContext context)
        {
            if (await hrRepository.GetCountAsync() > 0) return;

            var department = await departmentRepository.InsertAsync(new Department(guidGenerator.Create(), "İnsan Kaynakları"), true);
            var department2 = await departmentRepository.InsertAsync(new Department(guidGenerator.Create(), "Bilgi Teknolojileri"), true);
            var department3 = await departmentRepository.InsertAsync(new Department(guidGenerator.Create(), "Satış ve Pazarlama"), true);

            var title = await titleRepository.InsertAsync(new Title(guidGenerator.Create(), "İnsan Kaynakları Uzmanı"), true);
            var title2 = await titleRepository.InsertAsync(new Title(guidGenerator.Create(), "Yazılım Mühendisi"), true);
            var title3 = await titleRepository.InsertAsync(new Title(guidGenerator.Create(), "Satış ve Pazarlama Uzmanı"), true);

            var patient = await patientRepository.InsertAsync(new Patient(guidGenerator.Create(), "Ali", "Yılmaz", new System.DateTime(2020, 01, 01), "49762602911", "ali.yilmaz@gmail.com", "5111000515", 1), true);
            var patient2 = await patientRepository.InsertAsync(new Patient(guidGenerator.Create(), "Mehmet", "Kaya", new System.DateTime(1990, 05, 20), "18594105779", "mehmet.kaya@hotmail.com", "5091059875", 1), true);
            var patient3 = await patientRepository.InsertAsync(new Patient(guidGenerator.Create(), "Veli", "Yıldız", new System.DateTime(1995, 04, 15), "75710296501", "veli.yildiz@outlook.com.tr", "5059800099", 1), true);


            var employee = await employeeRepository.InsertAsync(new Employee(guidGenerator.Create(), title.Id, department.Id, "Ali", "Yılmaz", "5111000515", new System.DateTime(2020, 01, 01), 10, 5000), true);
            var employee2 = await employeeRepository.InsertAsync(new Employee(guidGenerator.Create(), title2.Id, department2.Id, "Mehmet", "Kaya", "5091059875", new System.DateTime(1990, 05, 20), 15, 7000), true);
            var employee3 = await employeeRepository.InsertAsync(new Employee(guidGenerator.Create(), title3.Id, department3.Id, "Veli", "Yıldız", "5059800099", new System.DateTime(1995, 04, 15), 12, 6000), true);

            var protocol = await protocolRepository.InsertAsync(new Protocol(guidGenerator.Create(), patient.Id, department.Id, "Yatan", new System.DateTime(2024, 10, 22)), true);
            var protocol2 = await protocolRepository.InsertAsync(new Protocol(guidGenerator.Create(), patient2.Id, department2.Id, "Ayaktan", new System.DateTime(2024, 10, 23), "2024-10-23"), true);
            var protocol3 = await protocolRepository.InsertAsync(new Protocol(guidGenerator.Create(), patient3.Id, department3.Id, "Yatan", new System.DateTime(2024, 10, 16)), true);

            //var protocol = await protocolRepository.InsertAsync(new Protocol(guidGenerator.Create(), employee.Id, department.Id, "Yeni Başlayan Değil", new System.DateTime(2020, 10, 22)), true);
            //var protocol2 = await protocolRepository.InsertAsync(new Protocol(guidGenerator.Create(), employee2.Id, department2.Id, "Yeni Başlayan Değil", new System.DateTime(2021, 10, 23)), true);
            //var protocol3 = await protocolRepository.InsertAsync(new Protocol(guidGenerator.Create(), employee3.Id, department3.Id, "Yeni Başlayan", new System.DateTime(2024, 10, 16)), true);

            var hr = new HR(guidGenerator.Create(), title.Id, department.Id, "Necati", "Ateş", "5305538621", new System.DateTime(1978, 5, 19), EnumGender.Male);
            var hr2 = new HR(guidGenerator.Create(), title2.Id, department2.Id, "Hatice", "Kaya", "5358900011", new System.DateTime(1990, 4, 8), EnumGender.Female);
            var hr3 = new HR(guidGenerator.Create(), title3.Id, department3.Id, "Adnan", "Yıldızoğlu", "5119859900", new System.DateTime(1980, 1, 3), EnumGender.Male);

            await hrRepository.InsertManyAsync([hr, hr2, hr3], true);
        }
    }
}
