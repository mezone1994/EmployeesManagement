using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeesManagement.Data;
using EmployeesManagement.Models;
using System.Security.Claims;

namespace EmployeesManagement.Controllers
{
    public class LeaveApplicationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LeaveApplicationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LeaveApplications
        public async Task<IActionResult> Index()
        {
            var awaitingStatus = await _context.SystemCodeDetails
                .Include(x => x.SystemCode)
                .FirstOrDefaultAsync(y => y.SystemCode.Code == "LeaveApprovalStatus" && y.Code == "Pending");

            if (awaitingStatus == null)
            {
                // Handle the case when awaitingStatus is null, perhaps by returning an empty view or showing an error message
                return View(new List<LeaveApplication>()); // Empty list of leave applications
            }

            var employeesContext = _context.LeaveApplications
                .Include(l => l.Duration)
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .Include(l => l.Status)
                .Where(l => l.StatusID == awaitingStatus.Id);

            return View(await employeesContext.ToListAsync());
        }


        public async Task<IActionResult> ApprovedLeaveApplications()
        {
            var approvedStatus = await _context.SystemCodeDetails
                .Include(x => x.SystemCode)
                .FirstOrDefaultAsync(y => y.SystemCode.Code == "LeaveApprovalStatus" && y.Code == "Approved");

            if (approvedStatus == null)
            {
                // Handle the case when approvedStatus is null, perhaps by returning an empty view or showing an error message
                return View(new List<LeaveApplication>()); // Empty list of leave applications
            }

            var employeesContext = _context.LeaveApplications
                .Include(l => l.Duration)
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .Include(l => l.Status)
                .Where(l => l.StatusID == approvedStatus.Id);

            return View(await employeesContext.ToListAsync());
        }

        public async Task<IActionResult> RejectedLeaveApplications()
        {
            var rejectedStatus = await _context.SystemCodeDetails
                .Include(x => x.SystemCode)
                .FirstOrDefaultAsync(y => y.SystemCode.Code == "LeaveApprovalStatus" && y.Code == "Rejected");

            if (rejectedStatus == null)
            {
                // Handle the case when rejectedStatus is null, perhaps by returning an empty view or showing an error message
                return View(new List<LeaveApplication>()); // Empty list of leave applications
            }

            var employeesContext = _context.LeaveApplications
                .Include(l => l.Duration)
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .Include(l => l.Status)
                .Where(l => l.StatusID == rejectedStatus.Id);

            return View(await employeesContext.ToListAsync());
        }



        // GET: LeaveApplications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.LeaveApplications
                .Include(l => l.Duration)
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .Include(l => l.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leaveApplication == null)
            {
                return NotFound();
            }

            return View(leaveApplication);
        }

        // GET: LeaveApplications/Create
        public IActionResult Create()
        {
            ViewData["DurationID"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(y => y.SystemCode.Code == "LeaveDuration"), "Id", "Description");
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName");
            ViewData["LeaveTypeID"] = new SelectList(_context.LeaveTypes, "Id", "Name");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ApproveLeave(int? id)
        {
            var leaveApplication = await _context.LeaveApplications
                .Include (l => l.Duration)
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .Include (l => l.Status)
                .FirstOrDefaultAsync (m => m.Id == id);
            if (leaveApplication == null) { return NotFound(); }

            ViewData["DurationID"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(y => y.SystemCode.Code == "LeaveDuration"), "Id", "Description");
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName");
            ViewData["LeaveTypeID"] = new SelectList(_context.LeaveTypes, "Id", "Name");
            return View(leaveApplication);
        }
        [HttpPost]
        public async Task<IActionResult> ApproveLeave(LeaveApplication leave)
        {
            var approvedStatus = await _context.SystemCodeDetails
                .Include(x => x.SystemCode)
                .FirstOrDefaultAsync(y => y.SystemCode.Code == "LeaveApprovalStatus" && y.Code == "Approved");

            var leaveApplication = await _context.LeaveApplications
                .Include(l => l.Duration)
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .Include(l => l.Status)
                .FirstOrDefaultAsync(m => m.Id == leave.Id);
            if (leaveApplication == null) { return NotFound(); }

            var Userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            leaveApplication.ApprovedOn = DateTime.Now;
            leaveApplication.ApprovedByID = Userid;
            leaveApplication.StatusID = approvedStatus!.Id;
            leaveApplication.ApprovalNotes = leave.ApprovalNotes;

            _context.Update(leaveApplication);
            await _context.SaveChangesAsync(Userid);

            ViewData["DurationID"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(y => y.SystemCode.Code == "LeaveDuration"), "Id", "Description");
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName");
            ViewData["LeaveTypeID"] = new SelectList(_context.LeaveTypes, "Id", "Name");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> RejectLeave(int? id)
        {
            var leaveApplication = await _context.LeaveApplications
                .Include(l => l.Duration)
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .Include(l => l.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leaveApplication == null) { return NotFound(); }

            ViewData["DurationID"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(y => y.SystemCode.Code == "LeaveDuration"), "Id", "Description");
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName");
            ViewData["LeaveTypeID"] = new SelectList(_context.LeaveTypes, "Id", "Name");
            return View(leaveApplication);
        }
        [HttpPost]
        public async Task<IActionResult> RejectLeave(LeaveApplication leave)
        {
            var rejectedStatus = await _context.SystemCodeDetails
                .Include(x => x.SystemCode)
                .FirstOrDefaultAsync(y => y.SystemCode.Code == "LeaveApprovalStatus" && y.Code == "Rejected");

            var leaveApplication = await _context.LeaveApplications
                .Include(l => l.Duration)
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .Include(l => l.Status)
                .FirstOrDefaultAsync(m => m.Id == leave.Id);
            if (leaveApplication == null) { return NotFound(); }

            var Userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            leaveApplication.ApprovedOn = DateTime.Now;
            leaveApplication.ApprovedByID = Userid;
            leaveApplication.StatusID = rejectedStatus.Id;
            leaveApplication.ApprovalNotes = leave.ApprovalNotes;

            _context.Update(leaveApplication);
            await _context.SaveChangesAsync(Userid);

            ViewData["DurationID"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(y => y.SystemCode.Code == "LeaveDuration"), "Id", "Description");
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName");
            ViewData["LeaveTypeID"] = new SelectList(_context.LeaveTypes, "Id", "Name");
            return RedirectToAction(nameof(Index));
        }

        // POST: LeaveApplications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveApplication leaveApplication)
        {
            
            var pendingStatus = await _context.SystemCodeDetails.Include(x => x.SystemCode).Where(y => y.Code == "Pending" && y.SystemCode.Code == "LeaveApprovalStatus").FirstOrDefaultAsync();
            if (ModelState.IsValid)
            {
                var Userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                leaveApplication.StatusID = pendingStatus.Id;
                leaveApplication.CreatedByID = Userid;
                leaveApplication.CreatedOn = DateTime.Now;
                _context.Add(leaveApplication);
                await _context.SaveChangesAsync(Userid);
                return RedirectToAction(nameof(Index));
            }
            ViewData["DurationID"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(y => y.SystemCode.Code == "LeaveDuration"), "Id", "Description", leaveApplication.DurationID);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", leaveApplication.EmployeeId);
            ViewData["LeaveTypeID"] = new SelectList(_context.LeaveTypes, "Id", "Name", leaveApplication.LeaveTypeID);
            return View(leaveApplication);
        }

        // GET: LeaveApplications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.LeaveApplications.FindAsync(id);
            if (leaveApplication == null)
            {
                return NotFound();
            }
            ViewData["DurationID"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(y => y.SystemCode.Code == "LeaveDuration"), "Id", "Description", leaveApplication.DurationID);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", leaveApplication.EmployeeId);
            ViewData["LeaveTypeID"] = new SelectList(_context.LeaveTypes, "Id", "Name", leaveApplication.LeaveTypeID);
            return View(leaveApplication);
        }

        // POST: LeaveApplications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LeaveApplication leaveApplication)
        {
            if (id != leaveApplication.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                var pendingStatus = await _context.SystemCodeDetails.Include(x => x.SystemCode).Where(y => y.Code == "Pending" && y.SystemCode.Code == "LeaveApprovalStatus").FirstOrDefaultAsync();

                try
                {
                    var Userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    leaveApplication.ModifiedOn = DateTime.Now;
                    leaveApplication.ModifiedByID = Userid;
                    var oldapp = await _context.LeaveApplications.FindAsync(id);
                    _context.Entry(oldapp).CurrentValues.SetValues(leaveApplication);
                    await _context.SaveChangesAsync(Userid);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveApplicationExists(leaveApplication.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DurationID"] = new SelectList(_context.SystemCodeDetails.Include(x => x.SystemCode).Where(y => y.SystemCode.Code == "LeaveDuration"), "Id", "Description", leaveApplication.DurationID);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", leaveApplication.EmployeeId);
            ViewData["LeaveTypeID"] = new SelectList(_context.LeaveTypes, "Id", "Name", leaveApplication.LeaveTypeID);
            return View(leaveApplication);
        }

        // GET: LeaveApplications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.LeaveApplications
                .Include(l => l.Duration)
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .Include(l => l.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leaveApplication == null)
            {
                return NotFound();
            }

            return View(leaveApplication);
        }

        // POST: LeaveApplications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leaveApplication = await _context.LeaveApplications.FindAsync(id);
            if (leaveApplication != null)
            {
                _context.LeaveApplications.Remove(leaveApplication);
            }
            var Userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _context.SaveChangesAsync(Userid);
            return RedirectToAction(nameof(Index));
        }

        private bool LeaveApplicationExists(int id)
        {
            return _context.LeaveApplications.Any(e => e.Id == id);
        }
    }
}
