using HR.LeaveManagement.Application.DTOs.LeaveRequest;
using HR.LeaveManagement.Domain;

namespace HR.LeaveManagement.Application.Persistence.Contracts
{
    public interface ILeaveTypeRepository : IGenericRepository<LeaveType>
    {
    }
}
