using Microsoft.EntityFrameworkCore;
using SmartEnergy.Domain.Entities;

namespace SmartEnergy.Infrastructure.Persistence;

public class SmartEnergyDbContext : DbContext
{
    public SmartEnergyDbContext(
        DbContextOptions<SmartEnergyDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<Address> Addresses => Set<Address>();

    public DbSet<Subscription> Subscriptions => Set<Subscription>();

    public DbSet<Meter> Meters => Set<Meter>();

    public DbSet<Bill> Bills => Set<Bill>();

    public DbSet<ConsumptionRecord> ConsumptionRecords => Set<ConsumptionRecord>();

    public DbSet<PortalApplication> PortalApplications => Set<PortalApplication>();

    public DbSet<Appointment> Appointments => Set<Appointment>();

    public DbSet<Payment> Payments => Set<Payment>();

    public DbSet<Notification> Notifications => Set<Notification>();

    public DbSet<SecurityDeposit> SecurityDeposits => Set<SecurityDeposit>();

    public DbSet<Compensation> Compensations => Set<Compensation>();

    public DbSet<RefundRequest> RefundRequests => Set<RefundRequest>();

    public DbSet<EmailVerification> EmailVerifications => Set<EmailVerification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(SmartEnergyDbContext).Assembly);
    }
}