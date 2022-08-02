using HR.LeaveManagement.Application.Contracts.Persistence;
using HR.LeaveManagement.Domain;

namespace HR.LeaveManagement.Persistence.Repositories
{
    public class LeaveTypeRepository : GenericRepository<LeaveType>, ILeaveTypeRepository
    {
        private readonly LeaveManagementDBContext _dbContext;
        public LeaveTypeRepository(LeaveManagementDBContext dbContext): base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
