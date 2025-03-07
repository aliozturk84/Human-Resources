﻿using Microsoft.EntityFrameworkCore;
using Pusula.Training.HealthCare.Departments;
using Pusula.Training.HealthCare.Doctors;
using Pusula.Training.HealthCare.Employees;
using Pusula.Training.HealthCare.HRs;
using Pusula.Training.HealthCare.Patients;
using Pusula.Training.HealthCare.Protocols;
using Pusula.Training.HealthCare.Titles;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace Pusula.Training.HealthCare.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class HealthCareDbContext :
    AbpDbContext<HealthCareDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */
    public DbSet<Department> Departments { get; set; } = null!;
    public DbSet<Protocol> Protocols { get; set; } = null!;
    public DbSet<Patient> Patients { get; set; } = null!;
    public DbSet<Title> Titles { get; set; } = null!;
    public DbSet<Doctor> Doctors { get; set; } = null!;
    public DbSet<HR> HRs { get; set; } = null!;
    public DbSet<Employee> Employees { get; set; } = null!;


    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    //Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }
    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    public HealthCareDbContext(DbContextOptions<HealthCareDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        /* Configure your own tables/entities inside here */
        if (builder.IsHostDatabase())
        {
            builder.Entity<Patient>(b =>
            {
                b.ToTable(HealthCareConsts.DbTablePrefix + "Patients", HealthCareConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.FirstName).HasColumnName(nameof(Patient.FirstName)).IsRequired().HasMaxLength(PatientConsts.FirstNameMaxLength);
                b.Property(x => x.LastName).HasColumnName(nameof(Patient.LastName)).IsRequired().HasMaxLength(PatientConsts.LastNameMaxLength);
                b.Property(x => x.BirthDate).HasColumnName(nameof(Patient.BirthDate));
                b.Property(x => x.IdentityNumber).HasColumnName(nameof(Patient.IdentityNumber)).IsRequired().HasMaxLength(PatientConsts.IdentityNumberMaxLength);
                b.Property(x => x.EmailAddress).HasColumnName(nameof(Patient.EmailAddress)).IsRequired().HasMaxLength(PatientConsts.EmailAddressMaxLength);
                b.Property(x => x.MobilePhoneNumber).HasColumnName(nameof(Patient.MobilePhoneNumber)).IsRequired().HasMaxLength(PatientConsts.MobilePhoneNumberMaxLength);
                b.Property(x => x.HomePhoneNumber).HasColumnName(nameof(Patient.HomePhoneNumber));
                b.Property(x => x.Gender).HasColumnName(nameof(Patient.Gender)).IsRequired().HasMaxLength(PatientConsts.GenderMaxLength);
            });

            builder.Entity<Department>(b =>
            {
                b.ToTable(HealthCareConsts.DbTablePrefix + "Departments", HealthCareConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Name).HasColumnName(nameof(Department.Name)).IsRequired().HasMaxLength(DepartmentConsts.NameMaxLength);
            });

            builder.Entity<Protocol>(b =>
            {
                b.ToTable(HealthCareConsts.DbTablePrefix + "Protocols", HealthCareConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Type).HasColumnName(nameof(Protocol.Type)).IsRequired().HasMaxLength(ProtocolConsts.TypeMaxLength);
                b.Property(x => x.StartTime).HasColumnName(nameof(Protocol.StartTime));
                b.Property(x => x.EndTime).HasColumnName(nameof(Protocol.EndTime));
                b.HasOne<Patient>().WithMany().IsRequired().HasForeignKey(x => x.PatientId).OnDelete(DeleteBehavior.NoAction);
                b.HasOne<Department>().WithMany().IsRequired().HasForeignKey(x => x.DepartmentId).OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<Title>(b =>
            {
                b.ToTable(HealthCareConsts.DbTablePrefix + "Titles", HealthCareConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.Name).HasColumnName(nameof(Title.Name)).IsRequired().HasMaxLength(TitleConsts.NameMaxLength);
            });

            builder.Entity<Doctor>(b =>
            {
                b.ToTable(HealthCareConsts.DbTablePrefix + "Doctors", HealthCareConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.FirstName).HasColumnName(nameof(Doctor.FirstName)).IsRequired().HasMaxLength(DoctorConsts.FirstNameMaxLength);
                b.Property(x => x.LastName).HasColumnName(nameof(Doctor.LastName)).IsRequired().HasMaxLength(DoctorConsts.LastNameMaxLength);
                b.Property(x => x.PhoneNumber).HasColumnName(nameof(Doctor.PhoneNumber)).IsRequired().HasMaxLength(DoctorConsts.PhoneNumberMaxLength);
                b.Property(x => x.BirthDate).HasColumnName(nameof(Doctor.BirthDate)).IsRequired();
                b.Property(x => x.Gender).HasColumnName(nameof(Doctor.Gender));
                b.HasOne<Department>().WithMany().IsRequired().HasForeignKey(x => x.DepartmentId).OnDelete(DeleteBehavior.NoAction);
                b.HasOne<Title>().WithMany().IsRequired().HasForeignKey(x => x.TitleId).OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<HR>(b =>
            {
                b.ToTable(HealthCareConsts.DbTablePrefix + "HRs", HealthCareConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.FirstName).HasColumnName(nameof(HR.FirstName)).IsRequired().HasMaxLength(HRConsts.FirstNameMaxLength);
                b.Property(x => x.LastName).HasColumnName(nameof(HR.LastName)).IsRequired().HasMaxLength(HRConsts.LastNameMaxLength);
                b.Property(x => x.PhoneNumber).HasColumnName(nameof(HR.PhoneNumber)).IsRequired().HasMaxLength(HRConsts.PhoneNumberMaxLength);
                b.Property(x => x.BirthDate).HasColumnName(nameof(HR.BirthDate)).IsRequired();
                b.Property(x => x.Gender).HasColumnName(nameof(HR.Gender));
                b.HasOne<Department>().WithMany().IsRequired().HasForeignKey(x => x.DepartmentId).OnDelete(DeleteBehavior.NoAction);
                b.HasOne<Title>().WithMany().IsRequired().HasForeignKey(x => x.TitleId).OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<Employee>(b =>
            {
                b.ToTable(HealthCareConsts.DbTablePrefix + "Employees", HealthCareConsts.DbSchema);
                b.ConfigureByConvention();
                b.Property(x => x.FirstName).HasColumnName(nameof(Employee.FirstName)).IsRequired().HasMaxLength(EmployeeConsts.FirstNameMaxLength);
                b.Property(x => x.LastName).HasColumnName(nameof(Employee.LastName)).IsRequired().HasMaxLength(EmployeeConsts.LastNameMaxLength);
                b.Property(x => x.PhoneNumber).HasColumnName(nameof(Employee.PhoneNumber)).IsRequired().HasMaxLength(EmployeeConsts.PhoneNumberMaxLength);
                b.Property(x => x.BirthDate).HasColumnName(nameof(Employee.BirthDate)).IsRequired();
                b.Property(x => x.LeaveDays).HasColumnName(nameof(Employee.LeaveDays)).IsRequired();
                b.Property(x => x.Salary).HasColumnName(nameof(Employee.Salary)).IsRequired();
                b.HasOne<Department>().WithMany().IsRequired().HasForeignKey(x => x.DepartmentId).OnDelete(DeleteBehavior.NoAction);
                b.HasOne<Title>().WithMany().IsRequired().HasForeignKey(x => x.TitleId).OnDelete(DeleteBehavior.NoAction);
            });
        }

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(HealthCareConsts.DbTablePrefix + "YourEntities", HealthCareConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});
    }
}
