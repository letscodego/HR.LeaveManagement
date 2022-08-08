using HR.LeaveManagement.MVC.Models;
using HR.LeaveManagement.MVC.Services.Base;

namespace HR.LeaveManagement.MVC.Contracts
{
    public interface ILeaveRequestService
    {
        Task<List<LeaveRequestVM>> GetLeaveRequests();
        Task<LeaveRequestVM> GetLeaveRequest(int id);
        Task<Response<int>> CreateLeaveRequest(LeaveRequestVM leaveRequest);
        Task<Response<int>> UpdateLeaveRequest(int id, LeaveRequestVM leaveRequest);
        Task<Response<int>> DeleteLeaveRequest(int id);
    }
}
