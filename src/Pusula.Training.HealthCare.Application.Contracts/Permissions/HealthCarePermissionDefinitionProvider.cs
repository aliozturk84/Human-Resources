using Pusula.Training.HealthCare.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Pusula.Training.HealthCare.Permissions;

public class HealthCarePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(HealthCarePermissions.GroupName);

        myGroup.AddPermission(HealthCarePermissions.Dashboard.Host, L("Permission:Dashboard"), MultiTenancySides.Host);
        myGroup.AddPermission(HealthCarePermissions.Dashboard.Tenant, L("Permission:Dashboard"), MultiTenancySides.Tenant);

        //Define your own permissions here. Example:
        //myGroup.AddPermission(HealthCarePermissions.MyPermission1, L("Permission:MyPermission1"));

        var patientPermission = myGroup.AddPermission(HealthCarePermissions.Patients.Default, L("Permission:Patients"));
        patientPermission.AddChild(HealthCarePermissions.Patients.Create, L("Permission:Create"));
        patientPermission.AddChild(HealthCarePermissions.Patients.Edit, L("Permission:Edit"));
        patientPermission.AddChild(HealthCarePermissions.Patients.Delete, L("Permission:Delete"));

        var protocolPermission = myGroup.AddPermission(HealthCarePermissions.Protocols.Default, L("Permission:Protocols"));
        protocolPermission.AddChild(HealthCarePermissions.Protocols.Create, L("Permission:Create"));
        protocolPermission.AddChild(HealthCarePermissions.Protocols.Edit, L("Permission:Edit"));
        protocolPermission.AddChild(HealthCarePermissions.Protocols.Delete, L("Permission:Delete"));

        var departmentPermission = myGroup.AddPermission(HealthCarePermissions.Departments.Default, L("Permission:Departments"));
        departmentPermission.AddChild(HealthCarePermissions.Departments.Create, L("Permission:Create"));
        departmentPermission.AddChild(HealthCarePermissions.Departments.Edit, L("Permission:Edit"));
        departmentPermission.AddChild(HealthCarePermissions.Departments.Delete, L("Permission:Delete"));

        var titlePermission = myGroup.AddPermission(HealthCarePermissions.Titles.Default, L("Permission:Titles"));
        titlePermission.AddChild(HealthCarePermissions.Titles.Create, L("Permission:Create"));
        titlePermission.AddChild(HealthCarePermissions.Titles.Edit, L("Permission:Edit"));
        titlePermission.AddChild(HealthCarePermissions.Titles.Delete, L("Permission:Delete"));

        var doctorPermission = myGroup.AddPermission(HealthCarePermissions.Doctors.Default, L("Permission:Doctors"));
        doctorPermission.AddChild(HealthCarePermissions.Doctors.Create, L("Permission:Create"));
        doctorPermission.AddChild(HealthCarePermissions.Doctors.Edit, L("Permission:Edit"));
        doctorPermission.AddChild(HealthCarePermissions.Doctors.Delete, L("Permission:Delete"));

        var hrPermission = myGroup.AddPermission(HealthCarePermissions.HRs.Default, L("Permission:HRs"));
        hrPermission.AddChild(HealthCarePermissions.HRs.Create, L("Permission:Create"));
        hrPermission.AddChild(HealthCarePermissions.HRs.Edit, L("Permission:Edit"));
        hrPermission.AddChild(HealthCarePermissions.HRs.Delete, L("Permission:Delete"));

        var employeePermission = myGroup.AddPermission(HealthCarePermissions.Employees.Default, L("Permission:Employees"));
        employeePermission.AddChild(HealthCarePermissions.Employees.Create, L("Permission:Create"));
        employeePermission.AddChild(HealthCarePermissions.Employees.Edit, L("Permission:Edit"));
        employeePermission.AddChild(HealthCarePermissions.Employees.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<HealthCareResource>(name);
    }
}