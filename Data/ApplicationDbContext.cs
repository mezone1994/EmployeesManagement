using EmployeesManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeesManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            modelBuilder.Entity<LeaveApplication>()
                .HasOne(d => d.Status)
                .WithMany()
                .HasForeignKey(p => p.StatusID)
                .OnDelete(DeleteBehavior.Cascade);
        }


        public DbSet<Employee> Employees { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Designation> Designations { get; set; }

        public DbSet<Bank> Banks { get; set; }

        public DbSet<SystemCode> SystemCodes { get; set; }

        public DbSet<SystemCodeDetail> SystemCodeDetails { get; set; }

        public DbSet<LeaveType> LeaveTypes { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<LeaveApplication> LeaveApplications { get; set; }

        public DbSet<SystemProfile> SystemProfiles { get; set; }


        public DbSet<Audit> AuditLogs { get; set; }

        public DbSet<RoleProfile> RoleProfiles { get; set; }

        public DbSet<Holiday> Holidays { get; set; }


        public virtual async Task<int> SaveChangesAsync(string userId = null)
        {
            OnBeforeSaveChanges(userId);

            var result = await base.SaveChangesAsync();

            return result;
        }

        private void OnBeforeSaveChanges(string userId)
        {
            ChangeTracker.DetectChanges();

            var auditEntries = new List<AuditEntry>();
            var entries = ChangeTracker.Entries().ToList(); // Materialize the collection to prevent modification during enumeration
            foreach (var enrty in entries)
            {
                if (enrty.Entity is Audit || enrty.State == EntityState.Detached || enrty.State == EntityState.Unchanged)
                    continue;
                var auditEntry = new AuditEntry(enrty);
                auditEntry.TableName = enrty.Entity.GetType().Name;
                auditEntry.UserId = userId;

                auditEntries.Add(auditEntry);

                foreach (var property in enrty.Properties)
                {
                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }
                    switch (enrty.State)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = AuditType.Create;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;
                        case EntityState.Deleted:
                            auditEntry.AuditType = AuditType.Delete;
                            auditEntry.OldValues[propertyName] = property.CurrentValue;
                            break;
                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.ChangedColumns.Add(propertyName);
                                auditEntry.AuditType = AuditType.Update;
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }
            }

            foreach (var auditentry in auditEntries)
            {
                AuditLogs.Add(auditentry.ToAudit());
            }
        }
    }
}
