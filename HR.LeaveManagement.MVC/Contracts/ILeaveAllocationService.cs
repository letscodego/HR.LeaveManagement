using HR.LeaveManagement.MVC.Models;
using HR.LeaveManagement.MVC.Services.Base;

namespace HR.LeaveManagement.MVC.Contracts
{
    public interface ILeaveAllocationService
    {
        Task<List<LeaveAllocationVM>> GetLeaveAllocations();
        Task<LeaveAllocationVM> GetLeaveAllocation(int id);
        Task<Response<int>> CreateLeaveAllocation(int leaveTypeId);
        Task<Response<int>> UpdateLeaveAllocation(LeaveAllocationVM leaveAllocation);
        Task<Response<int>> DeleteLeaveAllocation(int id);
    }
}
