using HR.LeaveManagement.MVC.Contracts;
using HR.LeaveManagement.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HR.LeaveManagement.MVC.Controllers;

[Authorize(Roles = "Administrator")]
public class LeaveTypesController : Controller
{
    public ILeaveTypeService LeaveTypeService { get; }
    public ILeaveAllocationService LeaveAllocationService { get; }

    public LeaveTypesController(ILeaveTypeService leaveTypeService,
        ILeaveAllocationService leaveAllocationService)
    {
        LeaveTypeService = leaveTypeService;
        LeaveAllocationService = leaveAllocationService;
    }
    // GET: LeaveTypesController
    public async Task<ActionResult> Index()
    {
        var model = await LeaveTypeService.GetLeaveTypes();
        return View(model);
    }

    // GET: LeaveTypesController/Details/5
    public async Task<ActionResult> Details(int id)
    {
        var leaveType = await LeaveTypeService.GetLeaveType(id);
        return View(leaveType);
    }

    // GET: LeaveTypesController/Create
    public async Task<ActionResult> Create()
    {
        return View();
    }

    // POST: LeaveTypesController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(CreateLeaveTypeVM leaveType)
    {
        try
        {
            var response = await LeaveTypeService.CreateLeaveType(leaveType);
            if (response.Success)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", response.ValidationErrors);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
        }
        return View(leaveType);
    }

    // GET: LeaveTypesController/Edit/5
    public async Task<ActionResult> Edit(int id)
    {
        var leaveType = await LeaveTypeService.GetLeaveType(id);
        return View(leaveType);
    }

    // POST: LeaveTypesController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(LeaveTypeVM leaveType)
    {
        try
        {
            var response = await LeaveTypeService.UpdateLeaveType(leaveType);
            if (response.Success)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", response.ValidationErrors);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
        }
        return View(leaveType);
    }

    // POST: LeaveTypesController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var response = await LeaveTypeService.DeleteLeaveType(id);
            if (response.Success)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", response.ValidationErrors);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
        }
        return View();
    }

    // POST: LeaveTypesController/Allocate/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Allocate(int id)
    {
        try
        {
            var response = await LeaveAllocationService.CreateLeaveAllocation(id);
            if (response.Success) { return RedirectToAction(nameof(Index)); }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
        }
        return BadRequest();
    }
}