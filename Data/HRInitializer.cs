using EmployeesManagement.Models;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;

namespace EmployeesManagement.Data
{
    public static class HRInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                try
                {
                    SeedCountries(context);
                    SeedCities(context);
                    SeedBanks(context);
                    //SeedEmployees(context);
                    SeedLeaveTypes(context);
                    SeedSystemCodes(context);
                    SeedSystemCodesDetails(context);
                    SeedDepartments(context);
                    SeedDesignations(context);
                    SeedSystemProfiles(context);
                    SeedRoles(context);
                    // Add more seeding methods for other entities if needed
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.GetBaseException().Message);
                }
            }
        }

        private static void SeedCountries(ApplicationDbContext context)
        {
            if (!context.Countries.Any())
            {
                var canada = new Country
                {
                    Code = "CA",
                    Name = "Canada"
                };

                context.Countries.Add(canada);
                context.SaveChanges();
            }
        }

        private static void SeedCities(ApplicationDbContext context)
        {
            if (!context.Cities.Any())
            {
                var canada = context.Countries.FirstOrDefault(c => c.Code == "CA");
                if (canada != null)
                {
                    var cities = new List<City>
                    {
                        new City { Code = "TOR", Name = "Toronto", CountryId = canada.Id },
                        new City { Code = "VAN", Name = "Vancouver", CountryId = canada.Id },
                        new City { Code = "MTL", Name = "Montreal", CountryId = canada.Id }
                    };

                    context.Cities.AddRange(cities);
                    context.SaveChanges();
                }
            }
        }

        private static void SeedBanks(ApplicationDbContext context)
        {
            if (!context.Banks.Any())
            {
                var banks = new List<Bank>
                {
                    new Bank { Code = "TD", Name = "Toronto Dominion Bank", AccountNo = "1234567890" },
                    new Bank { Code = "RBC", Name = "Royal Bank of Canada", AccountNo = "0987654321" },
                    new Bank { Code = "BMO", Name = "Bank of Montreal", AccountNo = "5678901234" }
                };

                context.Banks.AddRange(banks);
                context.SaveChanges();
            }
        }

        //private static void SeedEmployees(ApplicationDbContext context)
        //{
        //    if (!context.Employees.Any())
        //    {
        //        var employee = new Employee
        //        {
        //            EmpNo = "EMP001",
        //            FirstName = "John",
        //            LastName = "Doe",
        //            PhoneNumber = "1234567890",
        //            EmailAddress = "johndoe@example.com",
        //            Country = "Canada",
        //            DateOfBirth = DateTime.Parse("1990-01-01"),
        //            Address = "123 Main St, Toronto",
        //            Department = "HR",
        //            Designation = "Manager"
        //        };

        //        context.Employees.Add(employee);
        //        context.SaveChanges();
        //    }
        //}

        private static void SeedLeaveTypes(ApplicationDbContext context)
        {
            if (!context.LeaveTypes.Any())
            {
                var leaveTypes = new List<LeaveType>
                {
                    new LeaveType { Code = "VL", Name = "Vacation Leave" },
                    new LeaveType { Code = "SL", Name = "Sick Leave" },
                    new LeaveType { Code = "ML", Name = "Maternity Leave" }
                };

                context.LeaveTypes.AddRange(leaveTypes);
                context.SaveChanges();
            }
        }

        private static void SeedSystemCodes(ApplicationDbContext context)
        {
            if (!context.SystemCodes.Any())
            {
                var systemCodes = new List<SystemCode>
                {
                    new SystemCode { Code = "LeaveDuration", Description = "LeaveDuration"},
                    new SystemCode { Code = "LeaveApprovalStatus", Description = "LeaveApprovalStatus" },
                    new SystemCode { Code = "Gender", Description = "Gender" }
                };

                context.SystemCodes.AddRange(systemCodes);
                context.SaveChanges();
            }
        }

        private static void SeedSystemCodesDetails(ApplicationDbContext context)
        {
            if (!context.SystemCodeDetails.Any())
            {
                var systemCodesdetails = new List<SystemCodeDetail>
                {
                    new SystemCodeDetail { Code = "Pending", Description = "Pending", SystemCodeId = 2 , OrderNo = 1},
                    new SystemCodeDetail { Code = "Approved", Description = "Approved", SystemCodeId = 2 , OrderNo = 2},
                    new SystemCodeDetail { Code = "Rejected", Description = "Rejected", SystemCodeId = 2 , OrderNo = 3},
                    new SystemCodeDetail { Code = "Male", Description = "Male", SystemCodeId = 3 , OrderNo = 1},
                    new SystemCodeDetail { Code = "Female", Description = "Female", SystemCodeId = 3 , OrderNo = 2},
                    new SystemCodeDetail { Code = "FullDay", Description = "FullDay", SystemCodeId = 1 , OrderNo = 1},
                    new SystemCodeDetail { Code = "HalfDay", Description = "HalfDay", SystemCodeId = 1 , OrderNo = 2}
                };

                context.SystemCodeDetails.AddRange(systemCodesdetails);
                context.SaveChanges();
            }
        }

        private static void SeedDepartments(ApplicationDbContext context)
        {
            if (!context.Departments.Any())
            {
                var departments = new List<Department>
                {
                    new Department { Code = "HR", Name = "Human Resources" },
                    new Department { Code = "IT", Name = "Information Technology" },
                    new Department { Code = "FIN", Name = "Finance" }
                };

                context.Departments.AddRange(departments);
                context.SaveChanges();
            }
        }

        private static void SeedDesignations(ApplicationDbContext context)
        {
            if (!context.Designations.Any())
            {
                var designations = new List<Designation>
                {
                    new Designation { Code = "IT", Name = "IT Manager" },
                    new Designation { Code = "HR", Name = "HR Manager" },
                    new Designation { Code = "FN", Name = "Finance Manager" },
                    new Designation { Code = "IO", Name = "IT Officer" },
                    new Designation { Code = "HO", Name = "HR Officer" },
                    new Designation { Code = "FO", Name = "Finance Officer" }
                };

                context.Designations.AddRange(designations);
                context.SaveChanges();
            }
        }

        private static void SeedSystemProfiles(ApplicationDbContext context)
        {
            if (!context.SystemProfiles.Any())
            {
                
                var systemprofiles = new List<SystemProfile>
                {
                    new SystemProfile { Id = 1, Name = "Dashboard", Order = 1 },
                    new SystemProfile { Id = 2, Name = "EmployeesManagement", Order = 1 },
                    new SystemProfile { Id = 3, Name = "LeaveManagement", Order = 1 },
                    new SystemProfile { Id = 4, Name = "SystemSecurity", Order = 1 },
                    new SystemProfile { Id = 5, Name = "SystemAdministrator", Order = 1 },

                };

                context.SystemProfiles.AddRange(systemprofiles);
                context.SaveChanges();
            }
        }

        private static void SeedRoles(ApplicationDbContext context)
        {
            if (!context.Roles.Any())
            {
                var roles = new List<IdentityRole>
                {
                    new IdentityRole {Name = "Super Admin"},
                    new IdentityRole {Name = "IT Manager"},
                    new IdentityRole {Name = "Finance Manager"},
                    new IdentityRole {Name = "HR Manager"},
                };
                context.Roles.AddRange(roles);
                context.SaveChanges();
            }
        }
    }
}
