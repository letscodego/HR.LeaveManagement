﻿using HR.LeaveManagement.MVC.Models;
using HR.LeaveManagement.MVC.Services.Base;

namespace HR.LeaveManagement.MVC.Contracts
{
    public interface ILeaveTypeService
    {
        Task<List<LeaveTypeVM>> GetLeaveTypes();
        Task<LeaveTypeVM> GetLeaveType(int id);
        Task<Response<int>> CreateLeaveType(LeaveTypeVM leaveType);
        Task<Response<int>> UpdateLeaveType(LeaveTypeVM leaveType);
        Task<Response<int>> DeleteLeaveType(int id);
    }
}
